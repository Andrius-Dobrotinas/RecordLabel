/// <reference path="../../jquery-2.2.3.intellisense.js" />
/// <reference path="../../jquery-2.2.3.js" />
/// <reference path="../../_vs2012.intellisense.js" />

/// <reference path="ImageViewer.js" />

/**
 * @class
 * @description
 * @param {string} imageViewContainer Selector for an element that contains image views (full-sized target images that are to be displayed)
 * @param {string} imageView Selector for a individual elements within imageViewContainer that contain target full-sized images
 * @param {string} thumbnail Selector for a individual elements that are to serve as triggers the displaying of imageViews' images
 */
ModalImageViewer = function (imageViewContainer, imageView, thumbnail) {
    ImageViewer.call(this, imageViewContainer, imageView, thumbnail,
        {
            eventType: "click",
            delegate: ModalImageViewer.DefaultImageViewEventHandler
        });

    this.closeButton = $("[name=close]", this.element);
    this.isActive = false;
    
    var me = this;

    this.closeButton.click(function () {
        me.Close();
    });

    window.onkeydown = function () {
        if (me.isActive === true && event.keyCode === 27) {
            me.Close();
        }
    };
}
ModalImageViewer.prototype = Object.create(ImageViewer.prototype);
//ModalImageViewer.prototype.constructor = ModalImageViewer;

ModalImageViewer.prototype.Open = function () {
    this.element.show();
    this.isActive = true;
}

ModalImageViewer.prototype.Close = function () {
    this.element.hide();
    this.isActive = false;
}

ModalImageViewer.DefaultImageViewEventHandler = function (imageView, imageViewerHandle) {
    imageViewerHandle.View(imageView);
    imageViewerHandle.Open();
}