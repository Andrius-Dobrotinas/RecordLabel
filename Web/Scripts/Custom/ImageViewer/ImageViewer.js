/// <reference path="../../jquery-1.10.2.intellisense.js" />
/// <reference path="../../jquery-1.10.2.js" />
/// <reference path="../../_vs2012.intellisense.js" />

/**
 * @class
 * @description Controls an individual image
 * @param {int} index And index that's to be assigned to this object
 * @param {jQuery} thumbnail A thumbnail element that serves as a trigger for the displaying of the image that this object controls
 * @param {jQuery} imageViewContainer An element that contains this image
 * @param {string} imageViewSelector Selector for an element within imageViewContainer that contain the target full-sized image
 * @param {ImageViewer} managerHandle An instance of ImageViewer that controls this ImageView
 */
ImageView = function (index, thumbnail, imageViewContainer, imageViewSelector, managerHandle) {
    this.index = index;
    this.id = thumbnail.data("id");
    this.element = $(imageViewSelector + "[data-id='" + this.id + "']", imageViewContainer);
    this.thumbnail = thumbnail;
}

ImageView.prototype.Hide = function () {
    this.element.hide();
}

ImageView.prototype.Show = function () {
    this.element.show();
}

/**
 * @class
 * @description Controls the displaying of images when triggered by respective thumbnails
 * @param {string} imageViewContainer Selector for an element that contains image views (full-sized target images that are to be displayed)
 * @param {string} imageView Selector for a individual elements within imageViewContainer that contain target full-sized images
 * @param {string} thumbnail Selector for a individual elements that are to serve as triggers the displaying of imageViews' images
 * @param {eventHandlerDelegate=} eventHandlerDelegate An object { eventType {string}, delegate {function} == event handler function }. Default one handles click event
 */
ImageViewer = function (imageViewContainer, imageView, thumbnail, eventHandler) {
    var thumbnails = $(thumbnail);
    if (thumbnails.length == 0) {
        return; //Exit if no thumbnails found
    }

    this.element = $(imageViewContainer);
    this.imageViews = new Array();

    var me = this;

    //If not event handler object  passed, use the default one
    if (eventHandler === undefined) {
        eventHandler = { eventType: "click", delegate: ImageViewer.DefaultImageViewEventHandler };
    }

    thumbnails.each(function (index) {
        var view = new ImageView(index, $(this), me.element, imageView, me)
        me.imageViews.push(view);

        //Attach event handlers
        view.thumbnail.on(eventHandler.eventType, function () {
            eventHandler.delegate(view, me);
        });
    });
    this.topIndex = this.imageViews.length - 1;

    this.View(this.imageViews[0]);

    //Prev & Next buttons
    this.prevBtn = $("button[name=prev]", imageViewContainer);
    this.nextBtn = $("button[name=next]", imageViewContainer);
    if (this.prevBtn !== undefined) {
        this.prevBtn.click(function () {
            me.Previous();
        });
    }
    if (this.nextBtn !== undefined) {
        this.nextBtn.click(function () {
            me.Next();
        });
    }
}

ImageViewer.prototype = {
    element: undefined,
    imageViews: undefined,
    topIndex: undefined,
    currentImage: null,
    prevBtn: undefined,
    nextBtn: undefined

}

ImageViewer.prototype.View = function (image) {
    if (this.currentImage === image) {
        return;
    }

    if (this.currentImage !== null) {
        this.currentImage.Hide();
    }

    this.currentImage = image;
    this.currentImage.Show();
}

ImageViewer.prototype.Previous = function () {
    var newIndex;
    if (this.currentImage.index > 0) {
        newIndex = this.currentImage.index - 1;
    } else {
        newIndex = this.topIndex;
    }
    this.View(this.imageViews[newIndex]);
}

ImageViewer.prototype.Next = function () {
    var newIndex;
    if (this.currentImage.index < this.topIndex) {
        newIndex = this.currentImage.index + 1;
    } else {
        newIndex = 0;
    }
    this.View(this.imageViews[newIndex]);
}

ImageViewer.DefaultImageViewEventHandler = function (imageView, imageViewerHandle) {
    imageViewerHandle.View(imageView);
}