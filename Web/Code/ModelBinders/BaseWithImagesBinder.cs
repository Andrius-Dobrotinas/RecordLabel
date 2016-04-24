using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecordLabel;
using RecordLabel.Content;
using System.IO;

namespace RecordLabel.Web.ModelBinding
{
    /// <summary>
    /// Saves posted images to the Contents location and generates corresponding thumbnails
    /// </summary>
    public class BaseWithImagesBinder : AttributeModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            BaseWithImages model = (BaseWithImages)base.BindModel(controllerContext, bindingContext);

            List<Image> images = GetAndSavePostedImages(controllerContext.HttpContext.Request, model.DetermineImageType());
            if (images.Count != controllerContext.HttpContext.Request.Files.Count)
            {
                bindingContext.ModelState.AddModelError("Images", "Failed to save some of the files");
            }
            if (images.Count > 0)
            {
                model.Images = new ImageSet(images);
            }
            return model;
        }

        /// <summary>
        /// Gets posted images from the Request and saves thumbnails them along with generated to the Content location defined in settings
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="imgType"></param>
        /// <returns></returns>
        private static List<Image> GetAndSavePostedImages(HttpRequestBase Request, ImageType imgType)
        {
            List<Image> images = new List<Image>(Request.Files.Count);

            if (Request.Files.Count > 0)
            {
                int[] imageOrders = Request.Form.GetValues("imageOrder").Select(item => int.Parse(item)).ToArray();

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase postedFile = Request.Files[i];
                    if (postedFile.InputStream.Length > 0)
                    {
                        Image image = SaveFile(postedFile, imageOrders[i], imgType);
                        if (image != null)
                        {
                            images.Add(image);
                        }
                    }
                }
            }

            return images;
        }

        /// <summary>
        /// Generates image thumbnail and saves both to the file system
        /// </summary>
        /// <param name="postedFile"></param>
        /// <param name="imageOrder"></param>
        /// <param name="imgType"></param>
        /// <returns></returns>
        private static Image SaveFile(HttpPostedFileBase postedFile, int imageOrder, ImageType imgType)
        {
            Image image = new Image()
            {
                Type = imgType,
                Name = Path.GetFileNameWithoutExtension(postedFile.FileName),
                FileName = Path.ChangeExtension(Guid.NewGuid().ToString(), Path.GetExtension(postedFile.FileName)),
                Order = imageOrder
            };
            //TODO: might want to check whether there is a file with this Guid already
            string absoluteFilePath = Path.Combine(ImageHelper.GetAbsoluteImageDirectory(imgType), image.FileName);
            string absoluteThumbnFilePath = Path.Combine(ImageHelper.GetAbsoluteImageDirectory(imgType), ImageHelper.GetThumbnailFileName(image.FileName));

            try
            {
                postedFile.SaveAs(absoluteFilePath);
                ImageProcessing.ImageProcessor.ResizeAndWriteFile(postedFile.InputStream, absoluteThumbnFilePath, Settings.ThumbnailWidth, Settings.ThumbnailQualityLevel);
            }
            catch
            {
                if (File.Exists(absoluteFilePath))
                {
                    File.Delete(absoluteFilePath);
                }
                if (File.Exists(absoluteThumbnFilePath))
                {
                    File.Delete(absoluteThumbnFilePath);
                }
                return null;
                //TODO: log this exception
            }
            return image;
        }
    }
}