using System;
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
        private Azimecha.Drawing.IBrush _brCur, _brRed, _brPattern, _brStretch;
        private Azimecha.Drawing.IDrawingContext _ctx;
        private Azimecha.Drawing.IPen _penCur;
        private int _nMouseX1, _nMouseX2, _nMouseX3;
        private int _nMouseY1, _nMouseY2, _nMouseY3;
        private int _nMouseValid;

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

            using (Azimecha.Drawing.AGG.Bitmap bmPattern = Azimecha.Drawing.AGG.Bitmap.CreateOnArray(Properties.Resources.nft32.Width,
                Properties.Resources.nft32.Height, GetImageData(Properties.Resources.nft32)))
                _brPattern = new Azimecha.Drawing.AGG.PatternBrush(bmPattern);

            using (Azimecha.Drawing.AGG.Bitmap bmStretch = Azimecha.Drawing.AGG.Bitmap.CreateOnArray(Properties.Resources.smug_trollface_cutout.Width,
                Properties.Resources.smug_trollface_cutout.Height, GetImageData(Properties.Resources.smug_trollface_cutout)))
                _brStretch = new Azimecha.Drawing.AGG.ScaledBrush(bmStretch, Azimecha.Drawing.ScaleMode.Stretch);

            _ctx = _bmCopy.CreateContext();
        }

        [DllImport("gdi32")]
        private static extern int SetDIBitsToDevice(IntPtr hDC, int nDestX, int nDestY, uint w, uint h, uint nSrcX, uint nSrcY, uint nStartLine,
            uint nLines, IntPtr pBits, [In] ref BitmapInfoHeader bmih, uint nColorUse);

        private void TestForm_Paint(object sender, PaintEventArgs e) {
            System.Drawing.Rectangle rectImage = new System.Drawing.Rectangle(0, 0, _bmCopy.Width, _bmCopy.Height);

            System.Drawing.Graphics gfx = e.Graphics;
            gfx.SetClip(e.ClipRectangle);

            if (gfx.Clip.IsVisible(rectImage)) {
                IntPtr hDC = gfx.GetHdc();
                try {
                    using (Azimecha.Drawing.IBitmapDataAccessor data = _bmCopy.AccessData(true, false))
                        SetDIBitsToDevice(hDC, 0, 0, (uint)_bmCopy.Width, (uint)_bmCopy.Height, 0, 0, 0, (uint)_bmCopy.Height,
                            data.Pointer, ref _bmih, 0);
                } finally {
                    gfx.ReleaseHdc(hDC);
                }
            }

            gfx.ExcludeClip(rectImage);
            base.OnPaintBackground(new PaintEventArgs(gfx, e.ClipRectangle));
        }

        private void TestForm_MouseMove(object sender, MouseEventArgs e) {
            if (((e.Button & MouseButtons.Left) != 0) && Capture && !(_penCur is null) && (_nMouseValid >= 3)) {
                _ctx.DrawPolyline(_penCur, new Azimecha.Drawing.PointF[] {
                    new Azimecha.Drawing.PointF(_nMouseX1, _nMouseY1),
                    new Azimecha.Drawing.PointF(_nMouseX2, _nMouseY2),
                    new Azimecha.Drawing.PointF(_nMouseX3, _nMouseY3)
                });

                _nMouseValid = 1;

                Invalidate();
            }

            _nMouseX3 = _nMouseX2;
            _nMouseX2 = _nMouseX1;
            _nMouseX1 = e.X;

            _nMouseY3 = _nMouseY2;
            _nMouseY2 = _nMouseY1;
            _nMouseY1 = e.Y;

            _nMouseValid++;
        }

        private void StretchBrushBtn_Click(object sender, EventArgs e) {
            _brCur = _brStretch;
            _penCur = new Azimecha.Drawing.AGG.Pen((Azimecha.Drawing.AGG.Brush)_brCur, 32.0f);
        }

        private void PatBrushBtn_Click(object sender, EventArgs e) {
            _brCur = _brPattern;
            _penCur = new Azimecha.Drawing.AGG.Pen((Azimecha.Drawing.AGG.Brush)_brCur, 20.0f);
        }

        private void RedBrushBtn_Click(object sender, EventArgs e) {
            _brCur = _brRed;
            _penCur = new Azimecha.Drawing.AGG.Pen((Azimecha.Drawing.AGG.Brush)_brCur, 2.5f);
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

        private void TestForm_MouseClick(object sender, MouseEventArgs e) {
            if (((e.Button & MouseButtons.Left) != 0) && !(_brCur is null)) {
                _ctx.FillCircle(_brCur, e.X, e.Y, 50.0f);
                Invalidate();
            }
        }

        private void TestForm_MouseDown(object sender, MouseEventArgs e) {
            Capture = true;
            _nMouseValid = 0;
        }

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
