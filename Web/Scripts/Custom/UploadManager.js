/// <reference path="../jquery-2.2.3.intellisense.js" />
/// <reference path="../jquery-2.2.3.js" />
/// <reference path="../_vs2012.intellisense.js" />

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