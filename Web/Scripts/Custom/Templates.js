TEMPLATES = {};

TEMPLATES.Template = function (templatesContainer, templateName, targetContainer) {
    this.root = templatesContainer.children("[name=" + templateName + "]").first();
    this.propertyName = this.root.attr("data-property-name");
    this.propertyId = this.root.attr("data-property-id");

    this._templateName = templateName;
    this._targetContainer = targetContainer;
}

/*
 * @description Creates an object from the template with appropriate input field names and ids
 */
TEMPLATES.Template.prototype.Instantiate = function (index) {

    var objectInstance = this.root.clone().appendTo(this._targetContainer);

    //Set input ids and names prefixes for them to serve as model properties
    var idPrefix = this.propertyId.concat("_", index.toString(), "__");
    var namePrefix = this.propertyName.concat("[", index.toString(), "].");

    $("input:not([data-name='indexField']):not([data-name='idField']), select", objectInstance).each(function (index, element) {
        var currentId = element.getAttribute("id");
        var currentName = element.getAttribute("name");
        element.setAttribute("id", idPrefix.concat(currentId));
        element.setAttribute("name", namePrefix.concat(currentName));
    })

    /*var idField = $("input[data-name='idField']", objectInstance)[0];
    idField.setAttribute("id", idPrefix.concat("Id"));
    idField.setAttribute("name", namePrefix.concat("Id"));*/

    $("label", objectInstance).each(function (index, element) {
        var currentFor = element.getAttribute("for");
        element.setAttribute("for", idPrefix.concat(currentFor));
    })

    objectInstance.find("input[data-name='indexField']").attr("value", index);

    return objectInstance;
}



TEMPLATES.ItemInstance = function (jObject, itemManager) {
    var me = this;

    this.ItemManager = itemManager;

    var elements = function (jObject) {
        this.root = jObject;
        this.indexField = this.root.find("input[name='indexFieldName']");
        this.removeButton = this.root.find("button[name='remove']"); //change
    }
    this.elements = new elements(jObject);

    this.getIndex = function () {
        var value = $("input[data-name='indexField']", this.elements.root).attr("value");
        return parseInt(value);
    }

    this.elements.removeButton.click(event, function () {
        me.Disable();
        me.Hide();
    });
}

TEMPLATES.ItemInstance.prototype.Disable = function () {
    $("input, textarea, select", this.elements.root).each(function() {
        this.setAttribute("disabled", "disabled");
    });
}

TEMPLATES.ItemInstance.prototype.Enable = function () {
    $("input, textarea, select", this.elements.root).each(function () {
        this.removeAttribute("disabled");
    });
}

TEMPLATES.ItemInstance.prototype.Remove = function () {
    $(document).remove(this.elements.root);
    this.ItemManager.RemoveItemInstance(this);
}

TEMPLATES.ItemInstance.prototype.Hide = function () {
    this.elements.root.hide();
}

////////////////////////////////////////////////////////////////////////////////
TEMPLATES.ItemManager = function (templatesContainerId, addButton, containerId) {
    this.ItemsFromTemplate = new Array();
    this._lastId = 0;
    this._templatesContainerId = $(templatesContainerId);

    this.container = $(containerId);
    this._objectName = this.container.attr("data-container-for");

    this.Template = new TEMPLATES.Template(this._templatesContainerId, this._objectName, containerId);

    var me = this;
    $(addButton).click(event, function () {
        me.AddItemInstance();
    });
}

TEMPLATES.ItemManager.prototype.AttachTo = function (jObject) {
    var uploader = new TEMPLATES.ItemInstance(jObject, this);
    this.ItemsFromTemplate.push(uploader);
    return uploader;
}

TEMPLATES.ItemManager.prototype.AttachToAll = function () {
    var me = this;
    this.container.children("[name=" + this._objectName + "]").each(function () {
        var uploader = me.AttachTo($(this));
        //Set index of last item to know what index to use when adding a new item via template
        me._lastId = uploader.getIndex();
    });
}

TEMPLATES.ItemManager.prototype.AddItemInstance = function () {
    var uploader = this.Template.Instantiate(++this._lastId);
    this.AttachTo(uploader);
}

TEMPLATES.ItemManager.prototype.RemoveItemInstance = function (editor) {
    var index = this.ItemsFromTemplate.indexOf(uploader);
    if (index > -1) {
        this.ItemsFromTemplate.splice(index, 1);
    };
}