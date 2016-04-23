using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RecordLabel.ImageProcessing
{
    public class ImageProcessor
    {
        public const string EncodedFileExtension = "jpg";

        //TODO: let ImageProcessor determine whether it should use this number for width or height by looking at the image proportions (which dimension is smaller)

        /// <summary>
        /// Resizes and saves the image to a specified location in the file system
        /// </summary>
        /// <param name="imageStream">Source image stream</param>
        /// <param name="outputFilePath">Full destination output file path (including name and extension)</param>
        /// <param name="width"></param>
        /// <param name="imageQualityLevel"></param>
        public static void ResizeAndWriteFile(Stream imageStream, string outputFilePath, double width, int imageQualityLevel)
        {
            BitmapFrame imageSource = BitmapFrame.Create(imageStream);
            double height = DetermineProportionateHeight(imageSource, width);
            BitmapSource resizedImage = Resize(imageSource, width, height);
            BitmapEncoder result = Encode(resizedImage, imageQualityLevel);
            WriteImageToFile(result, outputFilePath);
        }

        public static BitmapSource Resize(BitmapSource imageSource, double width, double height)
        {
            ScaleTransform transform = new ScaleTransform();
            transform.ScaleX = width / imageSource.PixelWidth;
            transform.ScaleY = height / imageSource.PixelHeight;
            return new TransformedBitmap(imageSource, transform);
        }

        public static BitmapEncoder Encode(BitmapSource imageSource, int qualityLevel)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.QualityLevel = 90;
            encoder.Frames.Add(BitmapFrame.Create(imageSource));
            return encoder;
        }

        public static double DetermineProportionateHeight(BitmapSource imageSource, double width)
        {
            double percent = width / imageSource.Width;
            return imageSource.Height * percent;
        }

        /// <summary>
        /// Writes encoded image stream to a file
        /// </summary>
        /// <param name="encoder"></param>
        /// <param name="filePath"></param>
        public static void WriteImageToFile(BitmapEncoder encoder, string filePath)
        {
            using (Stream stream = new FileStream(filePath, FileMode.CreateNew))
            {
                encoder.Save(stream);
                stream.Close();
            }
        }

        /// <summary>
        /// Loads image from file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static BitmapSource LoadImage(string filePath)
        {
            using (Stream stream = new FileStream(filePath, FileMode.Open))
            {
                return BitmapFrame.Create(stream);
            }
        }
    }
}
