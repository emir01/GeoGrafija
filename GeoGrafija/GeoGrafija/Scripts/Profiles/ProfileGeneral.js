// ===   ------------------------------------------------------------------
// ===   Sub Module that handles General functionality for user profile pages
// ===   ------------------------------------------------------------------
var ProfileGeneral = (function () {

    // ===   ------------------------------------------------------------------
    // ===   General parameters and configuration values
    // ===   ------------------------------------------------------------------
    var currentProfile;
    // ===   ------------------------------------------------------------------
    // ===   functions and methods
    // ===   ------------------------------------------------------------------

    function setCurrentProfile(profile) {
        currentProfile = profile;
    }

    function init() {
        setupCanvases();
        setUpEvents();
    }

    function setUpEvents() {
        //setup click events on action boxes
        $(".profileActionBoxItem").click(actionBoxClick);
    }

    function setupCanvases() {
        $(".profileCanvasElement").filter(function () {
            return $(this).attr("data-uid") != 1;
        }).hide();

        $(".profileActionBoxItem").filter(function () {
            return $(this).attr("data-uid") == 1;
        }).removeClass("green").addClass("red");
    }

    // ===   ------------------------------------------------------------------
    // ===   Event Handlers
    // ===   ------------------------------------------------------------------

    function actionBoxClick(event) {
        //check if uid diferent than zero
        var uid = $(this).attr("data-uid"),
            actionId = $(this).attr("data-actionid");

        if (uid == 0) {
            handleNonChangeElement(actionId);
        }
        else {
            changeCanvas(uid);
        }
    }

    // ===   ------------------------------------------------------------------
    // ===   Utilities
    // ===   ------------------------------------------------------------------

    function changeCanvas(uid) {
        //hide all of them
        $(".profileCanvasElement").hide();
        $(".profileActionBoxItem").removeClass("red").addClass("green");

        $(".profileCanvasElement").filter(function () {
            return $(this).attr("data-uid") == uid;
        }).show();

        $(".profileActionBoxItem").filter(function () {
            return $(this).attr("data-uid") == uid;
        }).removeClass("green").addClass("red");

    }

    function handleNonChangeElement(actionId) {
        currentProfile.HandleAction(actionId);
    }

    return {
        Initialize: init,
        SetCurrentProfile: setCurrentProfile
    };
})();

