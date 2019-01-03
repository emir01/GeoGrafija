(function () {
    $(document).ready(documentReady);
    function documentReady() {
        
        GeoSearch.Initialize({
                typeFilter:true,
                typeFilterElementId: "HiddenSelectedTypeId"
            });
       
        GeoDisplay.SetDataProvider(GeoSearch);
        GeoDisplay.Init({
                            canvas: $("#canvas"),
                            parent: $("#parent"),
                            strip: $("#strip"),
                            search: $("#searchBox")
                        },
                        "Пребарување Локации",
                        true,
                        {
                            links: true,
                            selectable: false,
                            search: true,
                            contextSearch: false,
                            searchHelp: false,
                            strip : false,
                            parent:false,
                            expansion:false,
                            emptySearch:true
                        }
                        );
                        GeoDisplay.SetDataProvider(GeoSearch);

                        GeoLearning.Initialize();
    }
})();