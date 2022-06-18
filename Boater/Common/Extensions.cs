using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Boater.Common
{
    public static class StringExtensions
    {
        public static string Obfuscate(this string str, char obfuscator = '*')
        {
            return new string(obfuscator, str.Length);
        }
    }

    public static class IEnumerableExtensions
    {
        public static string Stringify<T, TResult>(this IEnumerable<T> enumerable, Func<T, TResult> selector, string separator = ", ")
        {
            return string.Join(separator, enumerable.Select(selector));
        }
    }

    public static class WinFormExtensions
    {
        public static void SetText(this Control control, string text, int minFontSize = 12)
        {
            Graphics g = control.CreateGraphics();
            Font originalFont = control.Font;
            int width = control.Width;

            Font updatedFont = null;
            // We utilize MeasureString which we get via a control instance           
            for (float adjustedSize = originalFont.Size; adjustedSize >= minFontSize; adjustedSize--)
            {
                updatedFont = new Font(originalFont.Name, adjustedSize, originalFont.Style);

                // Test the string with the new size
                SizeF adjustedSizeNew = g.MeasureString(text, updatedFont);

                if (width > Convert.ToInt32(adjustedSizeNew.Width))
                {
                    break;
                }
            }

            SetTextPrivate(control, text, updatedFont);
        }

        delegate void SetTextCallback(Control control, string text, Font updatedFont);
        /// <summary>
        /// Set the text safely using the <see cref="Control.InvokeRequired"/> property.
        /// </summary>
        /// <param name="control">The control to update the text and font.</param>
        /// <param name="text">The updated text</param>
        /// <param name="font">A font that will fit within the control.</param>
        private static void SetTextPrivate(Control control, string text, Font font)
        {
            if (control.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetTextPrivate);
                control.Invoke(d, new object[] { control, text, font });
            }
            else
            {
                control.Text = text;
                control.Font = font;
            }
        }

        /// <summary>
        /// method to rotate an image either clockwise or counter-clockwise
        /// </summary>
        /// <param name="img">the image to be rotated</param>
        /// <param name="rotationAngle">the angle (in degrees).
        /// NOTE: 
        /// Positive values will rotate clockwise
        /// negative values will rotate counter-clockwise
        /// </param>
        /// <returns></returns>
        public static Image RotateImage(Image img, float rotationAngle)
        {
            //create an empty Bitmap image
            Bitmap bmp = new Bitmap(img.Width, img.Height);

            //turn the Bitmap into a Graphics object
            Graphics gfx = Graphics.FromImage(bmp);

            //now we set the rotation point to the center of our image
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            //now rotate the image
            gfx.RotateTransform(rotationAngle);

            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            //set the InterpolationMode to HighQualityBicubic so to ensure a high
            //quality image once it is transformed to the specified size
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //now draw our new image onto the graphics object
            gfx.DrawImage(img, new Point(0, 0));

            //dispose of our Graphics object
            gfx.Dispose();

            //return the image
            return bmp;
        }
    }
}
