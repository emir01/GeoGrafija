var GeoSingleDisplay = (function () {
    // The reference to the details map
    var map;

    // Data provider for  retrieving information/resources via ajax.
    // Muist be provided
    var searchDataProvider = null;

    // The main content window where the main information
    // Should be displayed
    var $content;

    // The hirearchy strip that displays the current locations hirearchy. 
    // Buit and retrieved with ajax after main info is displayed.
    var $strip;

    // The box with the name of the location
    var $name;

    // The find out more button
    var $findMore;

    //Default  wrapper selector ids 
    var strip = "locationHirearchy";
    var name = "locationNameBox";
    var content = "locationInfoContent";
    var findMore = "findOutMore";
    var showOnMap =  "showOnMapLink";

    // Default ids for the hidden elements on the details page,
    // for the various parameters ( coulour etc. etc.);
    var colorInputId = "typeColor";

    var hueChange = 15;

    function init(elements) {
        // Get main markup/layout elements 
        getMainElements(elements);

        // Setup the basic ui elements which can be 
        // calculated/designed by the parameters already asigned to the view via the view model.
        setupBasicUi();

        // Setup the map with the marker.
        setupMap();

        // Get all the extra stuff, like strip,resources etc.etc
        setupSugar();

        // Setup basic events
        setupEvents();

        if (GeoUtility.GetJqueryObjectById("searchDetailsWrapper")) {
            GeoResources.Initialize();
        }

        GeoUi.ResizeContent();
    };

    function setupEvents(){
        // get the find on map link and handle the event handler
        $("#findOnMapLink").on('click',handleShowLocationOnMap);
        $("#locationNameBox").on('click',function(){
            return false;
        });

        $("#locDetailsHelpButton").on('click',function(){
            // get the content and display it.
            var content = $("#LocationDetHelpWrapper");

            var clonedContent = content.clone();
            clonedContent.removeClass("hidden");

            var dialog = GeoDialogFactory.InformationDialog("SingleXZ2","Помош - Детали за локација", clonedContent, 600,600);
            GeoUtility.SetupHelpDialogContents(dialog.GetDialog());

            dialog.Open();

            clonedContent = null;
            return false;
        });

        $("#ResetDetailsMapButton").on('click', function(){
                var lat,lng,zoom;
                lat =  $("#Lat").val();
                lng =  $("#Lng").val();
                zoom = parseInt($("#Zoom").val());

                var latLng  = new google.maps.LatLng(lat,lng);

                map.setCenter(latLng);
                map.setZoom(zoom);

                return false;
        });
    }

    function handleShowLocationOnMap(event){
        alert("Show on map");
        return false;
    }

    function setupMap() {
        map = geografija.map.create.CreateMapD("mapDetails");
        var displayOpts = geografija.util.pullLocationDisplayOpts();
        var marker = geografija.util.setMap(map, displayOpts);
    };

    function setupBasicUi() {
        var color = GeoUtility.GetJqueryObjectById(colorInputId).val();
        $name.css("background-color", GeoUtility.ColorHexString(color));
        $name.attr("data-color", color);

        $name.hover(hoverInHuechange, hoverOutHuechange);
    };

    function hoverInHuechange(event) {
        var oldRgb = GeoUtility.GetRgbValuesForProperty("background-color", $(this));

        var newRgb = GeoUtility.ChangeColorHue(oldRgb, -hueChange);

        $(this).css("background-color", newRgb);
    };

    function hoverOutHuechange(event) {
        var oldRgb = GeoUtility.GetRgbValuesForProperty("background-color", $(this));

        var newRgb = GeoUtility.ChangeColorHue(oldRgb, hueChange);

        $(this).css("background-color", newRgb);
    };

    function setupSugar() {
        var id = $("#ID").val();
        searchDataProvider.GetSingleLocationDetails(id, getDetailsSuccess, getDetailsFail);

    };

    // The success hook for the success result on 
    // the  data provider get GetSingleLocationDetails call
    // Passes back, if everything is ok, a anonymous object with several propertis for the 
    // locaiton, which among other things includes the necesarry info to build the strip
    function getDetailsSuccess(infoDetails) {
        // Call setup strip passing from  info details  only the path
        setupStrip(infoDetails.Path);
    };

    // The fail hook for the success result on 
    // the  data provider get GetSingleLocationDetails call.
    // The call passess to the hook a anonymous object that can contain a messag as to what went wrong.
    // This message could then be ofcourse display as needed.
    function getDetailsFail(infoDetails) {
        // Display friendly message.

        if (GeoDialogManager.ContainsDialog("FailDetailsStrip") >= 0) {
            var dialogFromManager = GeoDialogManager.GetDialog("FailDetailsStrip");
            dialogFromManager.Open();
        }
        else {
            var dialogOptions = new GeoJQueryUiWrappers.DialogOptions();
            dialogOptions.Name = "FailDetailsStrip";
            dialogOptions.Width = 400;
            dialogOptions.Height = 200;
            dialogOptions.Resizable = false;
            dialogOptions.ResetOnOpen = true;
            dialogOptions.Title = "Грешка при преземање на податоци за лента";
            dialogOptions.Contents = $("<h3>Неуспешно преземање на податоците за креирање на лента</h3>").css({
                margin: "0px",
                padding: "0px"

            });

            dialogOptions.Modal = true;

            var dialog = GeoJQueryUiWrappers.CreateDialog(dialogOptions);
            GeoDialogManager.AddDialog(dialog);
            dialog.Open();
        }
    };

    // Sets up the strip of hirearchical locations that leads to the current location.
    // Should get passed an array of anonymous name,id objects to build the strip with.
    // Will probably be called by the setupSugar successFunciton
    function setupStrip(trail) {
        var $calcWrapper = GeoUtility.CheckPadding($strip);
        $calcWrapper.html("");

        var $breadcrumbsStrip = $("<ul></ul>").addClass("stripBreadcrumbs").css("color", "#FFFFFF");

        // add the first link in the details strip that should lead to the beginning
        var $firstLinkElement = $("<a></a>").html("Почеток");
        $firstLinkElement.addClass("plainLink");
        $firstLinkElement.attr("href", GeoUtility.GiveSearchLink({ "locationId": 0 }));
        $firstLinkElement.html("Почеток");
        var $firstStripElement = $("<li></li>").addClass("stripBreadcrumbsElement").append($firstLinkElement);
        $breadcrumbsStrip.append($firstStripElement);


        // for each bredcrumb add it to the strip
        $(trail).each(function (index, element) {
            if (index == $(trail).size() - 1) {
                return;
            }
            // Create the anchor element for each of the strip elements
            var $stripLinkElement = $("<a></a>").html(element.Name);

            $stripLinkElement.addClass("plainLink");
            $stripLinkElement.attr("href", GeoUtility.GiveSearchLink({ "locationId": element.Id }));
            $stripLinkElement.html(element.Name);

            // Create a strip elemenet for each of the locations returned
            // in the location hirearchy of the curently open location
            var $stripElement = $("<li></li>").addClass("stripBreadcrumbsElement").append($stripLinkElement);

            // Add the strop element to the breadcrumbs list/strip/hirearchy
            $breadcrumbsStrip.append($stripElement);
        });

        $calcWrapper.append($breadcrumbsStrip);
    };

    function getMainElements(elements) {
        // Get the strip element
        var goDefault = false;
        if (!elements) {
            goDefault = true;
        }
        if (!goDefault && elements.hasOwnProperty('strip')) {
            $strip = elements.strip;
        }
        else {
            $strip = $("#" + strip);
        }

        // Get the content element
        if (!goDefault && elements.hasOwnProperty('content')) {
            $content = elements.content;
        }
        else {
            $content = $("#" + content);
        }

        // Get the find more element
        if (!goDefault && elements.hasOwnProperty('findMore')) {
            $findMore = elements.findMore;
        }
        else {
            $findMore = $("#" + findMore);
        }

        // Get the name more element
        if (!goDefault && elements.hasOwnProperty('name')) {
            $name = elements.name;
        }
        else {
            $name = $("#" + name);
        }
    };

    function setDataProvider(dataProvider) {
        searchDataProvider = dataProvider;
    };

    return {
        Initialize: init,
        SetDataProvider: setDataProvider
    };
})();
