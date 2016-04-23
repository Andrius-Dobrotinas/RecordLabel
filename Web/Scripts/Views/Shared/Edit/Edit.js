$(function () {

    //Create Rich Text Editors
    $("textarea.rich-text").each(function () {      
        CKEDITOR.replace(this.id, {
            extraPlugins: 'divarea'
        });
    });

    /*tinymce.init({
        selector: 'textarea.rich-text'
        //auto_focus: 'element1'
    });*/

    //Attach Delete button
    $("#deleteBtn").click(function () {
        return confirm("Do you really want to delete this item?");
    });
});