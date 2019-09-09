using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ImageSteganographyLibrary
{
    public class ColorEmbed
    {
        //TO DO : Check if edit original image or copy new one 
        public static string Embed(FileInfo image, string text)
        {
            Bitmap carrierImage = new Bitmap(image.FullName);
            string outputPath = $@"{image.DirectoryName}\IConceal - {image.Name}";

            InsertMetaData(carrierImage, text.Length - 1, text.Length.ToString().Length);
            InsertWatermark(carrierImage);

            for(int y = 1; y < text.Length - 1; y++)
            {
                for(int x = 0; x < carrierImage.Width; x++)
                {
                    if (x == text.Length - 1)
                    {
                        y = text.Length;
                        break;
                    }

                    carrierImage.SetPixel(y, x,
                        EmbedDataInColor(
                            carrierImage.GetPixel(x, y), text[x]));
                }
            }

            Console.WriteLine(carrierImage.GetPixel(1, 0));
            carrierImage.Save(outputPath);
            return outputPath;
        }

        private static bool InsertWatermark(Bitmap carrierImage)
        {
            string watermark = "IConceal";

            //start at bottom-left corner
            for(int i = 0; i < watermark.Length; i++)
            {
                carrierImage.SetPixel(i, carrierImage.Height - 1, 
                    EmbedDataInColor(
                        carrierImage.GetPixel(i, carrierImage.Height - 1), watermark[i]));
            }

            return true;
        }

        private static bool InsertMetaData(Bitmap image, int dataLength, int padding)
        {
            //Embed the padding value - Top-left corner
            image.SetPixel(image.Width - 1, 0, 
                EmbedDataInColor(image.GetPixel(image.Width - 1, 0), padding.ToString()[0]));

            //Embed the Data length
            for(int i = 0; i < padding-1; i++)
            {
                image.SetPixel(i, 0, 
                    EmbedDataInColor(image.GetPixel(i, 0), dataLength.ToString()[i]));
                Console.WriteLine("loop : "  + image.GetPixel(i, 0));
            }

            return true;
        }

        private static Color EmbedDataInColor(Color pixel, char value)
        {
            //H to nine binary digits -> 001001000
            string valueInBinary = ToNineBinaryDigits(value);
            //split H in binary into separate groups 001001000 -> 001 001 000
            Tuple<string, string, string> splitValueRGB = SplitValueToRGB(valueInBinary);
            return ConfigureLSB(pixel, splitValueRGB);
        }

        private static Color ConfigureLSB(Color pixel, Tuple<string, string, string> splitValueRGB)
        {
            //Pixel in binary
            string r = ToNineBinaryDigits(pixel.R), g = ToNineBinaryDigits(pixel.G), b = ToNineBinaryDigits(pixel.B);

            //Edit the LSBs of RGB values of the pixel to include the character
            r = r.Remove(r.Length - 3) + splitValueRGB.Item1;
            g = g.Remove(g.Length - 3) + splitValueRGB.Item2;
            b = b.Remove(b.Length - 3) + splitValueRGB.Item3;

            return Color.FromArgb(255,
                Convert.ToByte(Convert.ToInt32(r, 2)),
                Convert.ToByte(Convert.ToInt32(g, 2)),
                Convert.ToByte(Convert.ToInt32(b, 2))
                );
        }

        private static Tuple<string, string, string> SplitValueToRGB(string valueInBinary)
        {
            return new Tuple<string, string, string>
                (valueInBinary.Substring(0, 3), valueInBinary.Substring(3, 3), valueInBinary.Substring(6, 3));
        }

        private static string ToNineBinaryDigits(int value)
        {
            return Convert.ToString(value, 2).PadLeft(9, '0');
        }
    }
}
