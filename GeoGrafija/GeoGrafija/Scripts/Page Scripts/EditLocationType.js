var EditLocationType = (function () {
    function init(){
        $(function () {
            $(".markerIconList li").click(markerIconClick);
            $("#pickedMarker").val("");
            setupRichTextEditor();
        });
    }

    function markerIconClick(event) {
        var thisHasClass = $(this).hasClass("selectedMarker");

        $(".selectedMarker").removeClass("selectedMarker");
        $("#pickedMarker").val("");

        if (!thisHasClass) {
            $(this).addClass("selectedMarker");
            $("#pickedMarker").val($(this).attr("data-file"));
        }
    };
    
    function setupRichTextEditor() {
        $("#TypeDescription").wysiwyg();
    }

    return{
        Initialize:init
    };
})();