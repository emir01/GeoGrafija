(function () {
    $(document).ready(documentReady);
    
    function documentReady() {
        GeoSearch.Initialize();
        GeoDisplay.SetDataProvider(GeoSearch);
        GeoDisplay.Init({ canvas: $("#canvas"), parent: $("#parent"), strip: $("#strip"), search: $("#searchBox") }, "Пребарување Локации", false, { links: true
                                                                                                                                                    ,selectable: false
                                                                                                                                                    ,search: true
                                                                                                                                                    ,contextSearch: true
                                                                                                                                                    ,searchHelp: true
                                                                                                                                                    }
                        );
    };
})();