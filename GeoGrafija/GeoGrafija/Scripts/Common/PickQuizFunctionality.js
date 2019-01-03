// ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
// ===   Sub-Script-Module used in part in the taking quiz process
// ===   This module is used to organize the picking and filtering  of the quizes funcitonality
// ===   ------------------------------------------------------------------------------------------------------------------------------------------------------

var PickQuizFunctionality = (function () {

    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    // ===   Module variables parameters and configuration  -------------------------------------------------------------------------------------------------------
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    var allPredefQuizes,
        filteredPredefQuizes;
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    // ===   Module functions and methods  ------------------------------------------------------------------------------------------------------------------------
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    function init() {
        initEventHandlers();
    }

    function initEventHandlers() {
        // set up the event handlers for the buttons
        $("#RandomQuizButton").click(randomButtonClick);
        $("#PredefinedQuizButton").click(predefButtonClick);
        $("#StartPredefQuiz").click(startPredefQuizClick);
    }

    // ===   ------------------------------------------------------------------------
    // ===   Event Handlers --------------------------------------------------------------
    // ===   ------------------------------------------------------------------------
    // event hander for the take random quiz button
    function randomButtonClick(event) {
        // reroute to proper aciton on the controller that will generate 
        // and start the random quiz
        startQuiz(0);
    }

    // event handler for the show predef quizes button
    function predefButtonClick(event) {
        var predefQuizes,
            url,
            data,
            getPredefQuizesController = "TakeQuiz",
            getPredefQuizesAction = "PredifnedQuizes";

        // show pick predef quizes form
        $("#AvailableDefinedQuizesForm").slideDown();

        //make call and get check student id
        url = GeoAjax.GetUrlForAction(getPredefQuizesController, getPredefQuizesAction);
        data = GeoAjax.SimpleAjaxParam("", "void");

        GeoAjax.StartLoading("body");
        GeoAjax.MakeAjaxPost(url, data, getPredefSuccessCallback, getPredefFailCallback);
    }

    // event handler for ajax call to get predef quizes
    function getPredefSuccessCallback(msg) {
        GeoAjax.StopLoading();
        var predefQuizList,
            calcPrederQuizList;

        //clear prev quizes
        $(".predefQuizesContainer").html("");
        //get the predefined quiz list from markup and add to page.
        predefQuizList = GeoMarkup.QuizMarkup.PickQuiz.GetPickQuizList();
        $(".predefQuizesContainer").append(predefQuizList);
        calcPrederQuizList = GeoUtility.CheckPadding(predefQuizList);

        // get the js object from the json returned data.
        allPredefQuizes = msg;

        if (allPredefQuizes.AvailableQuizes.length > 0) {
            $.each(allPredefQuizes.AvailableQuizes, function (index, element) {
                var singleQuizMarkup;
                singleQuizMarkup = GeoMarkup.QuizMarkup.PickQuiz.GetPickQuizListItem(element);
                singleQuizMarkup.click(prederQuizClick);

                calcPrederQuizList.append(singleQuizMarkup);
            });
        }
        else {
            var noQuizMessage = $("<div class='noPredefQuizMessage'>Нема предефинарни квизови, кои не ги имате решено! Пробајте со генерирање на случаен квиз!</div>");
            calcPrederQuizList.append(noQuizMessage);
        }
    }

    // event callback from ajax post for failing to retrieve predef quizes
    function getPredefFailCallback(msg) {
        GeoAjax.StopLoading();
        var dialog;

        dialog = GeoDialogFactory.InformationDialog("GetPredefFailCall", "Порака!", "Грешка во прикажување на квизови. Обиди се повторно!", 400, 200);
        dialog.Open();
        return;
    }

    // event handler for clicking on a predef quiz in the list
    function prederQuizClick(event) {
        var id;
        //remove the selected class from all the predef quiz items
        $(".predefQuizListItem").removeClass("predefSelectedQuiz");
        $(this).addClass("predefSelectedQuiz");
    }

    // the event handler for the start predef quiz button
    function startPredefQuizClick(event) {
        var selectedQuiz,
            selectedQuizId,
            dialog;

        //try and get the selected quiz
        selectedQuiz = $(".predefSelectedQuiz");

        //check if there is a selected quiz
        if (!selectedQuiz.size() === 1) {
            dialog = GeoDialogFactory.InformationDialog("MustSelectQuizToSTart", "Порака!", "Мора да изберете квиз пред да започнете!", 400, 200);
            dialog.Open();
            return;
        }

        // try and get the id of the quiz
        selectedQuizId = $(".hiddenQuizId", selectedQuiz).val();

        if (!selectedQuizId) {
            dialog = GeoDialogFactory.InformationDialog("MustSelectQuizToSTart", "Порака!", "Мора да изберете квиз пред да започнете!", 400, 200);
            dialog.Open();
            return;
        }

        //reroute to proper action to start quiz
        startQuiz(selectedQuizId);
    }
    // ===   ------------------------------------------------------------------------
    // ===   Utilities --------------------------------------------------------------
    // ===   ------------------------------------------------------------------------

    // does the proper action to start a quiz for the   user.
    // if the quiz id is 0 then the quiz to be strated is a random quiz
    function startQuiz(quizId) {
        var dialog,
            routeControl = "TakeQuiz", // The MVC controller that starts up the quiz
            routeAction = "TakingQuiz"; // The MVC controller action  that  starts up the quiz

        //reroute to proper action on the controller
        window.location = GeoUtility.GetUrlWithParam(routeControl, routeAction, "quizId", quizId);
    }

    return {
        Initialize: init
    };
})();