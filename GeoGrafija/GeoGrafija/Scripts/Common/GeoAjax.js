//  Client Framework for making Ajax Calls
var GeoAjax = (function () {

    // Keep track of the curently masked element
    var lastMaskedObject;

    function init() {
    };

    function makeAjaxPost(where, data, success, fail) {
        //Transform the data to json format: 
        var jsonData;

        if (data) {
            jsonData = data;
        } else {
            jsonData = "";
        }

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: jsonData,
            url: where,
            dataType: 'json',
            success: success,
            error: fail
        });
    };

    function makeSimpleJsonParam(data, name) {
        var dat = {};
        dat[name] = data;
        return JSON.stringify(dat);
    };

    function getUrlForAction(controller, action) {
        return "/" + controller + "/" + action;
    };

    //To be used for adding multiple values in  a json object for sending to server.
    function jsonDataArray() {
        var values = new Array();

        function getJsonObject() {
            //iterate through the array and build the pure json;
            var json = {};
            var i;
            for (i = 0; i < values.length; i++) {
                for (var key in values[i]) {
                    json[key] = values[i][key];
                }
            }
            return JSON.stringify(json);
        };

        function addParameter(name, data) {
            var d = {};
            d[name] = data;
            values.push(d);
        };

        return {
            AddValue: addParameter,
            GetJsonObject: getJsonObject
        };
    };

    // Refactor top using prototype


    // Show loadmask loading wrapper
    // --- query can be jquery object or string that will then be used 
    //      to get a jquery object
    // --- text is optional to specify the text to appear on the loading bar.
    //      default is Почекајте
    // Example calls 

    function showLoading(query,text){
        var objectToMask;
        var textToShow;
        // Check the query passed in parameter
        if(typeof(query) == 'object'){
            objectToMask  = query;

            // make sure it is jquery object 
            if(objectToMask.size ==  undefined ||  objectToMask.size() == 0){
                // if it is not then we default pick body
                objectToMask = $("#Wrapper");
            }
        } else if(typeof(query) == 'string'){
            objectToMask = $(query);

            // Check if we have non empty jquery object
            if(objectToMask.size ==  undefined ||  objectToMask.size() == 0){
                // if it is not then we default pick #Wrapper
                objectToMask = $("#Wrapper");
            }
        }
        else{
            // if it is not then we default pick #Wrapper
            objectToMask = $("#Wrapper");
        }

        // Check the text parameter
        if(typeof(text) !== 'stirng'){
            textToShow = "Почекајте...";
        }
        else{
            textToShow = text;   
        }

        // make the call to the actual loadmask plugin
        lastMaskedObject = objectToMask;
        objectToMask.mask(textToShow);
    }

    function stopLoading(query){
        var objectToRemoveMask;
        
        // Check the query passed in parameter
        if(typeof(query) == 'object'){
            objectToRemoveMask  = query;
        } else if(typeof(query) == 'string'){
            objectToRemoveMask = $(query);

            // Check if we have non empty jquery object
            if(objectToRemoveMask.size ==  undefined ||  objectToRemoveMask.size() == 0){
                removeLastMasked();
                return false;
            }
        }
        else{
            removeLastMasked();
            return false;
        }

        if(!objectToRemoveMask.isMasked()){
            return true;
        }
        else{
            objectToRemoveMask.unmask();
        }
    }
    // Private  function that removes the mask from the last masked element
    // used when user calls unmask with invalid query
    function removeLastMasked(){
        if(lastMaskedObject == undefined && lastMaskedObject == null){
            return false;
        }

        if(lastMaskedObject.isMasked()){
            lastMaskedObject.unmask();
            return true;
        }
        else{
            return true;
        }
    }


    //The public part of the Ajax APi
    return {
        MakeAjaxPost: makeAjaxPost,
        SimpleAjaxParam: makeSimpleJsonParam,
        GetUrlForAction: getUrlForAction,
        JsonDataArray: jsonDataArray,
        StartLoading : showLoading,
        StopLoading : stopLoading
    };
})();