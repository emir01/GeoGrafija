var GeoUi = (function () {

    // === ---------------------------------------------------------------------------
    // ===  Basic Configuration values for UI ----------------------------------------
    // === ---------------------------------------------------------------------------

    //resource section parameters
    var defaultSectionsPerRow = 3;
    var defaultSectionsExtraMargin = 5;

    // === ---------------------------------------------------------------------------
    // ===  Methods  -----------------------------------------------------------------
    // === ---------------------------------------------------------------------------

    function init() {
        resizeContent();

        $(window).resize(resizeContent);
        $(document).resize(resizeContent);

        intervalResize();

        setupHintBoxes();
        setupMapSearchBoxes();
        resizeSections();

        setupInputColorPickers();
        setupDisplayColorBoxes();

        setupDropKick();
        setupDataTables();
        setupMenuIcons();

        setSelectedPage();

        setupBackToTopBox();

        // Preload all general images
        var images = [
            "/Content/css/Mask/images/loading.gif",
            "/Content/images/warning.png",
            "/Content/images/stop.png"
        ];
        

        preload(images);
    };


    function preload(arrayOfImages) {
        $(arrayOfImages).each(function(){
            $('<img/>')[0].src = this;
            // Alternatively you could use:
            // (new Image()).src = this;
        });
    }

    function setupBackToTopBox() {
        $(window).scroll(function () {
            if ($(this).scrollTop() != 0) {
                $('#toTop').fadeIn();
            } else {
                $('#toTop').fadeOut();
            }
        });

        $('#toTop').click(function () {
            $('body,html').animate({ scrollTop: 0 }, 800);
        });
    }

    function setSelectedPage() {

        var hiddenIndicator = $("#SelectedPage");
        var selectedValue = hiddenIndicator.val();

        $("#menu a").removeClass("activePage");

        $("#menu a").filter(function () {
            return $(this).text() == selectedValue;
        }).addClass("activePage");

    }

    function setupMenuIcons() {
        //get all icons
        var allIcons = $(".icon");
        //center the icons
        allIcons.each(function (index, callbacl) {
            var jIcon = $(this);
            var image = jIcon.attr("data-image");

            jIcon.css("background-image", "url('/Content/images/MenuIcons/" + image + "')");

            var li = jIcon.closest("li");

            if (li.size() == 0) {
                li = jIcon.closest(".profileActionBoxItem");
            }

            var liWidth = li.innerWidth();
            var liHeight = li.height();
            var iconWidth = jIcon.width();

            var push = (liWidth / 2) - (iconWidth / 2);
            jIcon.css("left", push);
            jIcon.css("bottom", liHeight + 5);

        });

        $("#menu").on("mouseover", "a", mouseOverMenuOption);
        $("#menu").on("mouseout", "a", mouseOutMenuOption);

        $(".profileActionBoxText").on("mouseover", mouseOverMenuOption);
        $(".profileActionBoxText").on("mouseout", mouseOutMenuOption);
    }

    function mouseOverMenuOption(event) {
        var li = $(this);
        var icon = li.siblings(".icon");

        // stop all previous animations
        var allIcons = $(".icon");
        allIcons.stop(true,true);

        if (icon.size() == 0) {
            icon = li.find(".icon");
        }
        icon.fadeIn(100);
    }

    function mouseOutMenuOption(event) {
        var li = $(this);
        var icon = li.siblings(".icon");
        
        // stop all previous animations
        var allIcons = $(".icon");
        allIcons.stop(true,true);

        if (icon.size() == 0) {
            icon = li.find(".icon");
        }
        icon.fadeOut(100);
    }

    function setupDropKick() {
        $(".dropKickEnabled").dropkick();
    }

    function setupDataTables() {
        $('.dataTablesEnabled').dataTable({
            "bJQueryUI": true,
            "sPaginationType": "full_numbers",
            "oLanguage": getMkTableOptions()
        });
    }

    function getMkTableOptions() {
        var paginate = {
            "sLast": "Последна",
            "sFirst": "Прва",
            "sNext": "Следна",
            "sPrevious": "Предходна"
        };

        var options = {
            "sLengthMenu": "Прикажи  _MENU_ податоци на  страна",
            "sZeroRecords": "Не се најдени податоци - жалам!",
            "sInfo": "Прикажува од _START_ до _END_ од _TOTAL_ вкупно",
            "sInfoEmpty": "Прикажува 0 од 0 до 0 податоци",
            "sInfoFiltered": "(Филтрирано  од _MAX_ вкупно  податоци)",
            "sSearch": "Барај!",
            "oPaginate": paginate,
            "sEmptyTable": "Нема податоци!",
            "sLoadingRecords": "Вчитување на податоци - почекајте...",
            "sProcessing": "Податоците се моментално зафатени!",
            "sInfoPostFix": ""
        };

        return options;
    }

    function intervalResize() {
        resizeContent();

        setTimeout(function () {
            intervalResize();
        }, 500);
    }

    function setupDisplayColorBoxes() {
        $(".colorPickerDisplayOnlyPreview").each(function () {
            var boxPreview = $(this);
            var box = $(".colorPickerDisplayOnlyBox", boxPreview.parent());
            var colorValueHtml = box.html();

            boxPreview.css("background-color", "#" + colorValueHtml);
        });
    };

    // initializes color pickers, for form input editable fields.
    function setupInputColorPickers() {

        $(".colorPickerPreview").height($('.colorPickerTextBox').height());
        $(".colorPickerPreview").width($('.colorPickerTextBox').height());

        $('.colorPickerTextBox').each(function () {

            var box = $(this);
            var boxPreview = $(".colorPickerPreview", box.parent());

            var prevColor = box.val();
            var setColor;
            if (prevColor == "" || typeof prevColor == "undefined") {
                setColor = "FF0000";
            }
            else {
                if (prevColor.substr(0, 1) == "#") {
                    setColor = prevColor.substr(1, prevColor.length);
                }
                else {
                    setColor = prevColor;
                }
            }

            box.val("#" + setColor);
            boxPreview.css("background-color", "#" + setColor);

            box.ColorPicker({
                onChange: function (hsb, hex, rgb) {
                    box.val("#" + hex);
                    boxPreview.css("background-color", "#" + hex);
                },
                color: setColor

            });

            boxPreview.ColorPicker({
                onChange: function (hsb, hex, rgb) {
                    box.val("#" + hex);
                    boxPreview.css("background-color", "#" + hex);
                },
                color: setColor
            });
        });
    };

    // used to properly resize the resource tabs
    // calls a timer so a resize can be done if zoom
    function resizeSections(sectionsPerRow, sectionExtraMargin) {
        var calcSectionsPerRow, calcSectionExtraMargin; // calculated values if null param/consider default

        calcSectionsPerRow = GeoUtility.CalculateValue(sectionsPerRow, defaultSectionsPerRow);
        calcSectionExtraMargin = GeoUtility.CalculateValue(sectionExtraMargin, defaultSectionsExtraMargin);

        if ($(".resourceSeciton").size() <= 0) {
            return;
        }

        var margin = GeoMarkup.ResourceMarkup.Options.SECTION_LEFT_MARGIN
                    + GeoMarkup.ResourceMarkup.Options.SECTION_RIGHT_MARGIN;

        var resourceWrapper = $("#resourceContainer");
        var workingWrapper = resourceWrapper;

        if (workingWrapper.size() == 0) {
            workingWrapper = $("#LearnMoreResourceContainer");
            calcSectionExtraMargin = calcSectionExtraMargin - 15;
        }

        var paddingWrapper = $(".padding", workingWrapper).first();

        var paddingLeftValue = parseInt(paddingWrapper.css("padding-left"));
        var paddingRightValue = parseInt(paddingWrapper.css("padding-right"));

        var wrapperWidth = workingWrapper.width();
        var mainWidth = wrapperWidth - (paddingLeftValue + paddingRightValue);
        var barWidth = Math.round((mainWidth / calcSectionsPerRow) - (margin)) - (calcSectionsPerRow * 2) - calcSectionExtraMargin;

        $(".resourceSeciton").css("width", barWidth);
    };

    // Set the map search boxes to react on pressing key
    // corelated with submit action
    function setupMapSearchBoxes() {
        $("#mapContainer #tbox_address").keyup(function (event) {
            if (event.keyCode == 13) {
                $("#mapContainer #btn_search").click();
            }
        });
    };

    function setupHintBoxes() {
        // for each hint box on the page store the hint
        // which would be at the begging set as value in a data atribute
        // so we can work with it later
        $(".hintBox").each(function (idnex, element) {
            var $box = $(element);
            $box.attr("data-hint", $box.val());
            $box.focusin(hintGotBoxFocus);
            $box.focusout(hintBoxLostFocust);
        });
    };

    function hintGotBoxFocus(event) {
        var $box = $(this);
        // if it has the hint box class remove the class 
        // and clear the value so you can type
        if ($box.hasClass("hintBox")) {
            $box.removeClass("hintBox");
            $box.val("");
        }
        else {
            $box.select();
        }
    };

    function hintBoxLostFocust(event) {
        var $box = $(this);

        // if no value was entered then we return the hintbox class
        // and the text stored in data-hint
        if ($box.val().trim() == "") {
            $box.addClass("hintBox");
            $box.val($box.attr("data-hint"));
        }
    };

    function resizeContent(event) {

        var headerHeight = $("#pageHeader").outerHeight();
        var headerBorderHeight = $("#HeaderBorder").outerHeight();
            
        var windowHeight = $(window).height();
        var mainBottomMargin = parseInt($("#Main").css("margin-bottom"));
        var mainHeight = $("#Main").height() + mainBottomMargin;

        var calc = ((windowHeight - headerHeight)) - headerBorderHeight;

        $("#Content").css("min-height", calc + "px");
        $("#sideSection").css("min-height", calc + "px");


        var resizeMain = $("#Main").hasClass("resizeMe");

        if ((resizeMain && resizeMain == true) || $("#ResizeMain").size() > 0) {
            $("#Main").css("min-height", calc + "px");
        }
    };

    function resetHintBoxes() {
        $(".hintBox").each(function (idnex, element) {
            var $box = $(element);
            if (!$box.attr("data-hint")) {
                $box.attr("data-hint", $box.val());
                $box.focusin(hintGotBoxFocus);
                $box.focusout(hintBoxLostFocust);
            }
        });
    };

    function resizeExplorationMap() {}

    return {
        Initialize: init,
        ResetHintBoxes: resetHintBoxes,
        ResizeSections: resizeSections,
        ResizeContent: resizeContent,
        ResizeExplorationMap: resizeExplorationMap,
        Preload:preload
    };
})();


