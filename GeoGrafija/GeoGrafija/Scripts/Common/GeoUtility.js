var GeoUtility = (function () {

    // ===   ------------------------------------------------------------------
    // ===   GeoUtility constans, configuration values, and parameters
    // ===   ------------------------------------------------------------------
    var freezeTimeOut = 5000;
    var defaultHueChange = 10;
    var loggingEnabled = true;
    var HelpDialogHeaderClickUtility = true;

    // ===   ------------------------------------------------------------------
    // ===   GeoUtility functions and methods
    // ===   ------------------------------------------------------------------

    function init() {
        animation.Initialize();
    };

    // Extract  value from the query string for an identifier with a given name.
    function getParameterByName(name) {
        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regexS = "[\\?&]" + name + "=([^&#]*)";
        var regex = new RegExp(regexS);
        var results = regex.exec(window.location.href);
        if (results == null)
            return "";
        else
            return decodeURIComponent(results[1].replace(/\+/g, " "));
    };

    //checks if a wrapper element has padding. 
    // if there is no element that has the class padding that is used to pad elements inside the wrapper
    // it returns the wrapper.
    // ------
    // if there is an element that is the first child and has the class padding that element is returned.
    // any further operations could then be done on the padding element ( insertion of content etc etc ) 
    function checkPadding($element) {
        var numberOfChildren = $element.children().size();


        // if there are no children or the number of children is above one then the element must be returned
        if ($element.children().size() <= 0 || numberOfChildren > 1)
            return $element;

        // if there is only one child then we need to check if it is the padding child
        var $child = $element.children().first();

        // if the child has the padding class then return the child else return the element
        if ($child.hasClass("padding")) {
            return $child;
        }
        else {
            return $element;
        }
    };

    function giveLink(parameters) {
        var options;

        options = extractGiveSearchLinkParameters(parameters);
        var url = "/Search";

        // if the locationId is extracted add as a query sting to the url to be returned
        if (options.locationId != "") {
            url += "?parentId=" + options.locationId;
        }
        else { // else do nothing and just point to the regular SearchPage

        }

        return url;
    };

    function extractGiveSearchLinkParameters(parameters) {
        // the object returned to the calling function that has
        // the extracted values from parameters or the default values if parameters is 
        // empty or has wrong key values
        var options = {};

        if (parameters.hasOwnProperty("locationId")) {
            options.locationId = parameters.locationId;
        } // else provide the default value
        else {
            options.locationId = "";
        }

        return options;
    };

    function giveSearchHelpMarkup() {
        var jLink = $("<a>помош ?</a>").addClass("locationSearchHelpLink button green");
        jLink.attr("href", "#");
        jLink.click(searchHelpClick);

        return jLink;
    };

    function searchHelpClick(event) {
        //create  basic dialog . This can be moved to a seperate utility
        //function that creates a basic dialog, to which content can be inserted and then displayed
        var dialog;
        if (GeoDialogManager.ContainsDialog("SearchHelp") >= 0) {
            dialog = GeoDialogManager.GetDialog("SearchHelp");
            
            dialog.Open();
            return false;
        }
        else {
            dialog = GeoDialogFactory.InformationDialog("SearchHelp", "Помош - Пребарување!", getSearchHelpContent(), 600, 600);
            setupHelpDialogContents(dialog.GetDialog());
            GeoDialogManager.AddDialog(dialog);
            dialog.Open();
            return false;
        }



        return false;
    };

    function getSearchHelpContent() {
        var content = $("#SearchHelpWrapper");

        var clonedContent = content.clone();
        clonedContent.removeClass("hidden");
        
        return clonedContent;
    };

    function getJqueryObjectById(id) {
        var calcId;
        var idSub = $.trim(id).substr(0, 1);

        if (idSub != "#")
            calcId = "#" + id;
        else
            calcId = id;

        var jObjectArray = $(calcId);

        if (jObjectArray.size() > 0) {
            return jObjectArray.first();
        }
        return null;
    };

    function currentPageIs(pageRegistryValue) {
        var url = getCurrentUrl();

    };

    function getCurrentUrl() {
        var pathname = window.location.pathname;
        return pathname;
    };

    var pageUrlRegistry = {

    };

    // freezes the screen so nothing is clickable
    // used when waiting for animations if needed.
    function freeze() {
        if ($(".freeze").size() > 0) {
            return;
        }

        var mF = $("<div></div>").addClass("freeze");

        setTimeout("GeoUtility.UnFreeze()", freezeTimeOut);
        $("body").append(mF);

    };

    function autoUnFreeze() {
        unFreeze();
    };

    // does the oposite of freeze
    function unFreeze() {
        if ($(".freeze").size() == 0) {
            return;
        }

        $(".freeze").remove();
    };

    // checks and if necesary  converts the passed in string to a nice hex string color representation
    function colorFromHexString(string) {
        var sub = $.trim(string).substr(0, 1);

        if (sub == "#") {
            return string;
        }
        else {
            return "#" + string;
        }
    };

    // based on a old rgb values object returns a new rgb object with the values slighty changed by a constant amount
    function changeColorHue(oldRgb, hueChangeValue) {
        var calcHueChange = GeoUtility.CalculateValue(hueChangeValue, defaultHueChange);

        var newRgbObejct = {};
        newRgbObejct.R = "" + (parseInt(oldRgb.R) + calcHueChange);
        newRgbObejct.G = "" + (parseInt(oldRgb.G) + calcHueChange);
        newRgbObejct.B = "" + (parseInt(oldRgb.B) + calcHueChange);

        var newRgb = "rgb(" + newRgbObejct.R + ", " + newRgbObejct.G + ", " + newRgbObejct.B + ")";

        return newRgb;
    };

    // for a given  property  returns a  rgb value object if that property has the value of rgb object
    function getRgbValuesForProperty(property, jElement) {
        var color = $(jElement).css(property);
        var onlyValues = color.substring(4, color.length - 1);

        var r = $.trim(onlyValues.split(',')[0]);
        var g = $.trim(onlyValues.split(',')[1]);
        var b = $.trim(onlyValues.split(',')[2]);

        return { R: r, G: g, B: b };
    };

    // Can be used to calculate which value to use given a default value.
    // The test value can be a  function parameter value which might or might not be provided
    function calculateValue(testValue, defaultValue) {
        if (testValue) {
            return testValue;
        }
        else {
            return defaultValue;
        }
    };

    //function that returns an url with one query string param
    function getUrlWithParam(controller, action, queryParamName, queryParamValue) {
        ///User/LogOn -- example
        return "" + controller + "/" + action + "?" + queryParamName + "=" + queryParamValue;
    }

    function shorten(string, length, finalizer) {
        if (string.length < length) {
            return string;
        }
        else {
            var substring = string.toString().substr(0, length - 3);
            var finalizerString = finalizer.toString();
            var finalized = substring.concat(finalizerString).concat(finalizerString).concat(finalizerString);
            return finalized;
        }
    }

    // ===   ------------------------------------------------------------------
    // ===   Utility Module that handes animation
    // ===   Does not handle multiple animation calls
    // ===   ------------------------------------------------------------------

    var animation = (function () {
        // ===   ------------------------------------------------------------------
        // ===   Animation utility module parameters configurations and stored values
        // ===   ------------------------------------------------------------------

        // ---- variables

        var constAnimDuration;
        var constSkipTimeDuration;

        // ---- callback functions

        // the callback from the entry point to the module
        var moduleGlobalCallback;

        // ===   ------------------------------------------------------------------
        // ===   Animation utility module functions and methods
        // ===   ------------------------------------------------------------------

        function init() {

        };

        /* Sequentialy fades in elements.
        // Parameters : 
        elements - jquery array of elements that will be sequentialy faded in
        duration - duration each element will take to fade in
        finished - the callback function that will be called once the elements have finished all fading in
        timeSkip - to be decided. Used as time between one element finish apearing and the next epement start fading in
        */
        function fadeInSequential(elements, duration, finished, timeSkip) {
            //save parameter values and callbacks
            constAnimDuration = duration;
            constSkipTimeDuration = timeSkip;
            moduleGlobalCallback = finished;

            fadeInElement(0, elements);
        };

        function fadeInElement(number, elements) {
            // check if done animation
            if (number == elements.size()) {
                animationFinished();
                return;
            }

            var element = elements.get(number);
            $(element).fadeIn(constAnimDuration, function () { fadeInElement(number + 1, elements); });
        };

        function animationFinished() {
            //release resources
            constAnimDuration = null;
            constSkipTimeDuration = null;

            // call global module callback
            moduleGlobalCallback();
    }

        return {
            Initialize: init,
            FadeInSequential: fadeInSequential
        };
    })();

    // ===   ------------------------------------------------------------------
    // ===   Messages Object that contains commonly used information and/or error messages
    // ===   ------------------------------------------------------------------

    var messages = {
        PickLocationMarker: "Мора да изберете локација на мапата!"
    };

function logToFirebugConsole(msg){
        if(loggingEnabled){
            console.log(msg);
        }
    }
   
    function setupHelpDialogContents(where){
        var contents =  $(".dialogExplanationContentsList",where);
        var contentItem = contents.find("a");
        
        // Setup the on click event for each of the contet items
        contentItem.off('click');
        contentItem.on('click', scrollToItem);

        // on click event for the back to content anchors
        $(".dialogExplanationScrollTop",where).off("click");
        var scrollToTops = $(".dialogExplanationScrollTop",where).on('click',function(){
            var dialogContents  = $(this).closest(".ui-dialog-content");
            dialogContents.scrollTop(0);
            return false;
        });

        if(HelpDialogHeaderClickUtility){
            //   Tmp Header Helpers
            var headers  =  $(".explanationSectionHeader",where);
            headers.off('click');

            headers.on('click',function(){
                var dialogContents  = $(this).closest(".ui-dialog-content");
                alert(dialogContents.scrollTop());
            })
        }

    }

    function scrollToItem() {
        var anchor  = $(this);
        var dialogContents  = anchor.closest(".ui-dialog-content");

        var scroll = anchor.attr("scroll");

        dialogContents.scrollTop(scroll);

        return false;
    }

    return {
        Initialize: init,
        GetJqueryObjectById: getJqueryObjectById,
        GetParamByName: getParameterByName,
        CheckPadding: checkPadding,

        GiveSearchLink: giveLink,

        GiveSearchHelpMarkup: giveSearchHelpMarkup,
        CurrentPageIs: currentPageIs,

        GetCurrentUrl: getCurrentUrl,
        PageUrlRegistry: pageUrlRegistry,

        Freeze: freeze,
        UnFreeze: unFreeze,

        ColorHexString: colorFromHexString,
        ChangeColorHue: changeColorHue,
        CalculateValue: calculateValue,
        GetRgbValuesForProperty: getRgbValuesForProperty,

        Animation: animation,

        Messages: messages,

        GetUrlWithParam: getUrlWithParam,
        Shorten:shorten,

        log:logToFirebugConsole,

        SetupHelpDialogContents: setupHelpDialogContents
    };
})();