// ===   ------------------------------------------------------------------
// ===   Sub Module that handles Specific functionality for student profile
// ===   ------------------------------------------------------------------

var ProfileStudent = (function () {
    // ===   ------------------------------------------------------------------
    // ===   Configuration variables and fields
    // ===   ------------------------------------------------------------------

    // ===   ------------------------------------------------------------------
    // ===   Functions and methods
    // ===   ------------------------------------------------------------------
    function init() {
        setupEvents();
        setupUi();
    }

    function handleAction(actionId) {
        if (actionId == 5) {
            return showStudentHelp();
        }
    }

    function showStudentHelp(){

        var content  =  $("#StudentProfileHelpWrapper");

        var clonedContent  = content.clone();
        clonedContent.removeClass("hidden");

        GeoUtility.SetupHelpDialogContents(clonedContent);

        var dialog = GeoDialogFactory.InformationDialog("StudentProfileHelpDialog","Помош - Профил за студент", clonedContent, 600,600);
        dialog.Open();

        GeoUtility.SetupHelpDialogContents(dialog);
        clonedContent = null;

        return false;
    }

    function studentTakeQuiz() {

    }

    function setupEvents() {
        $("#SaveNewTeacherChoise").click(saveTeacherChoice);

        $("#QuizResultWrapper .getMoreQuizResult").live("click", getResultDetails);
    }

    function setupUi() {
        var percentageOfProgress = $("#StudentQuizProgressValue").val();
        $("#RankProgress").css("width", percentageOfProgress);

        setupNumberOfStars();
    }

    function setupNumberOfStars(){
        var numberOfStars = $("#RankOrder").val();
        GeoUtility.log(numberOfStars);
        var i;

        var starsLeft = $("#StarsLeft");
        var starsRight = $("#StarsRight")

        starsLeft.empty();
        starsRight.empty();

        //clear stars
        starsLeft.html()

        // clone this when adding the stars
        var star = $("<div></div>").addClass("rankStar");

        if(numberOfStars===undefined || numberOfStars === null || numberOfStars === 0 ){
                numberOfStars = 1;
        }

        for(i = 1 ; i <= numberOfStars;i++){
            starsLeft.append(star.clone());
            starsRight.append(star.clone());
        }
    }
    // ===   ------------------------------------------------------------------
    // ===   Event handlers
    // ===   ------------------------------------------------------------------

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

        GeoAjax.StartLoading("body");
        GeoAjax.MakeAjaxPost(url, data, resultGetSuccess, resultGetFail);

        return false;
    }

    function resultGetSuccess(resultModel) {
        GeoAjax.StopLoading();
        var dialog,
            informationMarkup;

        if (resultModel.IsOk) {
            informationMarkup = GeoMarkup.QuizMarkup.TakeQuiz.ResultMarkup(resultModel);
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
        GeoAjax.StopLoading();
        var dialog;

        dialog = GeoDialogFactory.InformationDialog("DisplayQuizResultProfile", "Резултати-Неупех!", "Грешка при преземање на детални резултати!", 400, 200);
        dialog.Open();
        return false;
    }

    function saveTeacherChoice(event) {
        var teacherId = $('#ChosenProfessor option:selected').val(),
            dialog,
            controller = "User",
            action = "SetStudentTeacher",
            url,
            data;

        if (teacherId !== 'undefined' && teacherId != "") {
            data = GeoAjax.SimpleAjaxParam(teacherId, "teacherId");
            url = GeoAjax.GetUrlForAction(controller, action);

            GeoAjax.StartLoading();
            GeoAjax.MakeAjaxPost(url, data, changeProfSuccess, changeProfFail);
        }
        else {
            dialog = GeoDialogFactory.InformationDialog("MustPickProfesorToChange", "Порака!", "Мора да изберете професор за промена!", 400, 200);
            dialog.Open();
        }
    }

    function changeProfSuccess(data) {
        GeoAjax.StopLoading();
        var dialog;

        if (data.ChangeOk) {
            // Change the value of the prof name
            if (data.NewTeacherName !== 'udnefined' && data.NewTeacherName != "") {
                $(".value", "#CurrentTeacherInformation").html(data.NewTeacherName);
            }
            else {
                dialog = GeoDialogFactory.InformationDialog("ServerFailProfChangeNoName", "Порака!", "Неуспешна промена на професор!Обиди се повторно!", 400, 200);
                dialog.Open();
                return;
            }

            dialog = GeoDialogFactory.InformationDialog("SuccessProfChange", "Порака!", "Успешна промена на професор!", 400, 200);
            dialog.Open();
            return;

        } else {
            dialog = GeoDialogFactory.InformationDialog("ServerFailProfChange", "Порака!", "Неуспешна промена на професор!Обиди се повторно!", 400, 200);
            dialog.Open();
            return;
        }

    }

    function changeProfFail(data) {
        GeoAjax.StopLoading();
        var dialog;

        dialog = GeoDialogFactory.InformationDialog("SuccessProfChange", "Порака!", "Неуспешна промена на професор!Обиди се повторно!", 400, 200);
    }

    // ===   ------------------------------------------------------------------
    // ===   Utilities
    // ===   ------------------------------------------------------------------

    return {
        Initialize: init,
        HandleAction: handleAction
    };
})();