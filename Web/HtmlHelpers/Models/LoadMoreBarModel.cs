using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecordLabel.Web
{
    /// <summary>
    /// Model used as a parameter for LoadMoreBar Html helper
    /// </summary>
    public class LoadMoreBarModel
    {
        public string Action { get; set; }
        public string Controller { get; set; } 
        public int ItemCount { get; set; }
        public int? FilterSourceId { get; set; }

        public LoadMoreBarModel(string action, string controller, int itemCount, int? filterSourceId)
        {
            Action = action;
            Controller = controller;
            ItemCount = itemCount;
            FilterSourceId = filterSourceId;
        }
    }
}