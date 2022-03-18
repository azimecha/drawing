using System;
using System.Collections;
using Azimecha.Drawing;

namespace TestCoreApp {
    internal class Program {
        static void Main(string[] args) {
            IDrawingAPI api = DrawingFactory.GetDrawingAPI();

            IBitmap bmMain = api.LoadBitmap(typeof(Program).Assembly.GetManifestResourceStream("TestCoreApp.cringe.jpg"));
            IBitmap bmStretch = api.LoadBitmap(GetResource("tface.png"));
            IBitmap bmTile = api.LoadBitmap(GetResource("nft32.png"));

            IFont fntFixed = api.CreateClassicBitmapFont('\0', 8, 14, GetBufferBits(GetResource("seabios8x14.bin")), bReverseBitOrder: true);
            IFontSet arrTTF = api.LoadTrueTypeFonts(GetResource("trim.ttf"));
            IFont fntVector = arrTTF[0].CreateFont(20);

            IBrush brSolid1 = api.CreateSolidBrush(new Color(255, 64, 64, 192));
            IBrush brSolid2 = api.CreateSolidBrush(new Color(255, 255, 255, 192));
            IBrush brStretch = api.CreateBitmapBrush(bmStretch, ScaleMode.Stretch);
            IBrush brTile = api.CreateBitmapBrush(bmTile, ScaleMode.Tile);

            IPen penSolid1 = brSolid1.CreatePen(4.0f);
            IPen penSolid2 = brSolid2.CreatePen(4.0f);
            IPen penSolid3 = api.CreateSolidBrush(new Color(0, 192, 0, 255)).CreatePen(4.0f);
            IPen penSolid4 = api.CreateSolidBrush(new Color(0, 0, 255, 255)).CreatePen(4.0f);

            float fSliceSize = (float)Math.PI / 3;

            using (IDrawingContext ctx = bmMain.CreateContext()) {
                ctx.BlitImage(bmStretch, 298, 160, 183, 164);
                ctx.FillRectangle(brTile, 44, 372, 366, 105);
                ctx.FillText(brSolid1, fntFixed, "ALERT:\nBIG CHUNGUS SPOTTED", 334, 16, 143, 75, Alignment.Center, Alignment.Center);
                ctx.FillText(brSolid2, fntVector, "brought to you by C#", 0, 0, bmMain.Width, bmMain.Height, Alignment.Near,
                    Alignment.Far, TextWrapping.None);

                ctx.DrawArc(penSolid1, 26, 124, 136, 97, 0, fSliceSize);
                ctx.DrawArc(penSolid2, 26, 124, 136, 97, fSliceSize, fSliceSize);
                ctx.DrawArc(penSolid3, 26, 124, 136, 97, fSliceSize * 2, fSliceSize);
                ctx.DrawArc(penSolid4, 26, 124, 136, 97, fSliceSize * 3, fSliceSize);

                ctx.FillPieSlice(brSolid1, 26, 124, 136, 97, 0, fSliceSize);
                ctx.FillPieSlice(brSolid2, 26, 124, 136, 97, fSliceSize, fSliceSize);
                ctx.FillPieSlice(brStretch, 26, 124, 136, 97, fSliceSize * 2, fSliceSize);
                ctx.FillPieSlice(brTile, 26, 124, 136, 97, fSliceSize * 3, fSliceSize);
            }

            unsafe {
                using (IBitmapDataAccessor data = bmMain.AccessData(true, false)) {
                    Span<byte> spanData = new Span<byte>((void*)data.Pointer, (int)data.Size);
                    using (System.IO.Stream stmFile = System.IO.File.OpenWrite("output.raw"))
                        stmFile.Write(spanData);
                }
            }
        }

        private static IDataBuffer GetResource(string strName)
            => DrawingFactory.CreateBufferFrom(typeof(Program).Assembly.GetManifestResourceStream(typeof(Program).Namespace + "." + strName));

        private static BitArray GetBufferBits(IDataBuffer buf) {
            byte[] arrData = new byte[buf.DataSize];
            System.Runtime.InteropServices.Marshal.Copy(buf.DataPointer, arrData, 0, (int)buf.DataSize);
            return new BitArray(arrData);
        }
    }
}
