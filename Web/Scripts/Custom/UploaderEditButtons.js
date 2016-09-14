/// <reference path="../jquery-2.2.3.intellisense.js" />
/// <reference path="../jquery-2.2.3.js" />
/// <reference path="../_vs2012.intellisense.js" />

UploaderEditButtons = function (contextObject, editButtonBlockSelector, uploader, deleteCallback, deleteUrl) {
    this.root = $(contextObject);
    this.editButtonBlock = $(editButtonBlockSelector, this.root);
    //this.edit = this.editButtonBlock.find("input[data-action='edit']");
    this.delete = this.editButtonBlock.find("button[data-action='delete']");

    this.deleteUrl = deleteUrl;

    this.uploader = uploader;
    this.deleteCallback = deleteCallback;

    var me = this;

    this.root.mouseover(function (event) {
        me.editButtonBlock.show();
    });

    this.root.mouseout(function (event) {
        me.editButtonBlock.hide();
    });

    this.delete.click(function () {
        me.Delete();
    });
}

UploaderEditButtons.prototype.Delete = function () {
    if (this.uploader.__isfromModel === true) {
        if (confirm("Are you sure?")) {
            var me = this;
            var data = new FormData();
            data.append("id", this.uploader.backendId);
            $.ajax({
                url: me.deleteUrl,
                method: "POST",
                data: data,
                contentType: false,
                processData: false,
                success: function (data, textStatus, jqXHR) {
                    if (jqXHR.status === 200) {
                        me.deleteCallback();
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert("Couldn't delete this file");
                }
            })
        };
    };
}