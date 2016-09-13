using RecordLabel.Content;
using System;

namespace RecordLabel.Web.ViewModels
{
    public class ListItemWithImageModel
    {
        public BaseWithImages Model { get; set; }
        public bool AdminMode { get; set; }
        public string TargetControllerName { get; set; }

        /// <summary>
        /// Non-null value. Empty string overrides default class
        /// </summary>
        public string ContainerClass { get; set; }
    }
}