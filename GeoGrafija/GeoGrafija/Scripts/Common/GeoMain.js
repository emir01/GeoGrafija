/*======================================
    This is the Geo Modules main script that 
    calls and initializes common functionality across all pages.
*/

$(function () {
    GeoUi.Initialize();
    GeoDialogManager.Initialize();
    GeoMarkup.Initialize();
    GeoUtility.Initialize();
    GeoDialogFactory.Initialize();
});