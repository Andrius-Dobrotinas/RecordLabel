/// <reference path="../jquery-2.2.3.intellisense.js" />
/// <reference path="../jquery-2.2.3.js" />
/// <reference path="../_vs2012.intellisense.js" />

$(document).ready(function () {
    IMAGESET.ImageViewer.Create("img[name=thumbnail]");
});

IMAGESET = {};

IMAGESET.ImageViewer = function (triggersSelector) {
    this.viewBox = $(".img-view-box");
    this.triggers = $(triggersSelector);

    var me = this;
    this.triggers.click(function () {
        var dataId = $(this).attr("data-id");
        var currentId = me.viewBox.data("current-id");
        if (currentId) {
            var currentImage = me.viewBox.find("[data-id='" + currentId + "']");
            currentImage.hide();
        }
        var image = me.viewBox.find("[data-id='" + dataId + "']");
        image.show();
        me.viewBox.data("current-id", dataId);
    });
}

IMAGESET.ImageViewer.Create = function (triggersSelector) {
    return new IMAGESET.ImageViewer(triggersSelector);
}