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
});

/*Versions menu and IsMasterVersion checkbox*/
Versions = function (sourceId, targetId) {
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