var GeoJQueryUiWrappers = (function () {
    // dialog options that will be passed to the function that creates the basic dialog
    // object that can then be opened or closed as necessary
    function dialogOptions(name, title, type, modal, resizable, contents, width, height, top, left, resetOnOpen) {
        // set name
        if (name)
            this.dialogName = name;
        else
            this.dialogName = "GenericUID";

        // set title 
        if (title)
            this.title = title;
        else
            this.title = "Dialog Title";

        // set title 
        if (type)
            this.type = type;
        else
            this.type = dialogTypes.Information;

        // set if the dialog is modal
        if (modal)
            this.modal = modal;
        else
            this.modal = false;

        // sets if the dialog is resizable
        if (resizable)
            this.resizable = resizable;
        else
            this.resizable = false;

        //set contents 
        if (contents)
            this.contents = contents;
        else
            this.contents = "";

        //set width
        if (width)
            this.width = width;
        else
            this.width = 350;

        // set height
        if (height)
            this.height = height;
        else
            this.height = 350;

        // set top position
        if (top)
            this.top = top;
        else
            this.top = "center";

        //set left poistion
        if (left)
            this.left = left;
        else
            this.left = "center";

        // set the option that tells us if the 
        // dialog will reset some properties on each open
        if (left)
            this.resetOnOpen = resetOnOpen;
        else
            this.resetOnOpen = false;

        return {
            Name: this.dialogName,
            Title: this.title,
            Type: this.type,
            Modal: this.modal,
            Resizable: this.resizeable,
            Contents: this.contents,
            Width: this.width,
            Height: this.height,
            Top: this.top,
            Left: this.left,
            ResetOnOpen: resetOnOpen
        };
    };

    // dialog open options to be passed to the dialog object when calling open.
    // ===== TO BE DECIDED IF IT WILL BE USED =====
    function dialogOpenOptions(top, left) {
        this.top = top;
        this.left = left;
    };

    // the main dialog object class
    function dialog(jDialog, options, reset) {
        this.jDialog = jDialog;
        this.reset = reset;
        this.options = options;

        var status = true;

        function open() {
            if (!status)
                return;
            // if reset there are some options to be reset.
            if (jDialog.dialog("isOpen")) {
                return;
            }

            if (reset) {
                jDialog.dialog("option", "position", [options.Left, options.Top]);
            }
            jDialog.dialog("open");
        };

        function close() {
            if (!status)
                return;
            jDialog.dialog("close");
        };

        function kill() {
            status = false;
            jDialog.remove();

        };

        function getDialog() {
            return jDialog;
        }

        function getDialogName(){
            var name = jDialog.attr('id');
            return name;

        }

        function saveDialog(jChangedDialog) {
            jDialog = jChangedDialog;
        }

        function setContents(newConents){
            jDialog.find(".informationDialogContent").html("");
            jDialog.find(".informationDialogContent").append(newConents);
            return this;
        }

        return {
            Open: open,
            Close: close,
            Kill: kill,
            Status: status,
            GetDialog : getDialog,
            SaveDialog : saveDialog,
            GetDialogName : getDialogName,
            SetContents : setContents
        };
    };

    // creates and returns a dialog object that can then be opened
    function createDialog(options) {
        // create a dialog using a div element 
        // but do not open it automatilcaly

        // acts as a wrapper outermost element of the dialog 
        var jDialog = $("<div></div>").addClass(".genericDialog");
        // set the name of the dialog.
        jDialog.attr("id", options.Name);

        jDialog.append(options.Contents);

        // create the jquery ui dialog using some of the basic options from the 
        //  options object
        jDialog.dialog({
            autoOpen: false
                , title: options.Title
                , height: options.Height
                , width: options.Width
                , resizable: options.Resizable
                , modal: options.Modal
                , position: [options.Left, options.Top]
        });

        // based on the dialog type passed in the options object create rest of dialog

        switch (options.Type) {
            case dialogTypes.Information:
                makeDialogInformation(jDialog);
                break;
            case dialogTypes.Error:
                makeDialogError(jDialog);
                break;
            case dialogTypes.Tutorial:
                makeDialogTutorial();
                break;
            case dialogTypes.Query:
                makeDialogQuery(jDialog);
                break;
            default:
        }

        return new dialog(jDialog, options, options.ResetOnOpen);
    };

    function makeDialogInformation(dialogObject) {
        dialogObject.dialog("option", "buttons", { "Затвори": function () { $(this).dialog("close"); } });
    };

    function makeDialogError(dialogObject) {
        dialogObject.dialog("option", "buttons", { "Затвори": function () { $(this).dialog("close"); } });
    };

    function makeDialogTutorial(dialogObject) {
        dialogObject.dialog("option", "buttons", { "Затвори": function () { $(this).dialog("close"); } });
    };

    function makeDialogQuery(dialogObject) {
        dialogObject.dialog("option", "buttons", { "Поднеси ": function () { $(this).dialog("close"); } });
    };

    var dialogTypes = {
        Information: "information",
        Error: "error",
        Tutorial: "tutorial",
        Query: "query"
    };

    return {
        DialogOptions: dialogOptions,
        DialogTypes: dialogTypes,
        CreateDialog: createDialog,
        Dialog: dialog
    };
})();