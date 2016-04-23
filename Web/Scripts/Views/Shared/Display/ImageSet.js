$(function () {
    new ImageViewer(".img-view-box", ".img-container img", "#images img[name=thumbnail]",
        {
            eventType: "mouseover",
            delegate: ImageViewer.DefaultImageViewEventHandler
        });
    new ModalImageViewer("#ModalImageViewer", "[name=image-view]", "#images .img-container img");
});