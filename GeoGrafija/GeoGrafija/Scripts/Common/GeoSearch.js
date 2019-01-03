//  The framework under which the UI for searching operates.
//  Dependant on JQuery and JQuery UI libraries.

var GeoSearch = (function () {

    //Stores the retrieved data/message from the server
    var data;

    //Stores and utility object on the previous serach
    var searchUtilObject;

    // private variables, where should we pass search information
    // for global search.
    var searchController;
    var searchAction;

    // private variables, where should we pass search information
    // for details info retrieval via ajax.
    var detailsController;
    var detailsAction;

    // Global variable for module.
    // If ture we should look to query string to find probably passed tipe of locations
    var firstCall = true;

    // Search Options : 
    var typeFilter = false;
    var parentFilter = false;

    var typeFilterElementId = "";
    var parentFilterElementId = "";

    // Initialize the GeoSearchModule passing values for the controller and action from
    // where to retrieve the data.
    function init(controllerName, actionName, options) {

        if (controllerName) {
            if (typeof controllerName == "string") {
                searchController = controllerName;
            }
            else {
                parseOptions(controllerName);
                searchController = "Search";
            }
        }
        else {
            searchController = "Search";
        }

        if (actionName && typeof actionName == "string") {
            searchAction = actionName;
        }
        else {
            searchAction = "AjaxFindLocations";
        }

        if (options && typeof options == "object") {
            parseOptions(options);
        }

        detailsController = "Search";
        detailsAction = "LocationInfo";
    };

    function parseOptions(options) {

        // Check for type filter constrain.
        if (options.hasOwnProperty("typeFilter")) {
            typeFilter = options.typeFilter;
        }

        if (options.hasOwnProperty("typeFilterElementId")) {
            typeFilterElementId = options.typeFilterElementId;
        } else {
            typeFilter = false;
        }

        // Check for parent filter constrain.
        if (options.hasOwnProperty("parentFilter")) {
            parentFilter = options.parentFilter;
        }

        if (options.hasOwnProperty("parentFilterElementId")) {
            parentFilterElementId = options.parentFilterElementId;
        } else {
            parentFilter = false;
        }
    }

    // Hook Events ====================================

    var successHook;
    var failHook;

    var detailsSuccessHook;
    var detailsFailHook;

    function setSucessHook(hook) {
        successHook = hook;
    };

    function setFailHook(hook) {
        failHook = hook;
    };

    // Main Search Calls====================================
    function getLocations(searchOptions) {
        var locationName = extractSearchValue(searchOptions, "name", "string");
        var locationType = getSpecialLocationType(searchOptions);
        var parentId = getSpecialParentId(searchOptions);
        var zoom = extractSearchValue(searchOptions, "zoom", "string");
        var zoomTolerance = extractSearchValue(searchOptions, "zoomTolerance", "string");
        var locationId = extractSearchValue(searchOptions,"locationId", "string");

        // create the search util object
        searchUtilObject = getSearchUtilObject(parentId, locationName, locationType);

        //Create the json data object.
        var jsonData = new GeoAjax.JsonDataArray();

        jsonData.AddValue("locationName", locationName);
        jsonData.AddValue("locationType", locationType);
        jsonData.AddValue("parentId", parentId);
        jsonData.AddValue("zoom", zoom);
        jsonData.AddValue("zoomTolerance", zoomTolerance);
        jsonData.AddValue("locationId", locationId);
        
        var jsonObject = jsonData.GetJsonObject();

        // Make the call
        GeoAjax.MakeAjaxPost(GeoAjax.GetUrlForAction(searchController, searchAction), jsonObject, success, fail);
    };

    var successOnDetails;
    var failOnDetails;

    function getLocationDetails(locationId, success, fail) {
        if (!locationId) {
            alert("Must Send the location Id Callback Function");
            return;
        }

        if (!success) {
            alert("Must Send Success Callback Function");
            return;
        }

        if (!fail) {
            alert("Must Send Fail  Callback Function");
            return;
        }

        successOnDetails = success;
        failOnDetails = fail;

        var url = GeoAjax.GetUrlForAction(detailsController, detailsAction);
        var jsonIdParam = GeoAjax.SimpleAjaxParam(locationId, "locationId");

        GeoAjax.MakeAjaxPost(url, jsonIdParam, success, fail);
    };

    function successOnDetailsSetExtras(data) {
        successOnDetails(data, firstCall);
    };

    function failOnDetailsSetExtras(data) {
        failOnDetails(data);
    };

    function getSearchUtilObject(seachedId, searchedName, seachedType) {
        var object = {};

        if (seachedId) {
            object.SearchedId = seachedId;
        }
        else {
            object.SearchedId = "";
        }

        if (searchedName) {
            object.SearchedName = searchedName;
        }
        else {
            object.SearchedName = "";
        }

        if (seachedType) {
            object.SearchedType = seachedType;
        }
        else {
            object.SearchedType = "";
        }

        object.FirstCalled = firstCall;
        firstCall = false;

        return object;
    };

    // the success function success callback
    // this calls the success Hook on the Display Module passing in the data or/and extra information about the search 
    // and how it was performed
    function success(msg) {
        data = msg;
        // set the extra data on the uitl object and send the util object to the display module
        searchUtilObject.Data = msg;

        successHook(msg, searchUtilObject);
    };

    // this is the fail callback for the global serach call. Calls the Fail hook on the global display module
    // so it can display the fail information and redirect th euser to more useful operations
    function fail(msg) {
        data = msg;
        searchUtilObject.Data = msg;

        if (failHook) {
            failHook(msg);
        }
    };

    // Used to get details for a single locaiton from a url.
    // The url returns a json result containting multiple properties among which an array to create
    // the strip of the locaiton hirearchy of which the locatio with the given id is a part of.
    function getSingleLocationDetails(id, successHook, failHook) {
        // Set the success hook
        if (successHook) {
            detailsSuccessHook = successHook;
        }
        else {
            //Big trouble if nothing is passed
            alert("No callback function passed for Details Success Hook");
        }

        // Set the fail hook
        if (failHook) {
            detailsFailHook = failHook;
        }
        else {
            //Big trouble if nothing is passed
            alert("No callback function passed for Details Fail Hook");
        }

        //prepare parameters for the json post
        var url = GeoAjax.GetUrlForAction(detailsController, detailsAction);
        var jsonIdParam = GeoAjax.SimpleAjaxParam(id, "locationId");

        GeoAjax.MakeAjaxPost(url, jsonIdParam, successHook, failHook);
    };

    // Helper methods to retrieve passed in params in the searchOptions ====================================
    function getSpecialLocationType(searchOptions) {
        var locationType = "";

        if (firstCall) {
            locationType = GeoUtility.GetParamByName("typeId");
        }

        if (locationType && locationType != "") {
            return locationType;
        }

        if (typeFilter) {
            locationType = getLocationTypeFromFilter(searchOptions);
        }

        if (locationType && locationType != "") {
            return locationType;
        }

        return extractSearchValue(searchOptions, "type", "string");
    };

    function getLocationTypeFromFilter(searchOptions) {
        // get it from a value field on the element that was passed
        // along with the special filter option.
        var element = $("#" + typeFilterElementId);

        if (element.val() && element.val() != "") {
            return element.val();
        }
        else {
            return extractSearchValue(searchOptions, "type", "string");
        }
    }

    // Returns a parent  id from the search options.
    // Checks if this is a first call and if maybe a parent id 
    // has been passed via the query string.
    // If so that means the seach page has been called from outsite.
    // We make that location to be the parent location 
    // and display its child locations on the canvas.
    function getSpecialParentId(searchOptions) {
        var parentId = "";

        if (firstCall) {
            parentId = GeoUtility.GetParamByName("parentId");
        }

        if (parentId && parentId != "") {
            return parentId;
        }

        if (parentFilter) { // Check if special parent Filter has been applied.
            parentId = getParentIdFromFilter(searchOptions);
        }

        if (parentId && parentId != "") {
            return parentId;
        }

        return extractSearchValue(searchOptions, "parentId", "string");
    };

    function getParentIdFromFilter(searchOptions) {
        // get it from a value field on the element that was passed
        // along with the special filter option.
        var element = $("#" + parentFilterElementId);

        if (element.val() && element.val() != "") {
            return element.val();
        }
        else {
            return extractSearchValue(searchOptions, "parentId", "string");
        }
    }

    function extractSearchValue(collection, name, type) {
        if (!collection)
            return emptySearchValue(type);

        if (collection[name]) {
            return collection[name];
        }
        else {
            return emptySearchValue(type);
        }
    };

    function emptySearchValue(type) {
        switch (type) {
            case "string":
                return "";
                break;
            case "number":
                return 0;
                break;
            default:
                return null;
        }
    };
    
    return {
        Initialize: init,
        GetLocations: getLocations,
        Data: data,
        SetSuccessHook: setSucessHook,
        SetFailHook: setFailHook,
        GetSingleLocationDetails: getSingleLocationDetails,
        SearchUtilityObject: searchUtilObject,
        GetLocationDetails: getLocationDetails
    };
})();