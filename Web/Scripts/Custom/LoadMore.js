/// <reference path="../jquery-2.2.3.intellisense.js" />
/// <reference path="../jquery-2.2.3.js" />
/// <reference path="../../_vs2012.intellisense.js" />

/**
 * description Responsible for loading more results to the list
 * @param {string} loadButton jQuery selector for an Html element that will function as button
 * @param {string} destintionList jQuery selector for an Html element that will be used as container for new items 
 */
LoadMore = function (loadButton, destintionList) {
    this.srcElement = $(loadButton);

    // Store this object with the source element
    //this.srcElement.data("object", this); --> TODO: think if I should do this

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