/// <reference path="../jquery-1.10.2.intellisense.js" />
/// <reference path="../jquery-1.10.2.js" />
/// <reference path="_vs2012.intellisense.js" />
/// <reference path="../_vs2012.intellisense.js" />


AutoHide = function (targetElementSelector, contextObject) {
    this.contextObject = null;

    //TODO: create a static method global in a namespace for this method
    if (contextObject instanceof jQuery) {
        this.contextObject = contextObject;
    }
    else {
        this.contextObject = $(contextObject);
    }

    this.element = $(targetElementSelector, this.contextObject);
    //this.edit = this.editButtonBlock.find("[data-action='edit']");
    //this.delete = this.editButtonBlock.find("input[data-action='delete']");

    var me = this;

    this.contextObject.mouseover(function (event) {
        me.element.show();
    });

    this.contextObject.mouseout(function (event) {
        me.element.hide();
    });
}

AutoHide.selector = ".auto-hide";

///Context Object => container for the Auto-hide object whose mouse-over/out behaviour is to be associated with the Auto-hide object
AutoHide.AttachToAll = function (contextObjectSelector) {
    //TODO: use that global jQuery auto selector from above
    $(contextObjectSelector).each(function (index, item) {
        new AutoHide(AutoHide.selector, item);
    });
}

/*Entity.prototype.Edit = function () {
    alert('openedit');
}*/

$(document).ready(function () {
    /*$("div.media").each(function (index, item) {
        new Entity(_editButtonBlockSelector, item);
    });*/
    AutoHide.AttachToAll("div.media");
});