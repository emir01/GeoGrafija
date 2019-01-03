//global namespace/object   geografija
var geografija = (function () {

    //Private. Not callable from outside. For Readability
    //marker Module. Falls under map
    //Namespace for Creating Maps     ========================  map.markers ===========================
    var BasicMarkerFunctions = (function () {

        function addMarker(map, position, marker) {
            if (!marker) {
                marker = new google.maps.Marker({
                    map: map,
                    icon: 'http://gmaps-samples.googlecode.com/svn/trunk/markers/red/blank.png'
                });
            }

            marker.setPosition(position);
            return marker;
        };

        // My Marker Object
        function MyMarker(lat, lng, title, map, id, data) {
            this.lat = lat;
            this.lng = lng;
            this.title = title;
            this.map = map;
            this.properMarker = null;
            this.id = id;
            this.data = data;

            this.getMarker = function () {
                var latLng = new google.maps.LatLng(this.lat, this.lng);

                if (!this.properMarker) {
                    this.properMarker = new google.maps.Marker({
                        position: latLng,
                        map: this.map,
                        animation: google.maps.Animation.DROP,
                        title: this.title,
                        icon: 'http://gmaps-samples.googlecode.com/svn/trunk/markers/red/blank.png'
                    });
                }
                else {
                    this.properMarker.setPosition(latLng);
                }
                return this.properMarker;
            };

            this.setMap = function (map) {
                this.properMarker.setMap(map);
            };

            this.showMe = function () {
                var stringToShow = "";
                stringToShow += "Lat : " + this.lat + "\n";
                stringToShow += "Lng : " + this.lng + "\n";
                stringToShow += "Title: " + this.title;

                alert(stringToShow);
            };

            this.setPosition = function (lat, lng) {
                var googlePos;
                googlePos = new google.maps.LatLng(lat, lng);
                this.properMarker.setPosition(googlePos);
            };

            this.getActual = function () {
                return this.properMarker;
            };

            this.getLatLng = function () {
            return new google.maps.LatLng(this.lat, this.lng);
            };

            this.setIconString = function (iconString) {
                this.properMarker.setIcon(iconString);
            };

        }; //end of myMarker Object

        return {
            addMarker: addMarker, //geografija.map.markers.addMarker(map, position, marker)
            MyMarker: MyMarker // new geografija.map.markers.MyMarker(lat, lng, title, map) 
        };

    } ());

    //Namespace for Wiring Up Events   ========================  map.events===========================
    var Events = (function () {

        function AddClickEvent(map, doWhat) {
            google.maps.event.addListener(map, 'click', function (e) {
                doWhat(map, e.latLng());
            });
        };
        function AddMarkerClick(marker, handler) {

            google.maps.event.addListener(marker, 'click', handler);

        };

        return {
            AddClickEvent: AddClickEvent,  //geografija.map.events.AddClickEvent(map,doWhat)s
            AddMarkerClick: AddMarkerClick  //geografija.map.events.AddMarkerClick(marker,handler);
        };

    } ());

    //Namespace for Creating Maps     ========================  geografija.map.geocode ===========================

    var BasicGeocoding = (function () {

        function Geocode(address, callback) {
            var geocoder; //the geocoder object used to find the locations
            var geocoderRequest; //the request to user with the geocoder object

            geocoderRequest = {
                address: address
            };

            geocoder = new google.maps.Geocoder();

            geocoder.geocode(geocoderRequest, function (results, status) {

                if (status == google.maps.GeocoderStatus.OK) {
                    callback(results, "OK");
                }

                if (status == google.maps.GeocoderStatus.ERROR) {
                    callback(results, "Error Contacting Google Server!");
                }

                if (status == google.maps.GeocoderStatus.OVER_QUERY_LIMIT) {
                    callback(results, "We are over the query limit for geocoding!");
                }

                if (status == google.maps.GeocoderStatus.REQUEST_DENIED) {
                    callback(results, "We are not allowed to use the geocoder.");
                }

                if (status == google.maps.GeocoderStatus.UNKNOWN_ERROR) {
                    callback(results, "A geocoding request could not be processed due to a server error. The request may succeed if we  try again.");
                }

                if (status == google.maps.GeocoderStatus.ZERO_RESULTS) {
                    callback(results, "NO RESULTS");
                }
            });

        };

        function parseRawResults(map, rawResults) {
            var myResults = new Array();
            var rawResult;
            for (var i = 0; i < rawResults.length; i++) {
                rawResult = rawResults[i];
                var myMarker = new geografija.map.markers.MyMarker(
                   rawResult.geometry.location.lat(),
                   rawResult.geometry.location.lng(),
                   rawResult.address_components[0].short_name,
                   map
                );

                myResults.push(myMarker);
            }
            return myResults;

        };

        return {
            getRaw: Geocode, //geografija.map.geocode.getMyMarkers(address,callbackFunction);
            parseRaw: parseRawResults//geografija.map.geocode.getMyMarkers(address);
        };
    })();

    //=====================

    //Namespace for Creating Maps     ========================  map.create ===========================
    var BasicCreatingFunctions = (function () {

        // Custom Object for Creating  custom google maps options 
        function MyMapOptions(zoom, typeString, centerLat, centerLng, disableControls) {
            //Properties
            this.zoom = zoom;
            this.typeString = typeString;
            this.centerLat = centerLat;
            this.centerLng = centerLng;
            this.disableControls = disableControls

            //Functions
            this.showMe = function () {
                var txt = "";
                txt += this.zoom + "\n";
                txt += this.typeString + "\n";
                txt += this.centerLat + "\n";
                txt += this.centerLng + "\n";
                txt += this.disableControls + "\n";
                alert(txt);
            };

            this.getOptions = function () {
                var optionsObject;
                var type;
                var center;
                var zoom = this.zoom;

                if (centerLat != null && centerLng != null) {
                    center = new google.maps.LatLng(centerLat, centerLng);
                }
                else {
                    center = null;
                }

                switch (this.typeString.toUpperCase()) {
                    case "ROADMAP":
                        type = google.maps.MapTypeId.ROADMAP;
                        break;
                    case "HYBRID":
                        type = google.maps.MapTypeId.HYBRID;
                        break;
                    case "SATELLITE":
                        type = google.maps.MapTypeId.SATELLITE;
                        break;
                    case "TERRAIN":
                        type = google.maps.MapTypeId.TERRAIN;
                        break;
                }

                if (center != null) {
                    optionsObject = {
                        mapTypeId: type,
                        center: center,
                        zoom: zoom,
                        disableDefaultUI: this.disableControls

                    };
                }
                else {
                    optionsObject = {
                        mapTypeId: type,
                        zoom: zoom,
                        disableDefaultUI: this.disableControls

                    };
                }

                return optionsObject;
            };
        };   //_________________ GOOGLE OPTIONS MY OBJECT

        //Function to craete default map
        function CreateMapD(elementId) {
            var element = document.getElementById(elementId);
            var map;
            var options;

            if (!element) {
                alert("There is no element with id : " + elementId);
                return null;
            }
            options = {
                mapTypeId: google.maps.MapTypeId.TERRAIN,
                center: new google.maps.LatLng(0, 0),
                zoom: 2
            };
            map = new google.maps.Map(element, options);


            GeoUi.ResizeContent();
            return map;
        };

        //creating a map using a custom option google maps object
        //Can be created with the MyMapOptions class
        function CreateMap(elementId, options) {
            var element = document.getElementById(elementId);
            var map;

            if (!element) {
                alert("There is no element with id : " + elementId);
                return null;
            }

            map = new google.maps.Map(element, options);

            GeoUi.ResizeContent();
            return map;
        };

        // Public Api of BasicCreatingFunctions
        return {
            CreateMap: CreateMap, //geografija.map.create.CreateMap(elementId, options) 
            CreateMapD: CreateMapD, //geografija.map.create.CreateMapD(elementId)
            MyMapOptions: MyMapOptions //new geografija.map.create.MyMapOptions(zoom, typeString, centerLat, centerLng, disableControls)
        };
    } ());

    //===============================================================================

    //Namespace for Creating Maps     ========================  geografija.util===========================
    var Utils = (function () {
        function AlertMessage(message) {
            alert(message);
        };

        function ChangeFieldValue(fieldId, val) {
            var element;

            element = document.getElementById(fieldId);

            if (element) {
                element.value = val;
            }
            else {
                alert("There is no element with id : " + fieldId);
            }

        };

        function ChangeFieldValueR(fieldId, val, round) {
            var element;

            element = document.getElementById(fieldId);

            if (element) {
                element.value = Math.round((val * (Math.pow(10, round)))) / Math.pow(10, round);
            }
            else {
                alert("There is no element with id : " + fieldId);
            }

        };

        //Object for storing all the data to display a location.
        function LocationDisplayParameters(name,
                                           description,
                                           lat,
                                           lng,
                                           zoom,
                                           mapType,
                                           controls,
                                           icon,
                                           typeName,
                                           typeDesciption) {

            this.name = name;
            this.description = description;
            this.lat = lat;
            this.lng = lng;

            this.zoom = zoom;
            this.mapType = mapType;
            this.controls = controls;
            this.icon = icon;

            this.typeName = typeName;
            this.typeDescription = typeDesciption;

            function getPosition() {
                return new google.maps.LatLng(lat, lng);
            };

        };
        // TODO : Make this more robust
        function pullLocationDisplayOptions() {
            var options; // the options object to be returned

            var name;
            var description;
            var lat;
            var lng;

            var typeName;
            var typeDescription;

            var zoom;
            var mapType;
            var renderControls;
            var icon;

            //pull the values 
            //Location Name
            name = document.getElementById("Name").value;

            //Location Desciption
            //description = document.getElementById("Description").value;
            description = "";

            lat = document.getElementById("Lat").value;
            lng = document.getElementById("Lng").value;

            // typeName = document.getElementById("TypeName").value;
            // typeDescription = document.getElementById("TypeDescription").value;
            zoom = document.getElementById("Zoom").value;
            mapType = document.getElementById("MapType").value;


            var renderControllsContainer = document.getElementById("RenderControls");
            var iconContainer = document.getElementById("Icon");

            //If Specified. Set to VAlue
            if (renderControllsContainer) {
                renderControls = renderControllsContainer.value;
            }
            else {
                //IfNot set to true /// for Edit
                renderControls = "True";
            }

            if (iconContainer) {
                icon = iconContainer.value;
            }
            else {
                icon = null;
            }

            options = new geografija.util.LocationDisplayOpts(name,
                                                               description,
                                                               lat,
                                                               lng,
                                                               zoom,
                                                               mapType,
                                                               renderControls,
                                                               icon
                                                               );

            return options;
        };

        function setMap(map, displayOpts) {

            var myMapOptions; // MyMapOptions Object
            var mapOptions; // True Options Object

            var zoom = parseInt(displayOpts.zoom);
            
            // Override map type to terrain 
            //var mapType = displayOpts.mapType;
            var mapType = google.maps.MapTypeId.TERRAIN;

            var centerLat = displayOpts.lat;
            var centerLng = displayOpts.lng;

            var disableControls;

            if (displayOpts.controls == "True") {
                disableControls = false;
            }
            else {
                disableControls = true;
            }

            myMapOptions = new geografija.map.create.MyMapOptions(zoom, mapType, centerLat, centerLng, disableControls);

            mapOptions = myMapOptions.getOptions();

            //Set The Options for The Map
            map.setOptions(mapOptions);

            // Create a Marker 
            var myMarker = new geografija.map.markers.MyMarker(centerLat, centerLng, displayOpts.name, map);
            myMarker.getMarker();

            //If Icon Specified set to icon. 
            //If Not Specified Dispaly Default ( Edit//)
            if (displayOpts.icon) {
                myMarker.setIconString("/Content/MarkerIcons/" + displayOpts.icon);
            }

            return myMarker.getMarker();
        };


        // Public Api of Utils
        return {
            alert: AlertMessage, //geografija.util.alert(message)
            changeVal: ChangeFieldValue, //geografija.util.changeVal(elementId,val)
            changeValR: ChangeFieldValueR, //geografija.util.changeVal(elementId,val,round)
            LocationDisplayOpts: LocationDisplayParameters, // new geografija.util.LocationDisplayOptions(params)
            pullLocationDisplayOpts: pullLocationDisplayOptions, //  geografija.util.pullLocationDisplayOpts()
            setMap: setMap //    geografija.util.setMap(map,displayOpts)
        };
    } ());


    //==============================================================================


    //Public API. Access Point to geografija namespace
    return {
        map: (function () { //geografija.map
            return {
                markers: BasicMarkerFunctions, //geografija.map.markers.**Functions**
                create: BasicCreatingFunctions, //geografija.map.create.**Functions**
                events: Events, //geografija.map.events.**Functions**
                geocode: BasicGeocoding////geografija.map.geocode.**Functions**
            };
        } ()),

        util: Utils //geografija.util


    };

}
)();