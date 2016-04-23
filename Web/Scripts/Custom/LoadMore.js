/// <reference path="../jquery-1.10.2.intellisense.js" />
/// <reference path="../jquery-1.10.2.js" />
/// <reference path="../../_vs2012.intellisense.js" />

$(function () {
    new LoadMore("#LoadMore", "#TheList");
});

/**
 * description Responsible for loading more results to the list
 * @param {string} jQuery selector
 * @param {string} jQuery selector
 */
LoadMore = function (loadButton, destintionList) {
    this.srcElement = $(loadButton);
    this.list = $(destintionList);
    this.loader = $("#loader", this.srcElement);
    this.button = $("div", this.srcElement);
    this.url = this.srcElement.data("url");
    this.sourceId = this.srcElement.data("source-id");
    this.count = this.srcElement.data("count");
    this.batchSize = this.srcElement.data("batch-size");
    this.currentBatch = 0;

    if (typeof(this.sourceId) === "string" && this.sourceId.length === 0) {
        this.sourceId = null;
    }

    var me = this;
    this.srcElement.click(function () {
        me.LoadMore();
    })
}

LoadMore.prototype.LoadMore = function () {
    this.button.hide();
    this.loader.show();

    var data = {
        batch: ++this.currentBatch
    };
    if (this.sourceId !== null) {
        data.id = this.sourceId;
    }
    var me = this;
    $.ajax({
        url: this.url,
        data
    }).success(function (data) {
        me.list.append(data);
        if ((me.currentBatch + 1) * me.batchSize >= me.count) {
            me.hide();
        }
    }).error(function (xhr) {
        --me.currentBatch;
    }).complete(function () {
        me.loader.hide();
        me.button.show();
    });
}

LoadMore.prototype.successCallback = function (data) {
    this.list.append(data);
}

LoadMore.prototype.hide = function () {
    this.srcElement.hide();
}