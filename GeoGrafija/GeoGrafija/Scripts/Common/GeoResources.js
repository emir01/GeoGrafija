// Handles the displaying of location resources in a interactive manner directly into the one page of locations... 
// Do this by fading out the map and  the text markup....
var GeoResources = (function () {

    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    // ===   Resource  module Parameters, Constants, And Configuration Values  ------------------------------------------------------------------------------------
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    var mapResource = {
        Text: 1,
        Link: 2,
        Animation: 3,
        Video: 4

    };
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    // ===   Resource  module functions and methods  --------------------------------------------------------------------------------------------------------------
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------

    function init() {
        setupAuxMarkup();
        setupBasicEventHandles();
    };

    // ===                                      ------------------------------------------------------------------------------------------------------
    // ===  Main Funcitons and Event Handlers   ------------------------------------------------------------------------------------------------------
    // ===                                      ------------------------------------------------------------------------------------------------------

    function setupAuxMarkup() {

        $(".resourceSeciton").each(function (inde, element) {
            var jwrapper = $(this);
            var options = GeoMarkup.ResourceMarkup.Options;
            jwrapper.css("width", options.SECTION_WIDTH);
            jwrapper.css("margin", options.SECTION_MARGIN);
            jwrapper.css("margin-left", options.SECTION_LEFT_MARGIN);
            jwrapper.css("margin-right", options.SECTION_RIGHT_MARGIN);
        });

        $(".resourceSectionHeadingBox .padding").each(function (index, element) {
            $(this).css("background-color", GeoUtility.ColorHexString($(this).attr("data-color")));
        });


        $("#ResourceUiLocationBox").css("background-color", $("#locationNameBox").css("background-color"));

        GeoUi.ResizeSections(3, 50);
    }

    //[PRIVATE] sets up basic event handles on links and buttons , mostly to do with switching between the views
    function setupBasicEventHandles() {
        // Shows the resources 
        $("#findoutMoreLink").click(showResources);

        // Come back to details for location
        $("#ResourceUiLocationBox a").click(showMap);

        // Come back to details for location.
        $("#BackToLocaitonDetailsLink").click(showMap);

        var listItems = $(".resourceSeciton li");

        listItems.click(displayResource);
    }

    function showResources(event) {
        var resource,
            map;

        map = $("#searchDetailsWrapper");
        resource = $("#resourceContainer");


        resource.show();
        map.hide();

        GeoDisplay.PerformDisplay();

        return false;
    }

    function showMap(event) {
        var resource,
            map;

        map = $("#searchDetailsWrapper");
        resource = $("#resourceContainer");

        map.show();
        resource.hide();

        return false;
    }

    function displayResource(event) {
        // get the resource type
        var resourceId,
            resourceTypeId,
            listItem;

        listItem = $(this);
        resourceId = listItem.attr("data-id");
        resourceTypeId = listItem.attr("data-type-id");
        var callback = null;

        // decite on the callback to be called when we get the 
        // resource data with ajax. Callback decided base on resource type.
        switch (parseInt(resourceTypeId)) {
            case mapResource.Animation:
                callback = displayAnimation;
                break;

            case mapResource.Text:
                callback = displayText;
                break;

            case mapResource.Video:
                callback = displayVideo;
                break;

            case mapResource.Link:
                callback = displayLink;
                break;

            default:
                callback = null;
        }

        makeAjaxCallForResource(resourceId, callback);
    }

    function makeAjaxCallForResource(resourceId, displayCallbak) {
        var action = "GetResourceDetails",
            controller = "Search",
            url,
            jsonData;

        url = GeoAjax.GetUrlForAction(controller, action);
        jsonData = GeoAjax.SimpleAjaxParam(resourceId, "resourceId");

        GeoAjax.MakeAjaxPost(url, jsonData, displayCallbak, failToGetResourceDetails);
    }

    // ===              ------------------------------------------------------------------------------------------------------
    // === Utilities    ------------------------------------------------------------------------------------------------------
    // ===              ------------------------------------------------------------------------------------------------------

    function failToGetResourceDetails(object, status, thrown) {
        var dialog;

        dialog = GeoDialogFactory.InformationDialog("FailedToGetResourceDetailsLocationSearch", "Порака!", "Неуспешен приказ на ресурс! Обиди се повторно!", 400, 200);
        dialog.Open();
    }

    function resourceDisplayError() {
        resourceDisplayError();
    }

    function displayText(data, status, object) {
        if (!data.IsOk) {
            resourceDisplayError();
            return;
        }

        //get the data from  the view model gotten from the server
        var text, name;
        text = data.Text;
        name = data.Name;

        // create the dialog.
        var dialogWrapper = $("#TextResourceDisplay");
        var textWrapper = $("#TextResourceMainDisplay");

        textWrapper.html(text);
        
        // create the dialog
        dialogWrapper.dialog({
            autoOpen: false,
            width: 600,
            height: 600,
            title: name,
            resizable: false,
            modal: true,
            buttons: {
                "Затвори ": function () {
                    $(this).dialog("close");
                }
            },
            open: function (event, ui) {

            },
            close: function () {

            }
        });

        dialogWrapper.dialog("open");
    }

    function displayVideo(data, status, object) {
        if (!data.IsOk) {
            resourceDisplayError();
            return;
        }

        //get the needed data:
        var name = data.Name;
        var url = data.Text;

        // get markup info
        var wrapper = $("#VideoResourceDisplay");
        var iframeWrapper = $("#VideoFrameWrapper");

        var playerWidth = 520;
        var playerHeight = 450;
        var player = $(url.replace(/(?:http:\/\/)?(?:www\.)?(?:youtube\.com|youtu\.be)\/(?:watch\?v=)?(.+)/g, '<iframe width="' + playerWidth + '" height="' + playerHeight + '" src="http://www.youtube.com/embed/$1" frameborder="0" allowfullscreen></iframe>'));

        iframeWrapper.html(player);

        wrapper.dialog({
            width: 600,
            height: 600,
            title: name,
            modal: true,
            resizable: false,
            autoOpen: false,
            buttons: {
                "Затвори ": function () {
                    $(this).dialog("close");
                }
            },
            open: function (event, ui) {

            },
            close: function () {
                $("#VideoFrameWrapper").html("");
                $('#TextResourceMainDisplay').wysiwyg('clear');
            }
        });

        wrapper.dialog("open");
    }

    function displayLink(data, status, object) {
        if (!data.IsOk) {
            resourceDisplayError();
            return;
        }

        //data 
        var text = data.Text;
        var name = data.Name;

        //markup
        var wrapper = $("#LinkResourceDisplay");
        var urlContainer = $("#LinkText");

        var decodeText =decodeURI(text);
        var displayText  = GeoUtility.Shorten(decodeText,65,'.');
        urlContainer.html(displayText);

        wrapper.dialog({
            width: 500,
            height: 250,
            title: name,
            modal: true,
            resizable: false,
            autoOpen: false,
            buttons: {
                "Затвори ": function () {
                    $(this).dialog("close");
                },
                "Отвори Линк": function () {
                    $(this).dialog("close");
                    window.location = text;
                }
            },
            open: function (event, ui) {

            },
            close: function () {
                urlContainer.html("");
            }
        });

        wrapper.dialog("open");
    }

    function displayAnimation(data, status, object) {
        if (!data.IsOk) {
            resourceDisplayError();
            return;
        }
    }

    // ===                      ------------------------------------------------------------------------------------------------------
    // === Markup Generation    ------------------------------------------------------------------------------------------------------
    // ===                      ------------------------------------------------------------------------------------------------------
    return {
        Initialize: init
    };
})();
;