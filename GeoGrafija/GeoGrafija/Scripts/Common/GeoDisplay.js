var GeoDisplay = (function () {

    //Define the hooks to be used by client code;

    // Called on click event on one of the expanded location nodes. 
    var expandedNodeClickHook;

    // Called on the click event on one of the locations in the main loc window.
    // Should expect location name and id as parameters  displayedLocationClickHook(locationName, locationId)
    var displayedLocationClickHook;

    //Called on begining of draw function for locations.
    var beginDrawLocationsHook;

    // Define a data provider object and callback.
    // Needs to to have GetData(options);
    var dataProvider = null;

    //Define Controller and Action for Details Page For location
    var controller = "Search";
    var action = "Location";

    //Define some options :
    var contextSearch = false;
    var selectable = true;
    var links = false;
    var makeStrip = true;
    var makeParent = true;
    var makeSearch = false;
    var makeContextSearch = false;
    var makeSerachHelp = false;

    var allowExpansion = true;
    var allowEmptySearch = false;

    var allowCrumbs = true;

    //Define some constant values 
    var hueChange = 10;

    //The init funcion needs to be called before using the Module or setting up any hooks
    function init(elements, heading, pasive, options) {
        getOptions(options);

        if (elements.global) {
            setUpMarkup(elements.global, heading);
        }
        else {
            setUpPartialMarkup(elements.canvas, elements.parent, elements.strip, elements.search, heading);
        }

        if (!pasive)
            getLocations("");

        wireUpEvents();
        preloadImages();
    };

    function preloadImages() {
        //$('<img/>')[0].src = "/Content/images/expand.png";
        //$('<img/>')[0].src = "/Content/images/expand-hover.png";
    }

    // Extract options from the optional options anonymous object that calling scripts could pass to Geo Display
    function getOptions(options) {

        // Extract the links option that specifies if the elements 
        // should include link that takes it to the details page
        if (options.hasOwnProperty("links")) {
            links = options.links;
        }

        // Extract the links option that specifies if the elements 
        // should include link that takes it to the details page
        if (options.hasOwnProperty("selectable")) {
            selectable = options.selectable;
        }

        // extract the strip option that tells Geo Display 
        // wether it should generate the breadcrumbs strip
        if (options.hasOwnProperty("strip")) {
            makeStrip = options.strip;
        }

        // extract the parrent option that tells Geo Display 
        // if it should generate the parent placeholder and place 
        // elements in the parent placeholder after expanding the,
        if (options.hasOwnProperty("parent")) {
            makeParent = options.parent;
        }

        // extract the search option that tells Geo Display 
        // if it should generate the search functionalirty 
        // for locations
        if (options.hasOwnProperty("search")) {
            makeSearch = options.search;
        }

        // extract the search option that tells Geo Display 
        // if it the search functionality should include options for context
        // searching
        if (options.hasOwnProperty("contextSearch")) {
            makeContextSearch = options.contextSearch;
            contextSearch = true;
        }

        // extract option that decides if search help should be generated.
        if (options.hasOwnProperty("searchHelp")) {
            makeSerachHelp = options.searchHelp;
        }

        // extract option that decides if  locations can be expanded if they have children.
        if (options.hasOwnProperty("expansion")) {
            allowExpansion = options.expansion;
        }

        // extract option that decides if  empty search string can happen
        if (options.hasOwnProperty("emptySearch")) {
            allowEmptySearch = options.emptySearch;
        }

        // extract option that decides if  empty search string can happen
        if (options.hasOwnProperty("allowCrumbs")) {
            allowCrumbs = options.allowCrumbs;
        }


    };

    function setUpPartialMarkup(canvas, parent, strip, search, headingMessage) {
        // Create a basic clear and padding div to be used where necesary using clone.
        var jClearDiv = $("<div></div>").addClass("clear");
        var jPadding = $("<div></div>").addClass("padding");

        // Create the strip elements.
        var jStrip = $("<div></div>").addClass("strip");
        var jStripPadding = $("<div></div>").addClass("padding");
        var jStripBreadcrumbs = $("<ul></ul>").addClass("stripBreadcrumbs");
        var jStartBreadcrumb = getStartBreadCrumb();

        // Create the canvas elements
        var jCanvas = $("<div></div>").addClass("locationCanvas");
        var jCanvasElementContainer = $("<ul></ul>").addClass("canvasElementContainer");

        //Create the parrent elements
        var jParent = $("<div></div>").addClass("parent");
        jParent.append(jPadding.clone());

        // Combine all the canvas elements
        jCanvas.append(jCanvasElementContainer);
        jCanvas.append(jClearDiv.clone());

        // Combine the strip elements
        jStripBreadcrumbs.append(jStartBreadcrumb);
        jStripPadding.append(jStripBreadcrumbs);
        jStripPadding.append(jClearDiv.clone());

        jStrip.append(jStripPadding);
        jStrip.append(jClearDiv.clone());

        // Add the created markup and elements to the apropriate containers
        // passed via the anonymous object in the init method
        if (canvas)
            canvas.append(jCanvas);

        if (makeStrip)
            if (strip)
                strip.append(jStrip);

        if (makeParent)
            if (parent)
                parent.append(jParent);

        if (makeSearch) {
            var jSearch = createSearchBar(jClearDiv, jPadding);
            search.append(jSearch);
        }

        GeoUi.ResetHintBoxes();
    };

    // == Helpers for creating specific search element specified markup
    function setUpMarkup(global, headingMessage) {
        //Create basic clear and padding div to be used where necesary with clone method
        var jClearDiv = $("<div></div>").addClass("clear");
        var jPadding = $("<div></div>").addClass("padding");

        // Creates the global wrapper where everyother element will be positioned
        var jGlobal = $(global);
        jGlobal.addClass("formBackground");

        // Create the heading message elements
        var jHeading = createGlobalHeading(headingMessage);

        var jSearchBar = createSearchBar(jClearDiv, jPadding);

        // Create the location strip elements
        var jStrip = createGlobalStip(jClearDiv);

        // Create  the global canvas element
        var jCanvas = createGlobalCanvas(jClearDiv);

        //Create the parrent elements
        var jParent = createGlobalParent(jPadding);

        // Add The created markup and elements to the global wrapper suplied via the anonymous object
        // in the init method of GeoDisplay
        jGlobal.append(jHeading);

        if (makeSearch) {
            jGlobal.append(jSearchBar);
            jGlobal.append(jClearDiv.clone());
        }

        if (makeStrip)
            jGlobal.append(jStrip);

        jGlobal.append(jClearDiv);

        if (makeParent)
            jGlobal.append(jParent);

        jGlobal.append(jCanvas);
        GeoUi.ResetHintBoxes();
    };

    //==   Helpers for creating global markup 
    function createGlobalParent(jPadding) {
        var jParent = $("<div></div>").addClass("parent");
        jParent.append(jPadding.clone());

        return jParent;
    };

    function createGlobalCanvas(jClearDiv) {
        var jCanvas = $("<div></div>").addClass("globalLocaitonCanvas locationCanvas");
        var jCanvasElementContainer = $("<ul></ul>").addClass("canvasElementContainer");

        jCanvas.append(jCanvasElementContainer);
        jCanvas.append(jClearDiv.clone());

        return jCanvas;
    };

    function createGlobalHeading(headingMessage) {
        var jHeading = $("<h3></h3>").addClass("formHeader mapPickerHeader");
        if (headingMessage) {
            jHeading.html(headingMessage);
        }
        else {
            jHeading.html("Локации :");
        }

        return jHeading;
    };

    function createGlobalStip(jClearDiv) {
        // Create the strip elements
        var jStrip = $("<div></div>");
        //Add the global strip class because this is where we generate general global markup
        jStrip.addClass("globalStrip strip");

        var jStripPadding = $("<div></div>").addClass("padding");
        var jStripBreadcrumbs = $("<ul></ul>").addClass("stripBreadcrumbs");
        var jStartBreadcrumb = $("<li></li>").addClass("stripBreadcrumbsElement").html("Почеток").attr("tag", "");

        //Combine all the strip elements
        jStripBreadcrumbs.append(jStartBreadcrumb);
        jStripPadding.append(jStripBreadcrumbs);
        jStripPadding.append(jClearDiv.clone());

        jStrip.append(jStripPadding);
        jStrip.append(jClearDiv.clone());

        return jStrip;
    };

    function wireUpEvents() {
        $(".stripBreadcrumbsElement").click(expandedNodeClick);

         // wire up the draw them already event for the animation
         // Hook it up to the parent element of the canvas and add it as a namespaced event
         var canvasParent = $(".locationCanvas").closest("body");
         canvasParent.on('click', function(){
                resetDrawingClear();
         })
     };

    //==
    //== Helpers that create common markup
    //== 
    function createSearchBar(jClearDiv, jPadding) {
        var jBox, jButton, jSearchBar, jContextBox;

        // Create the search bar wrapper
        jSearchBar = $("<div></div>").addClass("locationSearchBar globalLocationSearchBar");

        // Create the serach text input box
        jBox = $("<input type = 'text' />").addClass("locatinSearchBarBox hintBox").val("Побарај Локации");
        jBox.keyup(searchBoxTriggerSearch);

        // Create the button that activates the serach and hook up the event to it
        jButton = $("<input type = 'submit' />").addClass("locatinSearchBarButton button green").val("Најди");
        jButton.click(searchButtonClick);

        // Create the context search check box
        jContextBox = createSearchMarkupContextBox();

        //Create the search help link
        var jSearchHelpLink = createSearchHelpLink();

        // Construct and combine the markup
        if (makeSerachHelp) {
            jSearchBar.append(jSearchHelpLink);
        }

        jSearchBar.append(jBox);
        jSearchBar.append(jButton);
        jSearchBar.append(jClearDiv.clone());

        return jSearchBar;
    };

    function createSearchHelpLink() {
        var jSearchLink = GeoUtility.GiveSearchHelpMarkup();

        return jSearchLink;
    };

    function createSearchMarkupContextBox() {
        var jContextBox = $("<div></div>").addClass("locationSearchContextBox");
        var jCheckBox = $("<input type='checkbox'/>");
        var jCheckLabel = $("<span>Контекст</span>");

        jContextBox.append(jCheckBox);
        jContextBox.append(jCheckLabel);

        return jContextBox;
    };

    // Hook functions Seters and providers ====================================
    function setDrawLocationsHook(hookFunction) {
        beginDrawLocationsHook = hookFunction;
    };

    function setExpandedNodeClickHook(hookFunction) {
        expandedNodeClickHook = hookFunction;
    };

    function setDisplayedLocationClickHook(hookFunction) {
        displayedLocationClickHook = hookFunction;
    };

    function setDataProvider(provider) {
        dataProvider = provider;
        dataProvider.SetSuccessHook(dataProviderSuccessHook);
        dataProvider.SetFailHook(dataProviderFailHook);
    };

    var searchUtilFormDataProvider;

    function dataProviderSuccessHook(data, util) {
        searchUtilFormDataProvider = util;
        success(data);
    };

    function dataProviderFailHook(data) {
        fail(data);
    };

    // Event Handlers ====================================
    function expandedNodeClick(event) {
        // Get the index of this breadcrumb in the breadcrumbs list
        var index = $(this).index();

        // Get the location Id and Name
        // These will be used to create the new parent and to display the new children
        var locationId = $(this).attr("tag");
        var name = $(this).html();
        var color = $(this).attr("data-color");

        //remove all the other breadcrumbs that are above this breadcrumb by the index
        $(".stripBreadcrumbs .stripBreadcrumbsElement:gt(" + index + ")").remove();

        // Call the hook if any
        if (expandedNodeClickHook) {
            expandedNodeClickHook(locationId);
        }

        // Display the new child locations for this id
        getLocationsWrapper(locationId);

        //Place the clicked breadcrumb locaiton in the parent position
        placeInParentContainer(name, locationId, color);
    };

    function searchedNodeClick() {
        if (!selectable) {
            return;
        }

        $(".element").removeClass("selectedLocation");
        $(this).addClass("selectedLocation");

        var locationName = $(this).find(".searchedNameBox").text();
        var locationId = $(this).closest(".listChild").attr("tag");

        if (displayedLocationClickHook) {
            displayedLocationClickHook(locationName, locationId);
        }
    };

    function searchedNodeExpandButtonClick(event) {
        var locationId = $(this).closest(".listChild").attr("tag");
        var content = $(this).closest(".listChild").find(".searchedNameBox").text();
        var color = $(this).closest(".listChild").find(".searchedTypeBox").css("background-color");

        if (allowCrumbs) {
            $(".stripBreadcrumbs").append(getListItemForExpandedLocation(content, locationId, color));
        }

        // put element in parent position. Get the name and id first
        var id = $(this).closest(".element").closest(".listChild").attr("tag");
        var name = content;

        placeInParentContainer(name, id, color);

        getLocationsWrapper(locationId);
        event.stopPropagation();
    };

    // --event handlers for search functionality

    // called when we want to trigger serach by key on search box
    function searchBoxTriggerSearch(event) {
        if (event.keyCode == 13) {
            $(".locatinSearchBarButton").click();
        }
    };


    // called when the search button is clicked
    function searchButtonClick(event) {
        resetDrawingClear();

        var nameValue, parentId;
        var jBox = $(".locatinSearchBarBox");

        // Check if context search is on.  Here a check to the checkbox might come in, but now I assume 
        // it has been clicked as I dont know if that implementation is final
        if (contextSearch) {
            // check if there is a markup element with the apropriate class 
            // that holds the element of the parrent.
            // if the parent markup has been generated and parents are asigned as you serach this elmenet should be present.
            if ($(".searchParentIdContainerElement").size() > 0) {
                parentId = $(".searchParentIdContainerElement").first().attr("data-id");
            }
            else {
                parentId = "0";
            }
        }
        else {
            // if context search is not on then set parent id to empty so only serach by name is done.
            parentId = "0";
        }

        //extract the name value and make the call to the provider
        nameValue = jBox.val();
        nameValue = nameValue == jBox.attr("data-hint") ? "" : nameValue;

        if (!allowEmptySearch) {
            if ($.trim(nameValue) != "") {
                getLocationsWrapper(parentId, $.trim(nameValue));
            }
        }
        else {
            getLocationsWrapper(parentId, $.trim(nameValue));
        }

        event.stopPropagation();
        event.preventDefault();
    };

    // == Utility functions
    function placeInParentContainer(name, id, color) {

        // Check if there is a parent element to place in 
        if ($(".parent").size() > 0) {
            $(".parent").html("");
            if (name && id && color) {
                var jContent = getMarkupForParentContainer(name, id, color);
                $(".parent").append(jContent);
            }
        }
    };

    function getStartBreadCrumb() {
        return $("<li></li>").addClass("stripBreadcrumbsElement").html("Почеток").attr("tag", "");
    }

    function getMarkupForParentContainer(name, id, color) {
        var locationId = id;
        var locationName = name;

        // Create the parent content element and padding and combine        
        var jParent = $("<div></div>").addClass("content");
        var jParentPadding = $("<div></div>").addClass("padding");
        jParent.append(jParentPadding);

        // Add the name of the location to the content padding wrapper
        jParentPadding.html(locationName);

        // Set the correct css Properties as inherited from the element
        // Mostly operates on properties as dynamicly defined by the location type
        jParent.css("background-color", color);

        // Creat the link and add wrap the content with the link
        var link = getDetailsLink(locationId);
        link.addClass("searchParentIdContainerElement").attr("data-id", locationId);
        link.append(jParent);

        jParent.hover(hoverIn, hoverOut);
        return link;
    };

    function hoverIn(event) {
        var rgb = GeoUtility.GetRgbValuesForProperty("background-color", $(this));

        var hoverRgb = GeoUtility.ChangeColorHue(rgb, -hueChange);

        $(this).css("background-color", hoverRgb);
    };

    function hoverOut(event) {
        var rgb = GeoUtility.GetRgbValuesForProperty("background-color", $(this));

        var hoverRgb = GeoUtility.ChangeColorHue(rgb, hueChange);

        $(this).css("background-color", hoverRgb);
    };

    // Main Location retrieval Calls and Helpers ====================================

    //Called from click events on expanded or searched locations passing the id of the parrent.
    function getLocationsWrapper(parentId, name) {
        getLocations(parentId, name);
    };

    function performDisplay(term) {
        var searchTerm;

        if (term) {
            searchTerm = term;
        }
        else {
            searchTerm = "се";
        }

        getLocationsWrapper("", "се");
    }

    function getLocations(parentId, name) {
        if (!dataProvider) {
            GeoAjax.MakeAjaxPost(GeoAjax.GetUrlForAction("Search", "AjaxFindLocations"), GeoAjax.SimpleAjaxParam(parentId, "parentId"), success, fail);
        }
        else {
            dataProvider.GetLocations({ parentId: parentId, name: name });
        }
    };

    function success(data) {
        drawLocations(data);
    };

    function fail(msg) {
        alert("Грешка во апликацијата. Ве молиме контактирајте го администраторот, ако грешката продолжи да се случува.");
    };

    //====================================================================
    //====================================================================
    //====================================================================
    // Main Draw Location Call
    // Probably the main funciton call whre the locations are drawn on the canvas
    //====================================================================
    //====================================================================
    //====================================================================
    function drawLocations(data) {

        resetDrawingClear();

        $(".canvasElementContainer").html("");


        if (beginDrawLocationsHook) {
            beginDrawLocationsHook();
        }

        // If there are no locations to be drawn return after drawing message for no locations
        if(data.length == 0 ){
            drawNoLocation();
            return;
        }

        
        for (var i = 1; i <= data.length; i++) {

            var location = getLocationMarkup(data[i - 1]);
            location.click(searchedNodeClick);
            location.find(".expand .exploreLink").click(searchedNodeExpandButtonClick);
            getListItemForLocation(location, data[i - 1].Id, i).appendTo(".canvasElementContainer");
        }

        $(".detailsLink").on('click.search',function(){
            $("body").trigger('click');
            return false;
        })

        $("<div></div>").addClass("clear").appendTo(".canvasElementContainer");

        drawExtras();

        fadeInLocations(1);
    };

    // Fix to use jquery object and $.each for simpler code and no messing
    // with callbacks on the animation
    function fadeInLocations(number) {
        var box = $("#box" + number);
        if (box.size() != 1) {
            return;
        }

        $("#box" + number).animate({
            opacity: 1
        },
        200,
        function () {
            // callback
            $(this).find(".detailsLink").off(".search");
            fadeInLocations(number + 1);
        });
    };

    function drawNoLocation(){
        var container =  $(".canvasElementContainer");
        var noLocMessage =  $("<div></div>").html("Нема Локации!").addClass("noLocationsDivMessage");
        container.append(noLocMessage);
    }

    function resetDrawingClear(){
        $(".detailsLink",".canvasElementContainer").off(".search");
        $(".listChild").stop(true,true)
    }

    // queries the data provider to check if there is something extra to be done.
    function drawExtras() {
        var utilObject = searchUtilFormDataProvider;

        if (!utilObject) {
            return;
        }

        if (utilObject.SearchedId && utilObject.FirstCalled) {
            // make call for locatio details for the parent location id and get the trail for the parent location id;
            dataProvider.GetLocationDetails(utilObject.SearchedId, drawSpecial, detailsFail);
        }
    };

    function drawSpecial(data) {
        if (!data)
            return;

        var crumbs = $(data.Path);

        if (allowCrumbs) {
            if (crumbs) {
                crumbs.each(function (index, element) {
                    $(".stripBreadcrumbs").append(getListItemForExpandedLocation(element.Name, element.Id, "#" + element.Color));
                });
            }
        }

        if (data.Name && data.Id && data.Color) {
            placeInParentContainer(data.Name, data.Id, "#" + data.Color);
        }
    };

    function detailsFail(data) {
        alert("Грешка во апликацијата. Ве молиме контактирајте го администраторот, ако грешката продолжи да се случува.");
    };

    // Locations Markup Creation  ====================================
    // ##LOC_MARKUP##   (###)
    //Creates  markup for a passed in location to be displayed on the locaiton canvas.
    function getLocationMarkup(location) {

        var locationMarkup = $("<div></div>").addClass("element");

        var locationMarkupCalc = GeoUtility.CheckPadding(locationMarkup);

        // Get a link to the location details
        // This might be not used depending on the options passed by the calling script
        var detailsLink = getDetailsLink(location.Id);

        //create and set the type box on the element
        var typeBox = GeoMarkup.Utility.Wrapper().addClass("searchedTypeBox");
        typeBox.css("background-color", "#" + location.Color);

        locationMarkupCalc.append(typeBox);

        // Crete the content markup element containeing the name of the location
        var contentMarkup = $("<div></div>").addClass("content");
        contentMarkup.append(GeoMarkup.Utility.Padding());
        var contentMarkupCalc = GeoUtility.CheckPadding(contentMarkup);

        //Create the name box
        var nameBox = GeoMarkup.Utility.Wrapper().addClass("searchedNameBox").html(location.Name.toLowerCase());
        contentMarkupCalc.append(nameBox);

        //append the content markup to  location markup

        // Create the expand - explore link markup element
        var expandMarkup = $("<div></div>").addClass("expand");
        expandMarkup.append(GeoMarkup.Utility.Padding());
        var expandMarkupCalc = GeoUtility.CheckPadding(expandMarkup);

        var exploreLink = $("<div></div>").addClass("exploreLink");
        exploreLink.hover(exploreLinkHIn, exploreLinkHOut);
        expandMarkupCalc.append(exploreLink);

        // Check if the elements should be wrapped  in a link that will take it to the details page
        if (links) {
            detailsLink.append(contentMarkup);
            detailsLink.append(GeoMarkup.Utility.Clear());
            locationMarkupCalc.append(detailsLink);
        }
        else {
            locationMarkupCalc.append(contentMarkup);
        }

        if (allowExpansion) {
            // Check if the location has children to decide if the explore-markup should be added
            if (location.HasChildren) {
                locationMarkupCalc.append(expandMarkup);
            }
            else {
                contentMarkup.css("width", 170);
            }
        }
        // Combine the elements and return the final box
        return locationMarkup;
    };

    function exploreLinkHIn(event) {}

    function exploreLinkHOut(event) {}

    function getDetailsLink(id) {
        var url = "/" + controller + "/" + action + "/" + id;
        var $detailsLink = $("<a href=" + url + "></a>").addClass("detailsLink");
        return $detailsLink;
    };

    // Wraps the location markup in a list item for the main location list in the main panel.
    function getListItemForLocation(element, locationId, indexId) {
        var listItem = $("<li></li>").attr("id", "box" + indexId).attr("tag", locationId).addClass("listChild").append(element);
        listItem.append(GeoMarkup.Utility.Clear());
        return listItem;
    };

    // Creates markup for the expaned location node list.
    function getListItemForExpandedLocation(content, locationId, color) {
        var expandedItem = $("<li></li>").attr("tag", locationId).attr("data-color", color).addClass("stripBreadcrumbsElement").append(content);
        expandedItem.click(expandedNodeClick);
        return expandedItem;
    };

    // Other Global Functions  ====================================
    function deselectLocation() {
        $(".element").removeClass("selectedLocation");
    };

    return {
        Init: init,
        Deselect: deselectLocation,
        SetDrawLocationsHook: setDrawLocationsHook,
        SetExpandednodeClickHook: setExpandedNodeClickHook,
        SetDisplayedLocationClickHook: setDisplayedLocationClickHook,
        SetDataProvider: setDataProvider,
        PerformDisplay: performDisplay
    };
})();
