var AddLocationScript = (function () {
    var marker; //the marker created by clicking on the map
    var selectedMarker; // variable that will hold the current selected marker
    var map; // the map 
    var myMarkers = new Array(); // the markers returned from geocode.  MyMarkerObjects. Not ACtual Markers

    var missingMarkerDialogName = "CreateLocation_MarkerMissing";

    function init(){
        $(document).ready(function () {
            setUpParentPicker();
            setUpRichEditor();
            wireUpEvents();
            map = geografija.map.create.CreateMapD("map");
            google.maps.event.addListener(map, 'click', function (e) {

                //regular marker add using googlemaps
                marker = geografija.map.markers.addMarker(map, e.latLng, marker);

                marker.setIcon("http://gmaps-samples.googlecode.com/svn/trunk/markers/blue/blank.png");
                selectedMarker = marker;

                for (var i = 0; i < myMarkers.length; i++) {

                    myMarkers[i].getActual().setIcon("http://gmaps-samples.googlecode.com/svn/trunk/markers/red/blank.png");
                }

                geografija.util.changeValR("Lat", e.latLng.lat(), 5);
                geografija.util.changeValR("Lng", e.latLng.lng(), 5);

                geografija.map.events.AddMarkerClick(marker, function () {

                    geografija.util.changeValR("Lat", this.getPosition().lat(), 5);
                    geografija.util.changeValR("Lng", this.getPosition().lng(), 5);

                    for (var i = 0; i < myMarkers.length; i++) {

                        myMarkers[i].getActual().setIcon("http://gmaps-samples.googlecode.com/svn/trunk/markers/red/blank.png");
                    }

                    marker.setIcon("http://gmaps-samples.googlecode.com/svn/trunk/markers/blue/blank.png");
                    selectedMarker = marker;

                });
            });
        });
    }

    // sets up the short description field as a rich html editor
    function setUpRichEditor() {
        $("#Description").wysiwyg();
    }

    function setUpParentPicker() {
        //initialize and set the data provider
        GeoSearch.Initialize();
        GeoDisplay.SetDataProvider(GeoSearch);

        //Initialize and set up the display module
        GeoDisplay.Init({ global: $("div.global") }, "Изберете каде припаѓа новата локација :", false, { links: false, parent: false, search: true });
        //Set up hooks
        GeoDisplay.SetDisplayedLocationClickHook(clickOnDisplayedLocation);
        GeoDisplay.SetDrawLocationsHook(locationsDrawHook);

        $("#ClearParentLocation").click(function (event) {
            GeoDisplay.Deselect();

            //Clear Selected location values
            $("#ParentLocationName").val("");
            $("#ParentLocationId").val("");

            event.stopPropagation();
            return false;
        });

        GeoUi.ResizeContent();
    };

    function clickOnDisplayedLocation(name, id) {
        $("#ParentLocationName").val(name);
        $("#ParentLocationId").val(id);
    };

    function locationsDrawHook() {
        $("#ParentLocationName").val("");
        $("#ParentLocationId").val("");
    };

    // called on clicking  the find locations buttn
    function geocodeAddress() {
        var textBox;
        var text;

        if (!marker) {
            geografija.util.changeValR("Lat", 0, 5);
            geografija.util.changeValR("Lng", 0, 5);
        }
        else { // if there is an custom marker set the values to the position(lat lng) values of that marker
            geografija.util.changeValR("Lat", marker.getPosition().lat(), 5);
            geografija.util.changeValR("Lng", marker.getPosition().lng(), 5);
            //martk the marker blue = selected
            marker.setIcon("http://gmaps-samples.googlecode.com/svn/trunk/markers/blue/blank.png");
            selectedMarker = marker;
        }

        textBox = document.getElementById("tbox_address");
        text = textBox.value;

        //call getRaw which does a call to gmaps geocode service and 
        //expects a callback function to accespt a message and raw geocode results
        geografija.map.geocode.getRaw(text, function (raw, message) {
            if (message = "OK") {
                // if you dont have some markers already just parse the raw results
                if (myMarkers.length == 0) {
                    myMarkers = geografija.map.geocode.parseRaw(map, raw);
                }
                else { // if you have old markers, best clear them out first
                    //clear the old ones first
                    for (var k = 0; k < myMarkers.length; k++) {
                        myMarkers[k].setMap(null);
                    }
                    myMarkers = geografija.map.geocode.parseRaw(map, raw);
                }

                //after parsing the raw results
                //add the new icons 
                for (var i = 0; i < myMarkers.length; i++) {
                    //create an actual marker on the map
                    myMarkers[i].getMarker();
                    //add a click event
                    geografija.map.events.AddMarkerClick(myMarkers[i].getActual(), function () {
                        //get and set the values to the text fields
                        geografija.util.changeValR("Lat", this.getPosition().lat(), 5);
                        geografija.util.changeValR("Lng", this.getPosition().lng(), 5);
                        //set the name field
                        geografija.util.changeVal("Name", this.getTitle());

                        //set all the  other markers as red =  not selected
                        for (var i = 0; i < myMarkers.length; i++) {
                            myMarkers[i].getActual().setIcon("http://gmaps-samples.googlecode.com/svn/trunk/markers/red/blank.png");
                        }

                        //if there is a freehand marker set its color to red = not selected
                        if (marker) {
                            marker.setIcon("http://gmaps-samples.googlecode.com/svn/trunk/markers/red/blank.png");
                        }

                        //set the clicked marker to blue =  selecter
                        this.setIcon("http://gmaps-samples.googlecode.com/svn/trunk/markers/blue/blank.png");
                        selectedMarker = this;
                    });
                    refitViewPort(map, myMarkers, marker);
                }
            }
            // if the callback function is called with something other than "OK"
            else {
                alert("Something went wrong with geocoding call to google service");
            }
        });
        return false;
    };

    function refitViewPort(map, markers, marker) {
        var myBounds = new google.maps.LatLngBounds();

        if (markers && markers.length > 0) {
            for (var i = 0; i < myMarkers.length; i++) {
                myBounds.extend(markers[i].getLatLng());
            }
        }

        if (marker) {
            myBounds.extend(marker.getPosition());
        }

        map.fitBounds(myBounds);
    };

    function showDisplayAjax() {
        var dropDown = document.getElementById("ChosenDisplaySetting");
        var id = dropDown.value;

        if (!id) {
            return false;
        }

        var dataJax = JSON.stringify({ id: id });

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: dataJax,
            url: "/Locations/LocationDisplaySettingsAjax",
            dataType: 'json',
            success: function (msg) {
                ChangeMapView(msg);
            },
            error: function (msg) {
                ChangeMapView(msg);
            }
        });
        return false;
    };

    function ChangeMapView(OptionsObject) {
        var lat;
        var lng;
        var latParam = null;
        var lngParam = null;

        lat = document.getElementById("Lat").value;
        lng = document.getElementById("Lng").value;

        if (lat && lat != 0 && lat != "0" && lat != "") {
            latParam = lat;
        }

        if (lng && lng != 0 && lng != "0" && lng != "") {
            lngParam = lng;
        }

        if (!OptionsObject.Result && !OptionsObject.Result != null && !OptionsObject.Result != 0) {
            alert("");
            return;
        }

        var myMapOptions = new geografija.map.create.MyMapOptions(parseInt(OptionsObject.Zoom), OptionsObject.MapType, latParam, lngParam, OptionsObject.RenderControlls);
        var options = myMapOptions.getOptions();

        map.setOptions(options);
    };

    function submit() {
        var dialog,
            zoom,
            mapType,
            checkBox;

        if (!selectedMarker) {
            dialog = GeoDialogFactory.InformationDialog(missingMarkerDialogName, "Изберете Локација", GeoUtility.Messages.PickLocationMarker, 400, 200);
            dialog.Open();

            return false;
        }
        zoom = map.getZoom();
        mapType = map.getMapTypeId();
        checkBox = document.getElementById("CurrentDisplaySetting");

        if (checkBox.checked) {
            $("#Zoom").attr('value', zoom);
            $("#MapType").attr('value', mapType);
        }
        return true;
    };

    function valueChangeCurrentDispaly() {
        checkBox = document.getElementById("CurrentDisplaySetting");

        if (checkBox.checked) {
            $("#displayNameHidden").slideDown();
        }
        else {
            $("#displayNameHidden").slideUp();
        }
    };

    function wireUpEvents() {
        var geocodeButton; // button for location serach
        var displayLocation;
        var form;
        var checkBox;

        //search location wire up
        geocodeButton = document.getElementById("btn_search");
        geocodeButton.onclick = geocodeAddress;

        form = document.getElementById("form");
        form.onsubmit = submit;

        displayLocation = document.getElementById("showDisplay");
        displayLocation.onclick = showDisplayAjax;

        checkBox = document.getElementById("CurrentDisplaySetting");
        checkBox.onchange = valueChangeCurrentDispaly;
    };

    return{
        Initialize:init
    };
})();