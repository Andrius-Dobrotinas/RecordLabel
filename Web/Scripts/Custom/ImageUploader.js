/// <reference path="../jquery-2.2.3.intellisense.js" />
/// <reference path="../jquery-2.2.3.js" />
/// <reference path="../_vs2012.intellisense.js" />

/// <reference path="UploadManager.js" />
/// <reference path="UploaderEditButtons.js" />

/* 
 * @description Deals with selecting, previewing and uploading images or sending commands to delete existing images
 */
var ImageUploader = function (id, imageManager) {
    var me = this;

    this.UploadManager = imageManager;

    var elements = function (id) {
        this.root = $(UploadManager.GetOjectSelectorString(imageManager._itemIdentifier, id));
        this.order = this.root.find(imageManager._orderIdentifier);
        this.previewImage = this.root.find("img[name='preview']");
        this.preview = this.previewImage.parent();
        this.chooseImageCaption = this.root.find("span.chooseimg");
        this.chooseImage = this.root.find("div.chooseimg");
        this.fileInput = this.root.find("input[type='file']");
        this.progress = this.root.find("progress");
    }
    this.elements = new elements(id);
    this.id = id;
    this.__isfromModel = this.elements.root.attr("data-from-model") === "1";
    this.backendId = this.elements.root.attr("data-backendId");

    if (this.__isfromModel === true) {
        this.Disable();
    }

    this.editButtons = new UploaderEditButtons(this.elements.root, "div.edit-btn-block",
        this, function () {
            me.Remove()
        }, this.UploadManager._deleteUrl);

    //Attach to events
    this.elements.chooseImage.click(event, function () {
        me.ChooseImage();
    });
    this.elements.fileInput.change(event, function () {
        me.PreviewImage();
    });
}


//METHODS*****************************************

ImageUploader.prototype.ChooseImage = function() {
    this.elements.fileInput.trigger("click");
};

ImageUploader.prototype.PreviewImage = function () {
    if (this.elements.fileInput[0].files.length === 0) {
        this.elements.preview.addClass("hidden");
        this.elements.previewImage.removeAttr("src");
        return false;
    }
    this.elements.preview.removeClass("hidden");

    var me = this;
    var reader = new FileReader();
    reader.onload = function (event) {
        me.elements.previewImage[0].src = event.target.result;
    };
    reader.readAsDataURL(this.elements.fileInput[0].files[0]);
};

ImageUploader.prototype.getOrder = function () {
    return parseInt(this.elements.order.val());
};

ImageUploader.prototype.setOrder = function (value) {
    this.elements.order.val(value);
};

ImageUploader.prototype.Disable = function () {
    this.elements.fileInput.attr("disabled", "disabled");
};

ImageUploader.prototype.getIsEmpty = function() {
    var value = this.elements.fileInput.attr("src");
    return typeof(value) === "undefined" || (typeof(value) === "string" && value.length === 0);
}
ImageUploader.prototype.Remove = function () {
    this.elements.root.remove();
    this.UploadManager.RemoveUploader(this);
}