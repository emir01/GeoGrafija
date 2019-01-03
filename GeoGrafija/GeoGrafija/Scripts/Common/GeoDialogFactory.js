// ===   ------------------------------------------------------------------
// ===   Module that handles creation of general dialogs
// ===   ------------------------------------------------------------------
var GeoDialogFactory = (function () {
    // ===   ------------------------------------------------------------------
    // ===   Dialog Factory settings, parameters, and configuration values
    // ===   ------------------------------------------------------------------

    var classNames = {
        Information: "informationDialogContent",
        Error: "errorDialogContent",
        Tutorial: "tutorialDialogContent"
    };

    // ===   ------------------------------------------------------------------
    // ===   Dialog Factory functions and methods
    // ===   ------------------------------------------------------------------
    function init() {

    };

    function getGeneralActionDialog(options) {
        var name,
            title,
            contents,
            width,
            height,
            action1Text,
            action2Text,
            action1,
            action2,
            dialog,
            dialogOptions,
            jDialog,
            buttons,
            onClose;

        // setup values 
        name = options.name || "DefaultName";
        title = options.title || "Активност";
        contents = options.contents || "";
        width = options.width || "400";
        height = options.height || "200";

        action1Text = options.action1Text || "Ок";
        action1 = options.action1;

        action2 = options.action2;
        action2Text = options.action2Text || "Затвори";

        onClose = options.onClose;

        if (!action1) {
            return null;
        }

        dialogOptions = new GeoJQueryUiWrappers.DialogOptions();
        setGenericOptions(dialogOptions);
        setSpecificOptions(dialogOptions, name, title, contents, width, height);
        setDialogContents(dialogOptions, "quizResultWrapper", contents);

        if (dialog = checkDialogManager(name)) {
            return dialog;
        }

        dialog = GeoJQueryUiWrappers.CreateDialog(dialogOptions);

        //enhance the dialog  adding the proper buttons and actions
        jDialog = dialog.GetDialog();

        buttons = {};
        if (typeof action1 == "function") {
            buttons[action1Text] = action1;
        }
        else {
            buttons[action1Text] = function () {
                $(this).dialog("close");
            };
        }

        if (typeof action2 == "function") {
            buttons[action2Text] = action2;
        }
        else {

        }

        $(jDialog).dialog("option", "buttons", buttons);

        // setup on close event
        if(onClose !== undefined){
            jDialog.bind("dialogclose",onClose);
        }

        dialog.SaveDialog($(jDialog));

        saveDialog(dialog);
        return dialog;
    }

    // returns a dialog that is used to display information messages
    function generalInformationDialog(name, title, message, width, height) {
        var dialog,
            dialogOptions;

        if (GeoDialogManager.ContainsDialog(name) >= 0) {
            dialog = GeoDialogManager.GetDialog(name);
            return dialog;
        }
        
        dialogOptions = new GeoJQueryUiWrappers.DialogOptions();

        //set up options
        setGenericOptions(dialogOptions);
        setSpecificOptions(dialogOptions, name, title, message, width, height);

        //set up contents
        setDialogContents(dialogOptions, classNames.Information, message);

        dialogOptions.Type = GeoJQueryUiWrappers.DialogTypes.Information;
        dialog = GeoJQueryUiWrappers.CreateDialog(dialogOptions);

        // save the dialog in the dialog manaer so 
        // we wont have to create it the next call here.
        saveDialog(dialog);
        return dialog;
    };

    // returns a dialog that is used to display an error message
    function generalErrorDialog(name, title, message, width, height) {
        var dialog,
            dialogOptions;

        // check if the dialog has already been created
        if (dialog = checkDialogManager(name)) {
            return dialog;
        }

        dialogOptions = new GeoJQueryUiWrappers.DialogOptions();

        //set up options
        setGenericOptions(dialogOptions);
        setSpecificOptions(dialogOptions, name, title, message, width, height);

        //set up contents
        setDialogContents(dialogOptions, classNames.Error, message);

        dialogOptions.Type = GeoJQueryUiWrappers.DialogTypes.Error;
        dialog = GeoJQueryUiWrappers.CreateDialog(dialogOptions);

        saveDialog(dialog);
        return dialog;
    };

    // returns a dialog that  is used to display tutorials about specific parts of the system.
    function generalTutorialDialog(name, title, message, width, height) {
        var dialogOptions,
            dialog;

        // check if the dialog has already been created
        if (dialog = checkDialogManager(name)) {
            return dialog;
        }

        dialogOptions = new GeoJQueryUiWrappers.DialogOptions();

        //set up options
        setGenericOptions(dialogOptions);
        setSpecificOptions(dialogOptions, name, title, message, width, height);

        //set up contents
        setDialogContents(dialogOptions, classNames.Tutorial, message);

        dialogOptions.Type = GeoJQueryUiWrappers.DialogTypes.Tutorial;
        dialog = GeoJQueryUiWrappers.CreateDialog(dialogOptions);

        saveDialog(dialog);
        return dialog;
    };

    // sets generic options that dont depend on supplied values for the dialog
    function setGenericOptions(dialogOptions) {
        dialogOptions.Resizable = false;
        dialogOptions.ResetOnOpen = true;
        dialogOptions.Modal = true;
    };

    // set the specific options that are passed to one of the dialog creation functions
    function setSpecificOptions(dialogOptions, name, title, message, width, height) {
        dialogOptions.Width = width;
        dialogOptions.Height = height;
        dialogOptions.Name = name;
        dialogOptions.Title = title;

    };

    function setDialogContents(dialogOptions, cssClass, message) {
        var jContent = GeoMarkup.Utility.Wrapper();
        jContent.addClass(cssClass);

        var jpContent = GeoMarkup.Utility.Padding();
        jContent.append(jpContent);
        var jCalcContent = GeoUtility.CheckPadding(jContent);

        jCalcContent.html(message);

        dialogOptions.Contents = jContent;
    };


    function checkDialogManager(name) {
        var dialogIndex,
            dialog;

        dialogIndex = GeoDialogManager.ContainsDialog(name);
        if (dialogIndex >= 0) {
            dialog = GeoDialogManager.GetDialog(dialogIndex);
            return dialog;
        }
        else {
            return false;
        }
    }

    function saveDialog(dialog) {
        GeoDialogManager.AddDialog(dialog);
    }

    return {
        Initialize: init,
        InformationDialog: generalInformationDialog,
        ErrorDialog: generalErrorDialog,
        TutorialDiaog: generalTutorialDialog,
        ActionDialog: getGeneralActionDialog
    };
})();