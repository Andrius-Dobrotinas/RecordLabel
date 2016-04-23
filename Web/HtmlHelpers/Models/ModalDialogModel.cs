using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecordLabel.Web
{
    public class ModalDialogModel
    {
        public MvcHtmlString Body { get; set; }
        public MvcHtmlString Buttons { get; set; }
        public string Title { get; set; }
        public string Id { get; set; }
        

        public ModalDialogModel(MvcHtmlString body, MvcHtmlString buttons, string title, string id)
        {
            Body = body;
            Buttons = buttons;
            Title = title;
            Id = id;
        }
    }
}