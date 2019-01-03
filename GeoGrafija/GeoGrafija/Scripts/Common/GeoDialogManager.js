var GeoDialogManager = (function () {
    var dialogs;

    function init() {
        dialogs = new Array();
    };

    function addDialog(dialog) {
        var dialogIndex = containsDialog(dialog.GetDialogName());
        if (dialogIndex < 0) {
            dialogs.push(dialog);
        }
    };

    function removeDialog(dialog) {
        var dialogIndex = containsDialog(dialog);
        if (dialogIndex >= 0) {
            var removedDialog = dialogs.splice(dialogIndex, 1);
            removedDialog.Kill();
        }
    };

    function getDialog(dialog) {
        var dialogIndex = containsDialog(dialog);
        if (dialogIndex >= 0) {
            return dialogs[dialogIndex];
        }
        else {
            return null;
        }
    };

    //checks if the dialog manager contains a dialog with the given name and returns 
    // its position. Returns -1 if it does not contain the element
    function containsDialog(dialogName) {
        var contains = -1;

        for (var i = 0; i < dialogs.length; i++) {
            var savedDialog = dialogs[i];
            var dialogNameFromSaved = savedDialog.GetDialogName();
            if (dialogNameFromSaved == dialogName) {
                contains = i;
            }
        }

        return contains;
    };

    return {
        Initialize: init,
        AddDialog: addDialog,
        RemoveDialog: removeDialog,
        GetDialog: getDialog,
        ContainsDialog: containsDialog
    };

})();
