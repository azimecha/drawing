﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace TestWinApp {
    public partial class TestForm : Form {
        private Azimecha.Drawing.IBitmap _bmImage, _bmCopy;
        private BitmapInfoHeader _bmih = new BitmapInfoHeader();
        private Azimecha.Drawing.ISolidBrush _brRed;
        private Azimecha.Drawing.IBrush _brCur;
        private Azimecha.Drawing.IDrawingContext _ctx;

        public TestForm() {
            InitializeComponent();
        }

        private void TestForm_Load(object sender, EventArgs e) {
            byte[] arrData = GetImageData(Properties.Resources.cringe);
            _bmImage = Azimecha.Drawing.AGG.Bitmap.CreateOnArray(Properties.Resources.cringe.Width, Properties.Resources.cringe.Height, arrData);
            _bmCopy = _bmImage.Duplicate();

            _bmih.biSize = (uint)Marshal.SizeOf(_bmih);
            _bmih.biWidth = _bmCopy.Width;
            _bmih.biHeight = -_bmCopy.Height;
            _bmih.biPlanes = 1;
            _bmih.biBitCount = 32;
            _bmih.biSizeImage = (uint)_bmCopy.DataSize;

            _brRed = new Azimecha.Drawing.AGG.SolidBrush(255, 0, 0, 128);

            _ctx = _bmCopy.CreateContext();
        }

        [DllImport("gdi32")]
        private static extern int SetDIBitsToDevice(IntPtr hDC, int nDestX, int nDestY, uint w, uint h, uint nSrcX, uint nSrcY, uint nStartLine,
            uint nLines, IntPtr pBits, [In] ref BitmapInfoHeader bmih, uint nColorUse);

        private void TestForm_Paint(object sender, PaintEventArgs e) {
            System.Drawing.Graphics gfx = e.Graphics;

            IntPtr hDC = gfx.GetHdc();
            try {
                using (Azimecha.Drawing.IBitmapDataAccessor data = _bmCopy.AccessData(true, false)) {
                    int n = SetDIBitsToDevice(hDC, 0, 0, (uint)_bmCopy.Width, (uint)_bmCopy.Height, 0, 0, 0, (uint)_bmCopy.Height, 
                        data.Pointer, ref _bmih, 0);
                    Debug.WriteLine($"SetDIBitsToDevice: {n}");
                }
            } finally {
                gfx.ReleaseHdc(hDC);
            }

            gfx.ExcludeClip(new System.Drawing.Rectangle(0, 0, _bmCopy.Width, _bmCopy.Height));
            base.OnPaintBackground(new PaintEventArgs(gfx, e.ClipRectangle));
        }

        private void TestForm_MouseMove(object sender, MouseEventArgs e) {
            if (((e.Button & MouseButtons.Left) != 0) && !(_brCur is null)) {
                _ctx.FillCircle(_brCur, e.X, e.Y, 50.0f);
                Invalidate();
            }
        }

        private void RedBrushBtn_Click(object sender, EventArgs e) {
            _brCur = _brRed;
        }

        [DllImport("kernel32")]
        private static extern void RtlMoveMemory(IntPtr pDest, IntPtr pSrc, IntPtr nLength);

        private void ClearBtn_Click(object sender, EventArgs e) {
            using (Azimecha.Drawing.IBitmapDataAccessor dataImage = _bmImage.AccessData(true, false))
            using (Azimecha.Drawing.IBitmapDataAccessor dataCopy = _bmCopy.AccessData(false, true))
                RtlMoveMemory(dataCopy.Pointer, dataImage.Pointer, (IntPtr)dataCopy.Size);
            Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs e) { }

        private byte[] GetImageData(System.Drawing.Bitmap sbm) {
            System.Drawing.Imaging.BitmapData data = sbm.LockBits(
                new System.Drawing.Rectangle() {
                    X = 0,
                    Y = 0,
                    Width = sbm.Width,
                    Height = sbm.Height
                }, System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            try {
                byte[] arrData = new byte[data.Stride * data.Height];
                Marshal.Copy(data.Scan0, arrData, 0, arrData.Length);
                return arrData;
            } finally {
                sbm.UnlockBits(data);
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    struct BitmapInfoHeader {
        public uint biSize;
        public int biWidth;
        public int biHeight;
        public ushort biPlanes;
        public ushort biBitCount;
        public uint biCompression;
        public uint biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public uint biClrUsed;
        public uint biClrImportant;
    }
}
