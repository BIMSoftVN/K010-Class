using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace K010_RevitAddins
{
    public class mHelper
    {
        public static BitmapFrame  ConvertResourceToImgSource(string resourcePath, double Px)
        {
            BitmapFrame result = null;

            try
            {
                using (Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath))
                {
                    BitmapDecoder decoder = BitmapDecoder.Create(str, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    BitmapFrame frame = decoder.Frames[0];
                    result = BitmapFrame.Create(new TransformedBitmap(frame, new ScaleTransform(Px/ frame.Width, Px/ frame.Height)));
                }    
            }
            catch
            {

            }

            return result;
        }
    }
}
