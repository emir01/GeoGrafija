(function () {
    $(document).ready(documentReady);

    function documentReady() {
        GeoSearch.Initialize();
        GeoSingleDisplay.SetDataProvider(GeoSearch);
        GeoSingleDisplay.Initialize();

        GeoSearch.Initialize({
            parentFilter: true,
            parentFilterElementId: "HiddenLocationID"
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
                            strip: false,
                            parent: false,
                            expansion: false,
                            emptySearch: true,
                            allowCrumbs: false
                        }
                        );
        GeoDisplay.SetDataProvider(GeoSearch);
        setupRichTextDisplay();
    };

    function setupRichTextDisplay() {
//        $("#ShortDescription").wysiwyg(
//            {
//                rmUnusedControls: true
//            });
        
        //$('#ShortDescription').replaceWith($('#ShortDescription').wysiwyg('getContent'));
    }

})();