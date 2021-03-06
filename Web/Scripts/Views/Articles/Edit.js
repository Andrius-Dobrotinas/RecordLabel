﻿/// <reference path="../../jquery-2.2.3.intellisense.js" />
/// <reference path="../../jquery-2.2.3.js" />
/// <reference path="../../_vs2012.intellisense.js" />

/// <reference path="../../Custom/Templates.js" />

var references;

$(document).ready(function () {
    references = new TEMPLATES.ItemManager("#Templates", "#addReferenceBtn", "#ModelReferences");
    references.AttachToAll();

    //Initialize date picker
    $(function () {
        $("#Date").datepicker({
            dateFormat: "mm/dd/yy",
            changeMonth: true,
            changeYear: true
        });
    });
});