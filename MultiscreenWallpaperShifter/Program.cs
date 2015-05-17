using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiscreenWallpaperShifter
{
    class Program
    {
        /// <summary>
        /// Mod implementation since default operator did not handle negatives well.
        /// Followed JerKimball's implementation from http://stackoverflow.com/questions/5383050/how-can-i-calculate-divide-and-modulo-for-integers
        /// </summary>
        /// <param name="a">Number we are interested in</param>
        /// <param name="n">Modulus</param>
        /// <returns></returns>
        private static int Mod(int a, int n)
        {
            return a - (int)Math.Floor((double)a / n) * n;
        }

        static void Main(string[] args)
        {
            // Check our arguments
            if (args.Length == 0)
            {
                Console.WriteLine("No image specified! Exiting.");
                return;
            }

            string outFilename = "output.png";
            ImageFormat outFormat = ImageFormat.Png;
            string imageFormatString = "";
            // Get options
            if (args.Length > 1)
            {
                for (int i = 1; i < args.Length; i++)
                {
                    if (args[i] == "/o")
                    {
                        outFilename = args[i + 1];
                        i++;
                    }
                    else if (args[i] == "/f")
                    {
                        imageFormatString = args[i + 1].ToLowerInvariant();
                        switch (imageFormatString)
                        {
                            case "jpeg":
                                outFormat = ImageFormat.Jpeg;
                                break;
                            case "png":
                                outFormat = ImageFormat.Png;
                                break;
                            default:
                                break;
                        }
                        i++;
                    }
                }
            }

            // Load our image
            var sourceImage = new Bitmap(args[0]);

            // Get the screens, in the order from left to right
            var screens = Screen.AllScreens.OrderBy(screen => screen.Bounds.X);

            // Do a quick check with our dimensions
            var horizontalDesktopRes = screens.Select(screen => screen.Bounds.Width).Sum();
            var verticalDesktopRes = screens.Select(screen => screen.Bounds.Height).Max();
            if (horizontalDesktopRes != sourceImage.Width)
            {
                Console.WriteLine("Warning: Horizontal image resolution is not equivalent to horizontal desktop resolution. Behaviour is unspecified.");
            }

            var outImage = new Bitmap(sourceImage.Width, verticalDesktopRes);

            using (Graphics graphics = Graphics.FromImage(outImage))
            {
                int posX, posY;
                posX = posY = 0;
                foreach (var screen in screens)
                {
                    graphics.DrawImage(sourceImage, new Rectangle
                    {
                        X = Mod(screen.Bounds.X, sourceImage.Width),
                        Y = Mod(screen.Bounds.Y, sourceImage.Height),
                        Width = screen.Bounds.Width,
                        Height = screen.Bounds.Height
                    }, new Rectangle
                    {
                        X = posX,
                        Y = posY,
                        Width = screen.Bounds.Width,
                        Height = screen.Bounds.Height
                    }, GraphicsUnit.Pixel);

                    posX += screen.Bounds.Width;
                }
            }

            outImage.Save(outFilename, outFormat);

            return;
        }
    }
}
