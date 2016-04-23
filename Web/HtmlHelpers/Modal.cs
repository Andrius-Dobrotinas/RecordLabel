using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Reflection;

namespace RecordLabel.Web
{
    public static class Modal
    {
        public static MvcHtmlString ModalDialog(this HtmlHelper helper, string id, MvcHtmlString contents, string title)
        {
            return ModalDialog(helper, id, contents, title, null);
        }

        /// <summary>
        /// Returns a modal dialog
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="id">Modal dialog Id</param>
        /// <param name="contents">Contents of the dialog</param>
        /// <param name="title">Modal dialog title</param>
        /// <param name="buttons">Extra buttons for the modal dialog (by default it contains an OK button)</param>
        /// <returns></returns>
        public static MvcHtmlString ModalDialog(this HtmlHelper helper, string id, MvcHtmlString contents, string title, MvcHtmlString extraButtons)
        {
            return helper.Partial("ExtensionPartials/Modal", new ModalDialogModel(contents, extraButtons, title, id));
        }

        /// <summary>
        /// Returns a button for opening a modal dialog
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="modalId">Id of a modal dialog to open</param>
        /// <param name="buttonText">Button text</param>
        /// <returns></returns>
        public static MvcHtmlString ModalOpenButton(this HtmlHelper helper, string modalId, string buttonText)
        {
            return helper.Button(buttonText, null, null, new { data_toggle = "modal", data_target = $"#{modalId}" }, false);
        }
    }
}