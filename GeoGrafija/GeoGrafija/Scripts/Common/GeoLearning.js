// Handles the client side part of learning module
var GeoLearning = (function () {
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    // ===   Learning  module Parameters, Constants, And Configuration Values  ---------------------------------------------------------------------------
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    var selectedLocationTypeId;
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    // ===   Learning  module functions and methods  -----------------------------------------------------------------------------------------------------
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------

    function initialize() {
        setupUi();
        setupEvents();

        GeoResources.Initialize();
    }

    function setupUi() {
        $(".resourcePerLocationTypeWrapper").hide();
        $(".pickExplanation").hide();
        $("#LearnMoreLocations").hide();

        $("#LocationTypeDropdown").dropkick({
            change: onLocationTypeTypeChange
        });

        //check for already selected location type to display
        var selectedLocaitonType = $("#LocationTypeDropdown").closest("div").find(".dk_options").find("li.dk_option_current a").attr("data-dk-dropdown-value");

        if (selectedLocaitonType && selectedLocaitonType != "") {
            $(".resourcePerLocationTypeWrapper").hide();

            $("#HiddenSelectedTypeId").val(selectedLocaitonType);

            $(".pickExplanation").show();
            $("#LearnMoreLocations").show();
            GeoDisplay.PerformDisplay();

            $(".resourcePerLocationTypeWrapper").filter(function () {
                return $(this).attr("data-location-type-id") == selectedLocaitonType;
            }).slideDown();
        }
    }

    function setupEvents() {
    }

    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    // ===   Learning  module event handlers  -----------------------------------------------------------------------------------------------------
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------

    function onLocationTypeTypeChange(value, label) {
        $(".resourcePerLocationTypeWrapper").hide();

        $(".resourcePerLocationTypeWrapper").filter(function () {
            return $(this).attr("data-location-type-id") == value;
        }).slideDown();


        if (value != "") {
            $(".pickExplanation").show();
            $("#LearnMoreLocations").show();
            $("#HiddenSelectedTypeId").val(value);
            GeoDisplay.PerformDisplay();
        }
        else {
            $(".pickExplanation").hide();
            $("#LearnMoreLocations").hide();
            $("#HiddenSelectedTypeId").val("");
        }
    }

    return {
        Initialize: initialize
    };
})();
