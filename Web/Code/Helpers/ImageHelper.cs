using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using RecordLabel.Catalogue;

namespace RecordLabel.Web
{
    public static class ImageHelper
    {
        /// <summary>
        /// Deletes image from the database
        /// </summary>
        public static void DeleteImage(ReleaseContext dbContext, Image image)
        {
            image.Delete(dbContext);
        }

        /// <summary>
        /// Deletes image from the file system
        /// </summary>
        public static void DeleteImagePhysically(Image image)
        {
            File.Delete(Path.Combine(GetAbsoluteImageDirectory(image.Type), image.FileName));
            File.Delete(Path.Combine(GetAbsoluteImageDirectory(image.Type), GetThumbnailFileName(image.FileName)));
        }

        /// <summary>
        /// Deletes image from the file system
        /// </summary>
        public static void DeleteImagePhysically(string fileName, ImageType imgType)
        {
            File.Delete(Path.Combine(GetAbsoluteImageDirectory(imgType), fileName));
            File.Delete(Path.Combine(GetAbsoluteImageDirectory(imgType), GetThumbnailFileName(fileName)));
        }

        public static string GetThumbnailFileName(string imageFileName)
        {
            string fileName = $"{Path.GetFileNameWithoutExtension(imageFileName)}_thumb";
            return Path.ChangeExtension(fileName, ImageProcessing.ImageProcessor.EncodedFileExtension);
        }

        public static string GetAbsoluteImageDirectory(ImageType imgType)
        {
            return Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, Settings.ContentImageDirectory, imgType.ToString());
        }

        /// <summary>
        /// Returns relative path to a thumbnail
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static string ThumbnailFileName(this Image image)
        {
            return $"{Settings.ContentImageDirectory}/{image.Type.ToString()}/{GetThumbnailFileName(image.FileName)}";
        }

        /// <summary>
        /// Returns relative path to this image
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static string FullFileName(this Image image)
        {
            return $"{Settings.ContentImageDirectory}/{image.Type.ToString()}/{image.FileName}";
        }
    }
}