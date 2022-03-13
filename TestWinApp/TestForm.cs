using System;
using System.Collections;
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
        private Azimecha.Drawing.IBrush _brCur, _brRed, _brPattern, _brStretch, _brFillH, _brFitH, _brFillV, _brFitV;
        private Azimecha.Drawing.IDrawingContext _ctx;
        private Azimecha.Drawing.IPen _penCur;
        private Azimecha.Drawing.IFont _fontCur, _fontBitmap, _fontTTF;
        private int _nMouseX1, _nMouseX2, _nMouseX3;
        private int _nMouseY1, _nMouseY2, _nMouseY3;
        private int _nMouseValid;
        private bool _bPainting, _bBlitting;
        private int _nBlitX, _nBlitY;

        public TestForm() {
            InitializeComponent();
        }

        private void TestForm_Load(object sender, EventArgs e) {
            Azimecha.Drawing.IDrawingAPI api = Azimecha.Drawing.DrawingFactory.GetDrawingAPI();

            byte[] arrData = GetImageData(Properties.Resources.cringe);
            _bmImage = api.CreateBitmapOnData(Properties.Resources.cringe.Width, Properties.Resources.cringe.Height,
                Azimecha.Drawing.DrawingFactory.CreateBufferOn(arrData));
            _bmCopy = _bmImage.Duplicate();

            _bmih.biSize = (uint)Marshal.SizeOf(_bmih);
            _bmih.biPlanes = 1;
            _bmih.biBitCount = 32;

            _brRed = api.CreateSolidBrush(new Azimecha.Drawing.Color(255, 0, 0, 128));

            using (Azimecha.Drawing.IBitmap bmPattern = api.CreateBitmapOnData(Properties.Resources.nft32.Width,
                Properties.Resources.nft32.Height, Azimecha.Drawing.DrawingFactory.CreateBufferOn(GetImageData(Properties.Resources.nft32))))
                _brPattern = api.CreateBitmapBrush(bmPattern, Azimecha.Drawing.ScaleMode.Tile);

            using (Azimecha.Drawing.IBitmap bmStretch = api.CreateBitmapOnData(Properties.Resources.smug_trollface_cutout.Width,
                Properties.Resources.smug_trollface_cutout.Height, Azimecha.Drawing.DrawingFactory.CreateBufferOn(GetImageData(Properties.Resources.smug_trollface_cutout))))
                _brStretch = api.CreateBitmapBrush(bmStretch, Azimecha.Drawing.ScaleMode.Stretch);

            using (Azimecha.Drawing.IBitmap bmHoriz1 = api.CreateBitmapOnData(Properties.Resources.cursed_wikihow.Width,
                Properties.Resources.cursed_wikihow.Height, Azimecha.Drawing.DrawingFactory.CreateBufferOn(GetImageData(Properties.Resources.cursed_wikihow))))
                _brFillH = api.CreateBitmapBrush(bmHoriz1, Azimecha.Drawing.ScaleMode.Fill);

            using (Azimecha.Drawing.IBitmap bmHoriz2 = api.CreateBitmapOnData(Properties.Resources.cpp_vs_c.Width,
                Properties.Resources.cpp_vs_c.Height, Azimecha.Drawing.DrawingFactory.CreateBufferOn(GetImageData(Properties.Resources.cpp_vs_c))))
                _brFitH = api.CreateBitmapBrush(bmHoriz2, Azimecha.Drawing.ScaleMode.Fit);

            using (Azimecha.Drawing.IBitmap bmVert1 = api.CreateBitmapOnData(Properties.Resources.we_are_not_the_same.Width,
                Properties.Resources.we_are_not_the_same.Height, Azimecha.Drawing.DrawingFactory.CreateBufferOn(GetImageData(Properties.Resources.we_are_not_the_same))))
                _brFillV = api.CreateBitmapBrush(bmVert1, Azimecha.Drawing.ScaleMode.Fill);

            using (Azimecha.Drawing.IBitmap bmVert2 = api.CreateBitmapOnData(Properties.Resources.vim_can_cutout.Width,
                Properties.Resources.vim_can_cutout.Height, Azimecha.Drawing.DrawingFactory.CreateBufferOn(GetImageData(Properties.Resources.vim_can_cutout))))
                _brFitV = api.CreateBitmapBrush(bmVert2, Azimecha.Drawing.ScaleMode.Fit);

            _ctx = _bmCopy.CreateContext();

            BitArray arrFontBits = new BitArray(Properties.Resources.seabios8x14);
            _fontBitmap = api.CreateClassicBitmapFont('\0', 8, 14, arrFontBits, bReverseBitOrder: true);

            string strTempTTF = System.IO.Path.GetTempFileName();
            System.IO.File.WriteAllBytes(strTempTTF, Properties.Resources.trim);

            Azimecha.Drawing.IFontSet fontset = api.LoadTrueTypeFonts(strTempTTF);
            _fontTTF = fontset[0].CreateFont(24);
        }

        [DllImport("gdi32")]
        private static extern int SetDIBitsToDevice(IntPtr hDC, int nDestX, int nDestY, uint w, uint h, uint nSrcX, uint nSrcY, uint nStartLine,
            uint nLines, IntPtr pBits, [In] ref BitmapInfoHeader bmih, uint nColorUse);

        private void TestForm_Paint(object sender, PaintEventArgs e) {
            System.Drawing.Rectangle rectImage = new System.Drawing.Rectangle(0, 0, _bmCopy.Width, _bmCopy.Height);

            System.Drawing.Graphics gfx = e.Graphics;
            gfx.SetClip(e.ClipRectangle);

            _bmih.biWidth = _bmCopy.Width;
            _bmih.biHeight = -_bmCopy.Height;
            _bmih.biSizeImage = (uint)_bmCopy.DataSize;

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

        private void BitmapFontBtn_Click(object sender, EventArgs e) {
            _fontCur = _fontBitmap;
        }

        private void TruetypeFontBtn_Click(object sender, EventArgs e) {
            _fontCur = _fontTTF;
        }

        private void LoadBtn_Click(object sender, EventArgs e) {
            if (OpenImageDlg.ShowDialog() == DialogResult.OK) {
                _bmCopy = Azimecha.Drawing.DrawingFactory.GetDrawingAPI().LoadBitmap(OpenImageDlg.FileName);
                _ctx = _bmCopy.CreateContext();
                Invalidate();
            }
        }

        private void TestForm_MouseMove(object sender, MouseEventArgs e) {
            if (((e.Button & MouseButtons.Left) != 0) && _bPainting && !(_penCur is null) && (_nMouseValid >= 3)) {
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

        private void SaveBtn_Click(object sender, EventArgs e) {
            if (SaveImageDlg.ShowDialog() == DialogResult.OK)
                System.IO.File.WriteAllBytes(SaveImageDlg.FileName, _bmCopy.ReadData());
        }

        private void TestForm_MouseUp(object sender, MouseEventArgs e) {
            if ((e.Button & MouseButtons.Left) != 0)
                _bPainting = false;

            if (((e.Button & MouseButtons.Right) != 0) && _bBlitting) {
                int nBlitLeft, nBlitTop, nBlitRight, nBlitBottom;

                nBlitLeft = _nBlitX < e.X ? _nBlitX : e.X;
                nBlitTop = _nBlitY < e.Y ? _nBlitY : e.Y;
                nBlitRight = _nBlitX > e.X ? _nBlitX : e.X;
                nBlitBottom = _nBlitY > e.Y ? _nBlitY : e.Y;

                _ctx.BlitImage(_bmCopy, nBlitLeft, nBlitTop, nBlitRight - nBlitLeft, nBlitBottom - nBlitTop);
                Invalidate(new System.Drawing.Rectangle(nBlitLeft, nBlitTop, nBlitRight - nBlitLeft, nBlitBottom - nBlitTop));

                _bBlitting = false;
            }
        }

        private void FillHorizBrushBtn_Click(object sender, EventArgs e) {
            _brCur = _brFillH;
            _penCur = new Azimecha.Drawing.AGG.Pen((Azimecha.Drawing.AGG.Brush)_brCur, 32.0f);
        }

        private void FitHorizBrushBtn_Click(object sender, EventArgs e) {
            _brCur = _brFitH;
            _penCur = new Azimecha.Drawing.AGG.Pen((Azimecha.Drawing.AGG.Brush)_brCur, 32.0f);
        }

        private void FillVertBrushBtn_Click(object sender, EventArgs e) {
            _brCur = _brFillV;
            _penCur = new Azimecha.Drawing.AGG.Pen((Azimecha.Drawing.AGG.Brush)_brCur, 32.0f);

        }

        private void FitVertBrushBtn_Click(object sender, EventArgs e) {
            _brCur = _brFitV;
            _penCur = new Azimecha.Drawing.AGG.Pen((Azimecha.Drawing.AGG.Brush)_brCur, 32.0f);
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
            _bmCopy = _bmImage.Duplicate();
            _ctx = _bmCopy.CreateContext();
            Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs e) { }

        private void TestForm_MouseClick(object sender, MouseEventArgs e) {
            if (((e.Button & MouseButtons.Left) != 0) && !(_brCur is null)) {
                _ctx.FillCircle(_brCur, e.X, e.Y, 50.0f);
                Invalidate();
            }

            if (((e.Button & MouseButtons.Middle) != 0) && !(_brCur is null) && !(_fontCur is null)) {
                string str = Microsoft.VisualBasic.Interaction.InputBox("Enter text", "Font rendering", "Sample Text");
                if (!(str is null)) {
                    _ctx.FillText(_brCur, _fontCur, str, e.X - 50, e.Y - 50, 100, 100, Azimecha.Drawing.Alignment.Center, 
                        Azimecha.Drawing.Alignment.Center, Azimecha.Drawing.TextWrapping.WordWrap);
                    Invalidate();
                }
            }
        }

        private void TestForm_MouseDown(object sender, MouseEventArgs e) {
            Capture = true;

            if ((e.Button & MouseButtons.Left) != 0) {
                _nMouseValid = 0;
                _bPainting = true;
            }

            if ((e.Button & MouseButtons.Right) != 0) {
                _nBlitX = e.X;
                _nBlitY = e.Y;
                _bBlitting = true;
            }
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
