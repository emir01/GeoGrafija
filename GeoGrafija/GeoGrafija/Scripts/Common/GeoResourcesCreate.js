// Handles the creation of location resources.
var GeoResourcesCreate = (function () {

    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    // ===   Resource Creation  module Parameters, Constants, And Configuration Values  ------------------------------------------------------------------------------------
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------

    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    // ===   Resource Creation  module functions and methods  --------------------------------------------------------------------------------------------------------------
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------

    function init() {
        setupUi();
        setupEvents();
    };

    function setupUi() {

        $("#ResourceTypeDropdown").dropkick({
            change: onResourceTypeChange
        });

        $(".resourceTypeWrapper").hide();

        //set the heading color as per type
        $(".resourceTypeWrapper .formHeader").each(function () {
            var header = $(this);
            var color = header.attr("data-color");
            if (color && color != "") {
                header.css("color", "#" + color);
            }
        });

        $(".resourceTypeWrapper").filter(function () {
            return $(this).attr("data-id") == $("#ResourceTypeDropdown").val();
        }).slideDown();
    }

    function setupEvents() {
        $(".resourceCreateButton").click(resourceCreateClick);

        $(".resourceDelete").live("click", deleteResouce);
        $(".resourceDetails").live("click", resourceDetails);
    }

    // ===                                       ------------------------------------------------------------------------------------------------------
    // ===  Main Funcitons and Event Handlers   ------------------------------------------------------------------------------------------------------
    // ===                                      ------------------------------------------------------------------------------------------------------

    function deleteResouce(event) {
        var resourceId,
            locationId,
            url,
            dataArray,
            jsonObject,
            controller = "ResourcesCreation",
            action = "DeleteResource";

        resourceId = $(this).attr("data-id");
        locationId = $("#LocationId").val();

        url = GeoAjax.GetUrlForAction(controller, action);

        dataArray = new GeoAjax.JsonDataArray();
        dataArray.AddValue("resourceId", resourceId);
        dataArray.AddValue("locationId", locationId);

        jsonObject = dataArray.GetJsonObject();

        GeoAjax.MakeAjaxPost(url, jsonObject, resourceDeleteSuccess, resourceDeleteFail);

        return false;
    }

    function resourceDeleteSuccess(data) {
        var dialog;

        if (data.IsOk) {
            dialog = GeoDialogFactory.InformationDialog("ResourceDeleteFine", "Порака!", data.Message, 400, 200);
            dialog.Open();
            deleteRow(data.Data);
        } else {
            dialog = GeoDialogFactory.InformationDialog("ResourceDeleteCallOKProcFail", "Порака!", data.Message, 400, 200);
            dialog.Open();
        }
    }

    function resourceDeleteFail(data) {
        var dialog;
        dialog = GeoDialogFactory.InformationDialog("ResourceDeleteFine", "Порака!", "Грешка  при бришење на ресурс. Обиди се повторно!", 400, 200);
        dialog.Open();

    }

    function resourceDetails(event) {
        var resourceId,
            url,
            action = "GetResourceDetails",
            controller = "ResourcesCreation",
            jsondata;

        resourceId = $(this).attr("data-id");

        url = GeoAjax.GetUrlForAction(controller, action);
        jsondata = GeoAjax.SimpleAjaxParam(resourceId, "resourceId");

        GeoAjax.MakeAjaxPost(url, jsondata, getResourceDetailsSucecss, getResourceDetailsFail);

        return false;
    }

    function getResourceDetailsSucecss(msg) {
        var dialog;

        if (msg.IsOk) {
            showResourceDetailsDialog(msg);
        } else {
            dialog = GeoDialogFactory.InformationDialog("ResourceDetailsFail", "Порака!", "Грешка  при прикажување на детали за ресурс. Обиди се повторно!", 400, 200);
            dialog.Open();
            return false;
        }
    }

    function getResourceDetailsFail(msg) {
        var dialog;
        dialog = GeoDialogFactory.InformationDialog("ResourceDetailsFail", "Порака!", "Грешка  при прикажување на детали за ресурс. Обиди се повторно!", 400, 200);
        dialog.Open();
        return false;
    }

    function onResourceTypeChange(value, label) {
        $(".resourceTypeWrapper").hide();
        $(".resourceTypeWrapper").filter(function () {
            return $(this).attr("data-id") == value;
        }).slideDown();
    }

    function resourceCreateClick(event) {
        var resourceTypeId,
            resourceTypeName;

        resourceTypeName = $(this).attr("data-typename");
        resourceTypeId = $(this).attr("data-typeid");

        var dialogMarkup = $("#ResourceCreateDialog");
        dialogMarkup.find("#HiddenResourceDialogName").val(resourceTypeName);
        dialogMarkup.find("#HiddenResourceDialogId").val(resourceTypeId);
        dialogMarkup.find("#CreateResourceTypeName").html(resourceTypeName);

        //directly create the dialog 
        dialogMarkup.dialog({
            autoOpen: false,
            open: onDialogOpen,
            close: onDialogClose,
            modal: true,
            buttons: getButtonsConfigCreate(),
            width: 600,
            height: 600,
            title: resourceTypeName
        });

        dialogMarkup.dialog("open");
    }


    function onDialogClose() {
        $('#CreateResourceText').wysiwyg('clear');
        $("#ResourceNameValue").val('');
    }

    function onDialogOpen() {
        var calculatedHeight,
            textArea,
            richEditor;

        textArea = $("#CreateResourceText");
        calculatedHeight = calcualteTextAreaHeight();

        if ($(".wysiwyg", "#ResourceCreateDialog").size() == 0) {
            textArea.wysiwyg();
        }

        richEditor = $(".wysiwyg", "#ResourceCreateDialog");
        richEditor.width(richEditor.closest(".padding").width() - 10);
        $("iframe", richEditor).height(350);
    }

    function onDialogSubmit(event) {
        var typeName,
            typeId,
            text,
            name;

        typeName = $("#HiddenResourceDialogName").val();
        typeId = $("#HiddenResourceDialogId").val();
        text = $("#CreateResourceText").val();
        name = $("#ResourceNameValue").val();

        if (!validCreateUpdateForm(text, name)) {
            return;
        }
        
        saveResource(typeName, typeId, text, name);

        $('#CreateResourceText').wysiwyg('clear');
        $(this).dialog("close");
    }

    function onDialogCancel() {
        $('#CreateResourceText').wysiwyg('clear');
        $("#ResourceNameValue").val('');
        $(this).dialog("close");
    }

    //do ajax call and save resource
    function saveResource(typeName, typeId, text, name) {
        var locationId,

            controller = "ResourcesCreation",
            action = "CreateResources",
            url,
            dataArray,
            dataJson;

        locationId = $("#LocationId").val();

        url = GeoAjax.GetUrlForAction(controller, action);
        dataArray = GeoAjax.JsonDataArray();

        dataArray.AddValue("LocationId", locationId);
        dataArray.AddValue("Text", text);
        dataArray.AddValue("ResourceName", name);
        dataArray.AddValue("ResourceTypeId", typeId);
        dataArray.AddValue("ResourceName", name);

        dataJson = dataArray.GetJsonObject();

        GeoAjax.MakeAjaxPost(url, dataJson, resourceCreationPass, resourceCreationFail);
    }

    function resourceCreationPass(msg) {
        var dialog;
        dialog = GeoDialogFactory.InformationDialog("ResourceCreationFailAjax", "Порака!", msg.Message, 400, 200);
        insertDataInTable(msg);
        dialog.Open();
    }

    function resourceCreationFail(msg) {
        var dialog;

        dialog = GeoDialogFactory.InformationDialog("ResourceCreationFailAjax", "Порака!", "Неуспех при креирање на ресурс. Обиди се повторно!", 400, 200);
        dialog.Open();
    }

    // ===              ------------------------------------------------------------------------------------------------------
    // === Utilities    ------------------------------------------------------------------------------------------------------
    // ===              ------------------------------------------------------------------------------------------------------

    // ===                    ------------------------------------------------------------------------------------------------------
    // === Details Dialog    ------------------------------------------------------------------------------------------------------
    // ===                    ------------------------------------------------------------------------------------------------------

    function showResourceDetailsDialog(viewModel) {

        var dialogMarkup = $("#ResourceDetailsDialog");
        dialogMarkup.find("#HiddenDetailsResourceId").val(viewModel.Id);
        dialogMarkup.find("#DetailsResourceName").html(viewModel.Name);
        dialogMarkup.find("#DetailsResourceNameValue").val(viewModel.Name);

        dialogMarkup.find("#HiddenDetailsText").val(viewModel.Text);

        //directly create the dialog 
        dialogMarkup.dialog({
            autoOpen: false,
            open: onDetailsDialogOpen,
            close: onDetailsDialogClose,
            modal: true,
            buttons: getButtonsConfigDetails(),
            width: 600,
            height: 600,
            title: viewModel.Name
        });

        dialogMarkup.dialog("open");
    }

    function onDetailsSaveChanges() {
        var resourceId,
            newResourceName,
            newResourceText,
            url,
            controller,
            action,
            dataArray,
            dataJson;

        action = "UpdateResource";
        controller = "ResourcesCreation";

        url = GeoAjax.GetUrlForAction(controller, action);

        resourceId = $(this).find("#HiddenDetailsResourceId").val();
        newResourceName = $("#DetailsResourceNameValue").val();
        newResourceText = $('#DetailsResourceText').wysiwyg('getContent');

        if (!validCreateUpdateForm(newResourceText, newResourceName)) {
            return;
        }

        dataArray = new GeoAjax.JsonDataArray();
        dataArray.AddValue("Id", resourceId);
        dataArray.AddValue("Name", newResourceName);
        dataArray.AddValue("Text", newResourceText);
        dataJson = dataArray.GetJsonObject();

        GeoAjax.MakeAjaxPost(url, dataJson, onUpdateSuccess, onUpdateFailRequest);

        $(this).dialog("close");
    }

    function onUpdateSuccess(data, status, object) {
        var dialog;

        if (data.IsOk) {
            dialog = GeoDialogFactory.InformationDialog("UpdateTableRowSuccessResource", "Порака!", "Успешно променети информации за ресурс!", 400, 200);
            dialog.Open();
            updateRow(data);
        } else {
            dialog = GeoDialogFactory.InformationDialog("UpdateTableRowSuccessResourceFail", "Порака!", "Неуспешно променети информации за ресурс.Обиди се повторно!", 400, 200);
            dialog.Open();
        }
    }

    function onUpdateFailRequest(object, status, error) {
        var dialog;
        dialog = GeoDialogFactory.InformationDialog("UpdateTableRowSuccessResourceFail", "Порака!", "Неуспешно променети информации за ресурс.Обиди се повторно!", 400, 200);
        dialog.Open();
    }

    function onDetailsCancel() {
        $("#DetailsResourceNameValue").val('');
        $('#DetailsResourceText').wysiwyg('clear');
        $(this).dialog("close");
    }

    function onDetailsDialogClose() {
        $("#DetailsResourceNameValue").val('');
        $('#DetailsResourceText').wysiwyg('clear');

    }

    function onDetailsDialogOpen() {
        var calculatedHeight,
            textArea,
            richEditor;

        textArea = $("#DetailsResourceText");

        if ($(".wysiwyg", "#ResourceDetailsDialog").size() == 0) {
            textArea.wysiwyg();
        }

        richEditor = $(".wysiwyg", "#ResourceDetailsDialog");
        richEditor.width(richEditor.closest(".padding").width() - 10);
        $("iframe", richEditor).height(350);

        var textHidden = $(this).find("#HiddenDetailsText");
        $('#DetailsResourceText').wysiwyg('clear');
        $('#DetailsResourceText').wysiwyg('setContent', textHidden.val());
        textHidden.val("");
    }

    function insertDataInTable(msg) {
        var viewModel,
            activeTable;

        viewModel = msg.Data;
        activeTable = $(".resourceTypeWrapper").filter(function () {
            return $(this).attr("data-id") == viewModel.TypeId;
        }).find(".resourcesDisplayTable");

        var encoded = $('<div/>').text(viewModel.Text).html();

        var shortText = GeoUtility.Shorten(encoded, 100, '.');

        activeTable.dataTable()
        .fnAddData([
                        viewModel.Name,
                        shortText,
                        viewModel.TypeName,
                        viewModel.LocationName,
                        getActionsMarkup(viewModel.Id)
                  ]);
    };

    function deleteRow(resourceId) {
        var activeTable,
            tableRow,
            anchor;

        activeTable = $(".resourceTypeWrapper:visible").find(".resourcesDisplayTable");

        // under the active table find the acnhor that is connectected to this given resource id
        anchor = $("a", activeTable).filter(function () {
            return ($(this).attr("data-id") == resourceId) && $(this).hasClass("resourceDelete");
        });

        tableRow = anchor.closest("tr");

        activeTable.dataTable().fnDeleteRow(tableRow[0]);

        return false;
    }

    function updateRow(viewModel) {
        var resourceId,
            activeTable,
            anchor,
            tableRow;

        resourceId = viewModel.Id;

        activeTable = $(".resourceTypeWrapper:visible").find(".resourcesDisplayTable");

        // under the active table find the acnhor that is connectected to this given resource id
        anchor = $("a", activeTable).filter(function () {
            return ($(this).attr("data-id") == resourceId) && $(this).hasClass("resourceDelete");
        });

        tableRow = anchor.closest("tr");

        var encoded = $('<div/>').text(viewModel.Text).html();

        var nameTd = $(tableRow.children("td")[0]);
        var textTd = $(tableRow.children("td")[1]);

        nameTd.html(viewModel.Name);
        textTd.html(GeoUtility.Shorten(encoded, 100, '.'));

        return false;
    }

    function getActionsMarkup(resourceId) {
        var markup = '<a href="#" class="resourceDetails" data-id = "' + resourceId.toString() + '">Детали</a> | <a href="#"  class="resourceDelete" data-id = "' + resourceId.toString() + '">Бриши</a>';

        return markup;
    }

    function getButtonsConfigCreate() {
        var buttons = {};

        buttons["Креирај"] = onDialogSubmit;
        buttons["Откажи"] = onDialogCancel;

        return buttons;
    }

    function getButtonsConfigDetails() {
        var buttons = {};

        buttons["Зачувај Промени"] = onDetailsSaveChanges;
        buttons["Откажи"] = onDetailsCancel;

        return buttons;
    }

    // validation for required fields. 
    function validCreateUpdateForm(text, name) {
        text = text ? text != null ? text : "" : "";
        name = name ? name != null ? name : "" : "";
        var trimmedText;
        var trimmedName;

        var dialog;

        trimmedText = $.trim(text);
        trimmedName = $.trim(name);

        if (trimmedName == "") {
            dialog = GeoDialogFactory.InformationDialog("MustInputResourceName", "Порака!", "Мора да внесете име на ресурс!", 400, 200);
            dialog.Open();
            return false;
        }

        if (trimmedText == "") {
            dialog = GeoDialogFactory.InformationDialog("MustInputResourceText", "Порака!", "Мора да внесете текст на ресурс!", 400, 200);
            dialog.Open();
            return false;
        }

        return true;
    }

    // calcualtes the available area for the text editor in the 
    // create resource dialog.
    function calcualteTextAreaHeight() {
        var myDialogArea,
            header,
            height;

        myDialogArea = $("#ResourceCreateDialog");
        header = myDialogArea.find("h2");

        height = myDialogArea.height() - header.height();

        return height;
    }

    return {
        Initialize: init
    };
})();


