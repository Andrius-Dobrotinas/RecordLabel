using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLabel.Content
{
    public static class ModelExtensions
    {
        public static ImageType DetermineImageType(this BaseWithImages model)
        {
            string typeName = model.GetType().Name;
            return typeof(ImageType).GetEnumValues().Cast<ImageType>().FirstOrDefault(item => item.ToString() == typeName);
        }
    }
}
