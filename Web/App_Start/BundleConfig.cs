using System.Web.Optimization;

namespace RecordLabel.Web
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //Layout styles and scrips
            bundles.Add(new StyleBundle("~/Css/_BaseLayout")
                .Include("~/Content/Site.css")
                .Include("~/Content/Views/Shared/Common.css")
                .Include("~/Content/bootstrap.css"));

            bundles.Add(new ScriptBundle("~/Js/_BaseLayout")
                .Include("~/Scripts/jquery-{version}.js")
                .Include("~/Scripts/bootstrap.js"));

            //Styles
            bundles.Add(new StyleBundle("~/Css/Articles/View")
                .Include("~/Content/Views/Articles/View.css")
                .Include("~/Content/Views/Shared/Media.css")
                .Include("~/Content/Views/Shared/ModalImageViewer.css"));

            bundles.Add(new StyleBundle("~/Css/Release/View")
                .Include("~/Content/Views/Release/View.css")
                .Include("~/Content/Views/Shared/Media.css")
                .Include("~/Content/Views/Shared/ModalImageViewer.css"));

            bundles.Add(new StyleBundle("~/Css/EditWithImages")
                .Include("~/Content/themes/base/all.css") //jQuery UI
                .Include("~/Content/Views/Shared/Edit/ImageSet.css")
                .Include("~/Content/Views/Shared/Edit/EditButtons.css")
                .Include("~/Content/Views/Shared/Edit/Common.css"));

            bundles.Add(new StyleBundle("~/Css/Edit")
                .Include("~/Content/Views/Shared/Edit/Common.css"));

            bundles.Add(new StyleBundle("~/Css/Shared/List")
                .Include("~/Content/Views/Shared/List.css")
                .Include("~/Content/Views/Shared/Edit/EditButtons.css"));

            bundles.Add(new StyleBundle("~/Css/Management")
                .Include("~/Content/Views/Shared/Management.css"));

            bundles.Add(new StyleBundle("~/Css/Home/Index")
                .Include("~/Content/Views/Home/Index.css")
                .Include("~/Content/Views/Shared/Media.css")
                .Include("~/Content/Views/Shared/Common.css")
                .Include("~/Content/Views/Shared/Edit/EditButtons.css"));

            //Scripts
            bundles.Add(new ScriptBundle("~/Js/Articles/View")
                .Include("~/Scripts/Custom/ImageViewer/ImageViewer.js")
                .Include("~/Scripts/Custom/ImageViewer/ModalImageViewer.js")
                .Include("~/Scripts/Views/Shared/Display/ImageSet.js"));

            bundles.Add(new ScriptBundle("~/Js/Release/View")
                .Include("~/Scripts/Custom/ImageViewer/ImageViewer.js")
                .Include("~/Scripts/Custom/ImageViewer/ModalImageViewer.js")
                .Include("~/Scripts/Views/Shared/Display/ImageSet.js"));

            bundles.Add(new ScriptBundle("~/Js/Articles/Edit")
                .Include("~/Scripts/jquery.validate.min.js")
                .Include("~/Scripts/jquery.validate.unobtrusive.min.js")
                .Include("~/Scripts/jquery-ui-{version}.js")
                .Include("~/Scripts/ckeditor/ckeditor.js")
                .Include("~/Scripts/ckeditor/plugins/divarea/plugin.js")
                .Include("~/Scripts/Views/Articles/Edit.js")
                .Include("~/Scripts/Views/Shared/Edit/Edit.js")
                .Include("~/Scripts/Views/Shared/Edit/InitImageUploader.js")
                .Include("~/Scripts/Custom/ImageUploader.js")
                .Include("~/Scripts/Custom/UploaderEditButtons.js")
                .Include("~/Scripts/Custom/Templates.js"));

            bundles.Add(new ScriptBundle("~/Js/Release/Edit")
                .Include("~/Scripts/jquery.validate.min.js")
                .Include("~/Scripts/jquery.validate.unobtrusive.min.js")
                .Include("~/Scripts/jquery-ui-{version}.js")
                .Include("~/Scripts/ckeditor/ckeditor.js")
                .Include("~/Scripts/ckeditor/plugins/divarea/plugin.js")
                .Include("~/Scripts/Views/Release/Edit.js")
                .Include("~/Scripts/Views/Shared/Edit/Edit.js")
                .Include("~/Scripts/Views/Shared/Edit/InitImageUploader.js")
                .Include("~/Scripts/Custom/ImageUploader.js")
                .Include("~/Scripts/Custom/UploaderEditButtons.js")
                .Include("~/Scripts/Custom/Templates.js"));

            bundles.Add(new ScriptBundle("~/Js/Artist/Edit")
                .Include("~/Scripts/jquery.validate.min.js")
                .Include("~/Scripts/jquery.validate.unobtrusive.min.js")
                .Include("~/Scripts/ckeditor/ckeditor.js")
                .Include("~/Scripts/Views/Shared/Edit/Edit.js")
                .Include("~/Scripts/Views/Shared/Edit/InitImageUploader.js")
                .Include("~/Scripts/Custom/ImageUploader.js")
                .Include("~/Scripts/Custom/UploaderEditButtons.js"));

            bundles.Add(new ScriptBundle("~/Js/Metadata/Edit")
                .Include("~/Scripts/jquery.validate.min.js")
                .Include("~/Scripts/jquery.validate.unobtrusive.min.js")
                .Include("~/Scripts/ckeditor/ckeditor.js")
                .Include("~/Scripts/Views/Shared/Edit/Edit.js"));

            bundles.Add(new ScriptBundle("~/Js/Shared/List")
                .Include("~/Scripts/Custom/LoadMore.js"));
        }
    }
}