// Handles the interactive map exploration for locations.
var GeoExploration = (function () {
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    // ===   Map Exploration module Parameters, Constants, And Configuration Values  ------------------------------------------------------------------------------------
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    var allMarkers = new Array();
    var map;
    var firstDraw = true;

    var zoomTolerance = 1;

    var exploreSearch = false;
    var resetSearch = false;
    
    // To handle the bug with single results from explore
    var singleResultSearch = false;
    var zoomCounterSingleResult = 1;

    var infowindow = new window.google.maps.InfoWindow();
    
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    // ===   Map Exploration Main Functions and Methods  ------------------------------------------------------------------------------------
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    function initialize() {
        $('#MarkersScrollbar').tinyscrollbar();
        configureLayout();
        GeoUi.ResizeExplorationMap();

        $("body").on("click", ".zoomLink", zoomHandler);
        $("body").on("click", ".detailsLink", detailsHandler);
        $("body").on("click", ".closeLink", closeHandler);

        $("#ResetMap").on("click", resetMap);
        $("#MapNav").on("click", ".navOption", navigateLocOnMap);
        $("#ExplorationHelp").on('click',ShowExplorationHelp)

        map = geografija.map.create.CreateMapD("ExplorationMap");
        infowindow = new window.google.maps.InfoWindow();

        GeoSearch.Initialize("Exploration");
        GeoSearch.SetSuccessHook(searchSuccessHook);
        GeoSearch.SetFailHook(searchFailHook);

        $("#ExplorationMap").mask("Се Вчитува...");

        setTimeout(function () {
            var found = checkForQueryStringParams();

            if(found === true){
                GeoUtility.log("Found Query Parameter");
                setupMarkerEvents();
                window.google.maps.event.addListener(map, 'zoom_changed', mapZoomChanged);
            }
            else{
                GeoUtility.log("Have not found query paremeter");
                waitLoadStart();
            }

        }, 3000);


        //Setup the showing of the marker legend
        $("#MarkersDisplayLink a").on('click',function(){
            var legend = $("#MarkerDisplayWrapper")
            
            legend.animate({
                right:10
            },200,function(){
                $('#MarkersScrollbar').tinyscrollbar();
            });

            return false;
        });

        //setup the hiding of the marker legend
        $("#HideMarkerLegend a").on('click',function(){
            var legend = $("#MarkerDisplayWrapper")
            
            legend.animate({
                right:-350
            },200);

            return false;
        });
     }

    function checkForQueryStringParams(){
        
        var lid = parseInt(GeoUtility.GetParamByName("lid"));
        
        if(typeof(lid) === 'number' && lid !== 0 && !isNaN(lid)){
            showLocationOnMap(lid);
            return true;
        }
        return false;
    }

    // SINGLE LOCATION ==================================================================
    // Displays a single location on the map given the location id
    // Also get the path for the location displayed in the top map options corner.
    // SINGLE LOCATION ==================================================================
    function showLocationOnMap(lid){
        var searchOptions = {};
        searchOptions["locationId"] = lid;

        exploreSearch = true;
        firstDraw = false;
        GeoSearch.GetLocations(searchOptions);

        // Draw the hirearchy for the location.
        var  controller = "Search";
        var action = "LocationInfo";
        var jsonData=  GeoAjax.SimpleAjaxParam(lid,"locationId");
        var url = GeoAjax.GetUrlForAction(controller,action);

        GeoAjax.MakeAjaxPost(
            url,
            jsonData,
            function (data, status, object){
                GeoUtility.log("Success Retrive hirearchy for object");
                
                if(data && data != null && data.Path.length >0){
                    var paths  =  data.Path;
                    var pathsLength = paths.length;
                    
                    $.each(paths,function(index,element){
                       var id = element.Id;
                       var name = element.Name;
                       $('<li><a class="mapOption button blue navOption" data-id="' + id + '" href="#">' + name + '</a></li>').appendTo("#MapNav");
                    });
                }
                else{
                    return;
                }
            },
            function (object,status,thrown){
              GeoUtility.log("Error retriveving hirearcy for object");  
            });
    }

    function waitLoadStart() {
        GeoSearch.GetLocations();
    }

    function configureLayout() {
        $("#Main").addClass("resizeMe");
        var mainPadding = $("#Main").children(".padding");
        
        mainPadding.css("position","absolute");
        mainPadding.css("top","0px");
        mainPadding.css("right","0px");
        mainPadding.css("left","0px");
        mainPadding.css("bottom","0px");
        mainPadding.css("padding-top","5px");
    }

    function exploreMarker(marker) {
        var actual = marker.getActual();
        map.setCenter(actual.getPosition());

        var locaitonId = marker.id;

        $('<li><a class="mapOption button blue navOption" data-id="' + marker.data.Id + '" href="#">' + marker.data.Name + '</a></li>').appendTo("#MapNav");

        var searchOptions = {};
        searchOptions["parentId"] = locaitonId;

        exploreSearch = true;
        GeoSearch.GetLocations(searchOptions);
    }
    
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    // ===   Map Exploration Event handlres  ------------------------------------------------------------------------------------
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    function ShowExplorationHelp(event){
        var content  =  $("#ExplorationHelpWrapper");

        var clonedContent  = content.clone();
        clonedContent.removeClass("hidden");

        GeoUtility.SetupHelpDialogContents(clonedContent);

        var dialog = GeoDialogFactory.InformationDialog("GeoExplorationHelpDialog","Помош - Истражување", clonedContent, 600,600);
        dialog.Open();

        GeoUtility.SetupHelpDialogContents(dialog);
        clonedContent = null;

        return false;
    }

    function navigateLocOnMap(event) {
        var locationId = $(this).attr("data-id");

        var searchOptions = {};
        searchOptions["parentId"] = locationId;

        var listItem = $(this).closest("li");
        var index = listItem.index();

        $("#MapNav li:gt(" + index + ")").remove();

        exploreSearch = true;
        GeoSearch.GetLocations(searchOptions);
        return false;
    }

    function resetMap(event) {
        resetSearch = true;
        $("#MapNav").html("");
        map.setZoom(2);
        map.setCenter(new window.google.maps.LatLng(0, 0));
        GeoSearch.GetLocations();
        return false;
    }

    function searchFailHook(object, status, thrown) {
        alert(status);
    }

    // Callback after performing a serrch 
    function searchSuccessHook(data, status, object) {
        $("#ExplorationMap").unmask();
        var myMarker;

        // if we are drawing stuff for the first time, just draw the stuf for the first time.
        if (firstDraw) {
            GeoUtility.log("First Draw called");
            firstDraw = false;
            drawTopLevel(data);
            return;
        }

        if (!exploreSearch && !resetSearch) {
            clearAllMarkers();
            // otherwise each call here is just exploration call
            // which can be a reset to original position in which case firstDraw will be set
            $.each(data, function (index, element) {
                myMarker = new geografija.map.markers.MyMarker(element.Lat, element.Lng, element.Name, map, element.Id, element);
                myMarker.getMarker();
                myMarker.setIconString("/Content/MarkerIcons/" + element.Icon);
                allMarkers.push(myMarker);
            });
        }

        else {
            // setup markers in a diferent way and zoom into parent location to fit all markers.
            var boundingBox = new window.google.maps.LatLngBounds();
            if ($(data).size() > 1) {
                clearAllMarkers();
                $.each(data, function (index, element) {
                    myMarker = new geografija.map.markers.MyMarker(element.Lat, element.Lng, element.Name, map, element.Id, element);
                    myMarker.getMarker();
                    myMarker.setIconString("/Content/MarkerIcons/" + element.Icon);
                    allMarkers.push(myMarker);
                    var latLngToExtend = myMarker.getActual().getPosition();
                    boundingBox.extend(latLngToExtend);
                });

                map.fitBounds(boundingBox);
            }
            else if ($(data).size() == 0) {
                displayPopUpNoLocations();
            }
            else {
                // Shows a single location if a single location was retrieved.
                clearAllMarkers();
                var elementSingle = data[0];
                myMarker = new geografija.map.markers.MyMarker(elementSingle.Lat, elementSingle.Lng, elementSingle.Name, map, elementSingle.Id, elementSingle);
                myMarker.getMarker();
                myMarker.setIconString("/Content/MarkerIcons/" + elementSingle.Icon);
                allMarkers.push(myMarker);

                // Because of some bug we remove the zoom changes listener
                
                exploreSearch  = true;
                singleResultSearch = true;
                zoomCounterSingleResult =1;
                map.setCenter(myMarker.getActual().getPosition());
                map.setZoom(elementSingle.LocationZoomLevel);
                

            }
        }

        setupMarkerEvents();
    }

    function displayPopUpNoLocations() {
    }

    function drawTopLevel(data) {
        GeoUtility.log("Draw Top Level Called");
        $("#ExplorationMap").unmask();
        $.each(data, function (index, element) {
            var myMarker = new geografija.map.markers.MyMarker(element.Lat, element.Lng, element.Name, map, element.Id, element);
            myMarker.getMarker();
            myMarker.setIconString("/Content/MarkerIcons/" + element.Icon);
            allMarkers.push(myMarker);
        });

        setupMarkerEvents();
        window.google.maps.event.addListener(map, 'zoom_changed', mapZoomChanged);
    }

    function mapZoomChanged(x) {
        if(singleResultSearch){
            if(zoomCounterSingleResult <= 2 ){
                zoomCounterSingleResult++;
                return;
            }
            else{
                singleResultSearch = false;
            }
        }
        if (!exploreSearch && !resetSearch) {
        
            var currentZoom = map.getZoom();

            var searchOptions = {};
            searchOptions["zoom"] = currentZoom;
            searchOptions["zoomTolerance"] = zoomTolerance;

            $("#MapZoomLevel").html(map.getZoom());
            GeoSearch.GetLocations(searchOptions);
            $("#MapNav").html("");
        }
        else {
            exploreSearch = false;
            resetSearch = false;
        }
    }

    function zoomHandler(event) {
        var i,
            max,
            marker,
            tmpMarker,
            id = $(this).attr("data-id");

        //get the clicked marker from the array of all markers
        for (i = 0, max = allMarkers.length; i < max; i++) {
            tmpMarker = allMarkers[i];
            if (tmpMarker.id == id) {
                marker = tmpMarker;
                break;
            }
        }
        // explore the clicked markup
        exploreMarker(marker);
        return false;
    }

    function closeHandler(event) {
        infowindow.close();
        return false;
    }

    function detailsHandler(event) {
        return true;
    }

    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    // ===   Map Exploration Utilities  ------------------------------------------------------------------------------------
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------

    function clearAllMarkers() {
        $.each(allMarkers, function (index, element) {
            element.getActual().setMap(null);
            element.getActual = null;
        });

        allMarkers = null;
        allMarkers = new Array();
    }

    function setupMarkerEvents() {
        $.each(allMarkers, function (index, element) {
            window.google.maps.event.addListener(element.getActual(), 'click', markerHandler(element.id, element.data));
        });
    }

    function markerHandler(id, data) {
        return function (mouse) {
            var content = getIwContent(data, this, mouse);
            infowindow.setContent(content);
            infowindow.open(map, this);
            $(".scrollbarWrapper").tinyscrollbar();
        };
    }

    function getIwContent(data, marker, mouse) {
        var globalWrapper = $("#ExplorationCalloutWrapper");

        // Setup the markup and information 
        $("#LocationName", globalWrapper).html(data.Name);
        $("#LocationName", globalWrapper).css("color", GeoUtility.ColorHexString(data.Color));
        $("#LocationType", globalWrapper).html(data.TypeName);
        $("#ZoomLevel", globalWrapper).html(data.LocationZoomLevel);
        $("#LocationShortDescription", globalWrapper).html("");
        $("#LocationShortDescription", globalWrapper).html($("<div>" + data.ShortDescription + "</div>").html());

        // Setup the actions :
        // Details : 
        $("#Details", globalWrapper).attr("data-id", data.Id);
        $("#Details", globalWrapper).attr("href", "/Search/Location/" + data.Id);

        // Zoom :
        $("#Zoom", globalWrapper).attr("data-id", data.Id);

        if (!data.HasChildren) {
            $("#Zoom", globalWrapper).hide();
        }
        else {
            $("#Zoom", globalWrapper).show();
        }

        return globalWrapper.html();
    }

    return {
        Initialize: initialize
    };
})();