/// <reference path="../../jquery-2.2.3.intellisense.js" />
/// <reference path="../../jquery-2.2.3.js" />
/// <reference path="../../_vs2012.intellisense.js" />

/// <reference path="../../Custom/Templates.js" />

var references;
var tracklist;

$(function () {
    references = new TEMPLATES.ItemManager("#Templates", "#addReferenceBtn", "#ModelReferences");
    references.AttachToAll();
    
    tracklist = new TEMPLATES.ItemManager("#Templates", "#addTrackBtn", "#Tracklist");
    tracklist.AttachToAll();

    new Versions("IsMasterVersion", "MasterVersionId");


    //Initialize Date spinners
    function spinnerCallback(spinner, event, ui) {
        if (ui.value > 2099) {
            spinner.spinner("value", 1950);
            return false;
        } else if (ui.value < 1950) {
            spinner.spinner("value", 2099);
            return false;
        }
    }
    $("#Date").spinner({
        spin: function (event, ui) {
            return spinnerCallback($(this), event, ui);
        }
    });
    $("#DateRecorded").spinner({
        spin: function (event, ui) {
            return spinnerCallback($(this), event, ui);
        }
    });
});

/*Versions menu and IsMasterVersion checkbox*/
Versions = function (sourceId, targetId) {
    /*this.source = $(sourceInputSelector);
    this.target = $(targetInputSelector);
    this.source.change(function (event, data) {
        me.target.prop("disabled", this.checked);
    });*/
    var me = this;
    this.source = document.getElementById(sourceId);
    this.target = document.getElementById(targetId);

    this.source.addEventListener("change", function () {
        me.target.disabled = this.checked;
    });
}
/*-------------------------------*/

function editreference(that) {
    var me = $(that);
    var element = me.parents(".reference-view-line").first().siblings(".reference-edit").first();
    if (element.css("display") === "none") {
        element.show();
    } else {
        element.hide();
    }
}