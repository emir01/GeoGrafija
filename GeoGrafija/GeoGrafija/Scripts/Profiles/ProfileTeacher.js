// ===   ------------------------------------------------------------------
// ===   Sub Module that handles Specific functionality for student profile
// ===   ------------------------------------------------------------------

var ProfileTeacher = (function () {
    // ===   ------------------------------------------------------------------
    // ===   Configuration variables and fields
    // ===   ------------------------------------------------------------------

    // ===   ------------------------------------------------------------------
    // ===   Functions and methods
    // ===   ------------------------------------------------------------------
    function init() {
        initEventHandlers();

        initClassroomCreator();
    }

    function initEventHandlers() {

        $(".studentResultsActions").on("click", showStudentResultsAction);

        $(".getMoreQuizResult").on("click", getResultDetails);
    }
    
    function initClassroomCreator() {
        GeoClassroom.InitializeTeacher();
    }

    function handleAction(actionId) {
        if(actionId==3){
            return showTeacherHelp();
        }
    }

    function showTeacherHelp(){
        var content  =  $("#TeacherProfileHelpWrapper");
        var clonedContent  = content.clone();
        clonedContent.removeClass("hidden");

        GeoUtility.SetupHelpDialogContents(clonedContent);

        var dialog = GeoDialogFactory.InformationDialog("TeacherProfileHelpDialog","Помош - Профил за наставник", clonedContent, 600,600);
        dialog.Open();

        GeoUtility.SetupHelpDialogContents(dialog);
        clonedContent = null;

        return false;
    }

    // ===   ------------------------------------------------------------------
    // ===   Event handlers
    // ===   ------------------------------------------------------------------

    function showStudentResultsAction(event) {
        var studentName,
            studentNameTd,
            row;

        row = $(this).closest("tr");
        studentNameTd = row.children("td").first();

        studentName = studentNameTd.children(".hiddenValidInput").val();

        var studentResultsDialogs = $(".studentResultsDialog");

        var requestedDialog = studentResultsDialogs.filter(function () {
            return $(this).attr("data-student-name") == studentName;
        });

        var height = 600;

        if ($(".quizResultListItem", requestedDialog).size() == 0) {
            height = 200;
        }

        requestedDialog.dialog({
            title: "Резултати за " + studentName,
            width: 600,
            height: height,
            modal: true,
            autoOpen: true,
            open: function () {

            },
            close: function () {

            },
            buttons: {
                "Затвори": function () {
                    $(this).dialog("close");
                }
            }
        });

        return false;
    }

    function getResultDetails(event) {
        var controller,
            action,
            resultId,
            data,
            url;

        controller = "User";
        action = "GetStudentQuizResultDetails";

        resultId = $(this).attr("data-resultid");

        data = GeoAjax.SimpleAjaxParam(resultId, "resultId");
        url = GeoAjax.GetUrlForAction(controller, action);

        GeoAjax.MakeAjaxPost(url, data, resultGetSuccess, resultGetFail);

        return false;
    }

    function resultGetSuccess(resultModel) {
        var dialog,
            informationMarkup;

        if (resultModel.IsOk) {
            informationMarkup = GeoMarkup.QuizMarkup.TakeQuiz.ResultMarkup(resultModel, getResultMarkupOptionsObject());
            dialog = GeoDialogFactory.InformationDialog("DisplayQuizResultProfile", "Резултати!", informationMarkup, 600, 600);
            dialog.SetContents(informationMarkup);
            dialog.Open();
            return false;
        } else {
            dialog = GeoDialogFactory.InformationDialog("DisplayQuizResultProfile", "Резултати-Неупех!", resultModel.Message, 400, 200);
            dialog.Open();
            return false;
        }
    }

    function resultGetFail(resultModel) {
        var dialog;

        dialog = GeoDialogFactory.InformationDialog("DisplayQuizResultProfile", "Резултати-Неупех!", "Грешка при преземање на детални резултати!", 400, 200);
        dialog.Open();
        return false;
    }

    // ===   ------------------------------------------------------------------
    // ===   Utilities
    // ===   ------------------------------------------------------------------

    function getResultMarkupOptionsObject() {
        var options = {};

        options.title = "Резултати за студент!";
        options.studentAnswerHeader = "Студентот Одговорил:";

        return options;
    }

    return {
        Initialize: init,
        HandleAction: handleAction
    };
})();