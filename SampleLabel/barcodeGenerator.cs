using System;
using System.Collections.Generic;
using System.Text;
using ZXing;
using ZXing.Common;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

namespace VTBarcode
{
    class Code128
    {
        private const BarcodeFormat DEFAULT_BARCODE_FORMAT = BarcodeFormat.CODE_128;
        private static readonly ImageFormat DEFAULT_IMAGE_FORMAT = ImageFormat.Bmp;
        private const String DEFAULT_OUTPUT_FILE = "out";
        private const int DEFAULT_WIDTH = 2715;
        private const int DEFAULT_HEIGHT = 300;
        public byte[] Generate(string toEncode)
        {
            BarcodeFormat barcodeFormat = DEFAULT_BARCODE_FORMAT;
            ImageFormat imageFormat = DEFAULT_IMAGE_FORMAT;
            String outFileString = DEFAULT_OUTPUT_FILE;
            int width = DEFAULT_WIDTH;
            int height = DEFAULT_HEIGHT;
            bool clipboard = false;

            BarcodeWriter barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = barcodeFormat;
            barcodeWriter.Options.PureBarcode = true;
            barcodeWriter.Options.Width = width;
            barcodeWriter.Options.Height = height;
            Bitmap bitmap = barcodeWriter.Write(toEncode);
            bitmap.Save("sample.bmp");
            FileStream fs = new FileStream("sample.bmp", FileMode.Open); 
            BinaryReader br = new BinaryReader(fs);
            byte[] imgbyte = new byte[fs.Length + 1];
            imgbyte = br.ReadBytes(Convert.ToInt32((fs.Length)));
            br.Close();
            fs.Close();
            File.Delete("sample.bmp");
            return imgbyte;
        }
    }
}
