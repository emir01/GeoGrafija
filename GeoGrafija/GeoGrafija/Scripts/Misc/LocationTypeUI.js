(function () {
    $(document).ready(function () {

        GeoAjax.MakeAjaxPost(GeoAjax.GetUrlForAction("Search", "GetJsonLTypes"), null, ajaxSuccess, ajaxFail);

    });

    function ajaxSuccess(data) {
        //get if should be link or just legend
        var shouldBeLink = $("#MakeTypeLinks").val();
        if (typeof (shouldBeLink) === 'undefined') {
            shouldBeLink = true;
        }

        $(data).each(function () {
            var item = $("#locationTypeListTemplate").clone().removeAttr("id").css("display", "block");
            $(".content", item).html(this.LocationTypeName);
            if (shouldBeLink) {
                $("a.content", item).attr("href", "/Search?typeId=" + this.Id);
            }
            else {
                $("a.content", item).attr("href", "#");
                $("a.content", item).click(typeClick);
            }

            $(".color", item).css("background-color", "#" + this.Color);
            $(".locationTypeList").append(item);
        });
        $(".locationTypeList").append(GeoMarkup.Utility.Clear());
    };

    function ajaxFail(data) {
        alert("Грешка во апликацијата. Ве Молиме контактирајте го администраторот.");
    };

    function typeClick(event) {
        return false;
    }
})();