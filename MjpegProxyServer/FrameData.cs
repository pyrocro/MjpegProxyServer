using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MjpegProxyServer
{
    public class FrameData
    {

        string imageBase64String;
        public string ImageBase64String { get { return imageBase64String; } }

        string sha1hash;
        public string SHA1Hash {get{return sha1hash;} }

        Bitmap image;
        public Bitmap Image
        {
            get { return image; }
            set
            {
                image = value;
                imageBase64String = ImageToBase64(image, System.Drawing.Imaging.ImageFormat.Jpeg);
                sha1hash = Hash(this.imageBase64String);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bmp">Bitmap image for this frame</param>
        public FrameData(Bitmap bmp)
        {
            Image = bmp;
        }

        /// <summary>
        /// Images to base64.
        /// </summary>
        /// <returns>The to base64.</returns>
        /// <param name="image">Image.</param>
        /// <param name="format">Format.</param>
        private string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ImageCodecInfo jpgEncoder = GetEncoder(format);

                // Create an Encoder object based on the GUID
                // for the Quality parameter category.
                System.Drawing.Imaging.Encoder myEncoder =
                    System.Drawing.Imaging.Encoder.Quality;

                // Create an EncoderParameters object.
                // An EncoderParameters object has an array of EncoderParameter
                // objects. In this case, there is only one
                // EncoderParameter object in the array.
                EncoderParameters myEncoderParameters = new EncoderParameters(1);

                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 75L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                /******************************************************************************************************/

                // Convert Image to byte[]
                image.Save(ms, jpgEncoder, myEncoderParameters);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        /// <summary>
        /// Gets the encoder.
        /// </summary>
        /// <returns>The encoder.</returns>
        /// <param name="format">Format.</param>
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        private string Hash(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }

    }
}
