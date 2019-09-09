using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSteganographyLibrary
{
    public class ColorDataExtractor
    {
        public static string ExtractDataFromImage(FileInfo carrierImage)
        {
            Bitmap image = new Bitmap(carrierImage.FullName);
            //If the data was not hidden with this program, then metadata cannot be found
            if (!CheckForWatermark(image)) return "Image Not Compatible";
            //ASCII value of the padding is returned (e.g. 2 -> 50) so need to change it back
            int padding = int.Parse(ExtractFromPixel(image.GetPixel(image.Width - 1, 0)).ToString());
            int dataLength = GetLength(image, padding);

            string SteganoText = ExtractText(image, dataLength);
 
            return SteganoText;
        }

        private static string ExtractText(Bitmap image, int dataLength)
        {
            StringBuilder text = new StringBuilder();

            for (int y = 1; y < dataLength - 1; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    if (x == dataLength - 1)
                    {
                        y = dataLength;
                        break;
                    }

                    text.Append(ExtractFromPixel(image.GetPixel(x, y)));
                }
            }

            return text.ToString();
        }

        private static int GetLength(Bitmap image, int padding)
        {
            StringBuilder length = new StringBuilder();

            for(int i = 0; i < padding; i++)
            {
                int digit = int.Parse(ExtractFromPixel(image.GetPixel(i, 0)).ToString());
                length.Append(digit.ToString());
            }

            return int.Parse(length.ToString());
        }

        private static bool CheckForWatermark(Bitmap image)
        {
            string watermark = "IConceal";
            StringBuilder watermarkExtract = new StringBuilder();

            for(int i = 0; i < watermark.Length; i++)
            {
                watermarkExtract.Append(ExtractFromPixel(image.GetPixel(i, image.Height - 1)));
            }

            return string.Equals(watermarkExtract.ToString(), watermark);
        }

        private static char ExtractFromPixel(Color pixel)
        {
            string r = ToNineBinaryDigits(int.Parse(pixel.R.ToString()));
            string g = ToNineBinaryDigits(int.Parse(pixel.G.ToString()));
            string b = ToNineBinaryDigits(int.Parse(pixel.B.ToString()));

            string extractedLSB = ExtractLSBs(r, g, b);

            return ConvertBinaryToChar(extractedLSB);
        }

        private static string ExtractLSBs(string r, string g, string b)
        {
            string binaryValue = string.Empty;

            binaryValue += r.Substring(6, 3);
            binaryValue += g.Substring(6, 3);
            binaryValue += b.Substring(6, 3);

            return binaryValue;
        }

        private static string ToNineBinaryDigits(int value)
        {
            return Convert.ToString(value, 2).PadLeft(9, '0');
        }

        private static char ConvertBinaryToChar(string characterInBinary)
        {
            List<byte> list = new List<byte>();
            list.Add(Convert.ToByte(characterInBinary.Substring(0, 9), 2));
            return Encoding.ASCII.GetString(list.ToArray())[0];
        }
    }
}
