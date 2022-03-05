using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace TestWinApp {
    public partial class TestForm : Form {
        private Azimecha.Drawing.IBitmap _bmImage, _bmCopy;

        public TestForm() {
            InitializeComponent();
        }

        private void TestForm_Load(object sender, EventArgs e) {
            byte[] arrData = GetImageData(Properties.Resources.cringe);
            _bmImage = Azimecha.Drawing.AGG.Bitmap.CreateOnArray(Properties.Resources.cringe.Width, Properties.Resources.cringe.Height, arrData);
            _bmCopy = _bmImage.Duplicate();
            _bmImage.Dispose();
            _bmCopy.Dispose();
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
}
