/// <reference path="../jquery-2.2.3.intellisense.js" />
/// <reference path="../jquery-2.2.3.js" />
/// <reference path="../_vs2012.intellisense.js" />

/// <reference path="UploaderEditButtons.js" />

/* 
 * @description Deals with selecting, previewing and uploading images or sending commands to delete existing images
 */
var ImageUploader = function (id, imageManager) {
    var me = this;

    this.UploadManager = imageManager;

    var elements = function (id) {
        this.root = $(UploadManager.GetOjectSelectorString(imageManager._itemIdentifier, id));//$(ImageUploader._imageIdentifier + "[data-id='" + id + "']");
        this.order = this.root.find(imageManager._orderIdentifier);
        this.previewImage = this.root.find("img[name='preview']");
        this.preview = this.previewImage.parent();
        this.chooseImageCaption = this.root.find("span.chooseimg");
        this.chooseImage = this.root.find("div.chooseimg");
        this.fileInput = this.root.find("input[type='file']");
        this.progress = this.root.find("progress");
        //this.submitButton = this.root.find("input[type='button']");
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
    /*this.elements.submitButton.click(event, function () {
        me.UploadFile();
    });*/
}


//METHODS*****************************************

ImageUploader.prototype.ChooseImage = function() {
    this.elements.fileInput.trigger("click");
};

/*ImageUploader.prototype.UploadFile = function() {
    if (this.elements.fileInput[0].files.length > 0) {
        this.UploadFileAjax();
    }
};*/

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

/*UploadManager._postUrl = "/api/Image";
ImageUploader.prototype.UploadFileAjax = function () {
    var me = this;
    $.ajax({
        url: UploadManager._postUrl,
        method: "POST",
        data: new FormData().append("file", this.elements.fileInput[0].files[0]).append("id"), //remove append(id)?
        contentType: false,
        processData: false,
        xhr: function () {
            var xhr = new window.XMLHttpRequest();
            xhr.upload.addEventListener("progress", function (e) {
                if (e.lengthComputable) {
                    var completePercent = e.loaded / e.total * 100;
                    console.log(completePercent);
                    me.elements.progress.val(completePercent);
                }
            });
        }
    });
}*/

ImageUploader.prototype.Remove = function () {
    this.elements.root.remove();
    this.UploadManager.RemoveUploader(this);
}

/*
 * @param {jQuery} itemContainer
 * @param {string} itemSelector
 * @param {string} orderIdentifier
 */
UploadManager = function (itemContainer, itemSelector, orderIdentifier) {
    this.Uploaders = new Array();
    this.ItemContainer = itemContainer;
    this._lastId = 0;
    this._templateGetUrl = this.ItemContainer.attr("data-template-url");
    this._deleteUrl = this.ItemContainer.attr("data-delete-url");
    this._itemIdentifier = itemSelector;
    this._orderIdentifier = orderIdentifier;
}

UploadManager.prototype.AttachTo = function (id) {
    var uploader = new ImageUploader(id, this);
    this.Uploaders.push(uploader);
    return uploader;
}

UploadManager.prototype.AttachToAll = function () {
    var me = this;
    $(me._itemIdentifier).each(function () {
        //set element/object identification
        var id = ++me._lastId;
        this.setAttribute("data-id", id);

        me.AttachTo(id);
    });
}

/*
 * @param {jQuery} loader An element to be displayed during ajax calls
 */
UploadManager.prototype.AddUploader = function (loader) {
    var me = this;

    loader.show();

    $.ajax({
        url: me._templateGetUrl,
        //data: "id=" + this._lastId
    }).done(function (div) {
        //div is pure text //set element identification
        var id = ++me._lastId;
        div = $(div).attr("data-id", id)[0];

        me.ItemContainer.append(div);
        var uploader = me.AttachTo(id);
        var order = me.GetLastOrderPosition() + 1;
        uploader.setOrder(order);

        loader.hide();
    }).error(function (result) {
        alert("An error has occured. Error code: " + result.status + " (" + result.statusText + ")");
    });
}

UploadManager.prototype.RemoveUploader = function (uploader) {
    var index = this.Uploaders.indexOf(uploader);
    if (index > -1) {
        this.Uploaders.splice(index, 1);
    };
}

UploadManager.prototype.GetLastOrderPosition = function () {
    var lastPosition = -1;
    this.Uploaders.forEach(function (uploader) {
        var position = uploader.getOrder();
        if (position > lastPosition) {
            lastPosition = position;
        }
    });
    return lastPosition;
}

UploadManager.prototype.GetLastId = function () {
    var lastId = 0;
    this.Uploaders.forEach(function (uploader) {
        var id = uploader.getOrder();
        if (id > lastId) {
            lastId = id;
        }
    });
    return lastId;
}

UploadManager.prototype.UpdateLastId = function (id) {
    if (this._lastId < id) {
        this._lastId = id;
    }
}

UploadManager.GetOjectSelectorString = function (itemIdentifier, id) {
    return itemIdentifier + "[data-id='" + id + "']";
}