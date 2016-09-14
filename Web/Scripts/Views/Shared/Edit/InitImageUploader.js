/// <reference path="../../../jquery-2.2.3.intellisense.js" />
/// <reference path="../../../jquery-2.2.3.js" />
/// <reference path="../../../_vs2012.intellisense.js" />

/// <reference path="../../../Custom/ImageUploader.js" />

var imageManager;

$(document).ready(function () {
    var addImageButton = $("#addImageBtn");
    var loader = $("#loader");

    imageManager = new UploadManager(
        $(addImageButton.attr("data-img-container-id")),
        "div[name='Image']", "input[name='imageOrder']");

    addImageButton.click(event, function () {
        imageManager.AddUploader(loader);
    });

    imageManager.AttachToAll();
});