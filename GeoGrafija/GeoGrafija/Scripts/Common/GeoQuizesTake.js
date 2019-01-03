// ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
// ===   Module used in displaying the specifics and handling  of the quiz taking ui  -------------------------------------------------------------------------
// ===   ------------------------------------------------------------------------------------------------------------------------------------------------------

var GeoQuizesTake = (function () {

    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    // ===   Module variables parameters and configuration  -------------------------------------------------------------------------------------------------------
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    var quiz; // the quiz that is beeing taken right now. Gotten via ajax
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    // ===   Module functions and methods  ------------------------------------------------------------------------------------------------------------------------
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------

    function initPicker() {
        //load the PickQuizFunc Script if not loaded
        $.getScript('Scripts/Common/PickQuizFunctionality.js', function (data, textStatus) {
            console.log(data); //data returned
            console.log(textStatus); //success
            console.log('Load was performed.');
            PickQuizFunctionality.Initialize();
        });
    }

    // The main initialization funciton for the taking quz part
    function mainInit() {
        // make call and get the quiz a
        var url,
            data,
            quizId,
            controllerForData = "TakeQuiz",
            actionForData = "GetQuiz";

        quizId = GeoUtility.GetParamByName("quizId");
        
        url = GeoAjax.GetUrlForAction(controllerForData, actionForData);

        data = GeoAjax.SimpleAjaxParam(quizId, "quizId");

        GeoAjax.StartLoading();
        GeoAjax.MakeAjaxPost(url, data, drawQuiz, failedToGetQuiz);

        $("#SubmitTestAnswers").click(submitAnswers);

        // initialize the helper 
        $("#TakingQuizHelpDialogStudent").on('click',function(){

            var content  =  $("#StudentTakingQuizHelpWrapper");

            var clonedContent  = content.clone();
            clonedContent.removeClass("hidden");

            GeoUtility.SetupHelpDialogContents(clonedContent);

            var dialog = GeoDialogFactory.InformationDialog("GeoQuizStudentTakeHelp","Помош - Тестови", clonedContent, 600,600);
            dialog.Open();

            GeoUtility.SetupHelpDialogContents(dialog);
            clonedContent = null;

            return false;
        })
    }

    //makes the draw upon succesfull get of quiz
    function drawQuiz(model) {
        GeoAjax.StopLoading();
        var dialog,
            wrapper,
            quizName,
            tmpQuestionMarkup,
            tmpAnswerMarkup,
            answersWrapper,
            i,
            j,
            maxQuestions,
            maxAnswers,
            tmpQuestion,
            tmpAnswer;

        // check if model is ok
        if (!model.IsOk) {
            dialog = GeoDialogFactory.InformationDialog("CouldNotGetQuizModel", "Порака!", "Грешка при прикажување на квиз. Обиди се повторно!</br> Квизот или не постои или сте го решиле!", 400, 200);
            var actualDialog = dialog.GetDialog();

            //add button to the dialog
            actualDialog.dialog( "option", "buttons", [{
                text:"Затвори и врати се на профил!",
                click:function(){
                    window.location = "/User/Home";
                }
            }]);

            actualDialog.bind( "dialogclose", function(event, ui) {
                window.location = "/User/Home";
            });

            dialog.Open();

            return;
        }

        // srart drawing the quiz;
        wrapper = GeoUtility.CheckPadding($("#QuestionFormsWrapper"));
        quizName = $(".takeQuizName");

        //set the quiz name
        quizName.html(model.QuizName);

        quiz = model; // the model is the quiz we want to display

        for (i = 0, maxQuestions = quiz.Questions.length; i < maxQuestions; i++) {
            //get the markup for the global question 
            tmpQuestion = quiz.Questions[i];
            tmpQuestionMarkup = GeoMarkup.QuizMarkup.TakeQuiz.QuestionWrapper(tmpQuestion);

            //add the answers to the wrapper;
            answersWrapper = $(".takeAnswersWrapper", tmpQuestionMarkup);
            for (j = 0, maxAnswers = tmpQuestion.Answers.length; j < maxAnswers; j++) {
                tmpAnswer = tmpQuestion.Answers[j];
                tmpAnswerMarkup = GeoMarkup.QuizMarkup.TakeQuiz.AnswerWrapper(tmpAnswer);

                tmpAnswerMarkup.click(answerClicked);
                tmpAnswerMarkup.hover(
                    function (event) {
                        if (!$(this).hasClass("selectedAnswer")) {
                            $(this).addClass("takeSingleAnswerWrapperHoverBackground");
                        }
                    },
                    function (event) {
                        $(this).removeClass("takeSingleAnswerWrapperHoverBackground");
                    });

                answersWrapper.append(tmpAnswerMarkup);
                answersWrapper.append(GeoMarkup.Utility.Clear());
            }
            wrapper.append(tmpQuestionMarkup);
        }
        setupUpQuizUi();
    }

    function failedToGetQuiz(model) {
        GeoAjax.StopLoading();

    }

    function setupUpQuizUi() {
        var allQuestionForms,
            startNumber = 1;

        allQuestionForms = $(".takeQuestionFormBackground");

        //setup the quiz number boxes
        setupQuizNumberBoxes(allQuestionForms);

        //set up the box numbers on the question forms
        allQuestionForms.each(function (index, element) {
            $(element).attr("data-box-number", startNumber);
            startNumber++;
        });

        setupFirstQuestion(allQuestionForms);

        // set up the navigation buttons
        setupNavButton();
    }

    function setupNavButton() {
        var next,
            previous;

        //setup the next button
        next = $("#NextQiestion");
        next.click(nextQuestionClick);

        //setup the previous button 
        previous = $("#PreviousQiestion");
        previous.click(previousQuestionClick);
        previous.addClass("disabledNavButton");
    }

    function setupQuizNumberBoxes(allQuestionForms) {
        var maxQuestions,
            questionNumbersList,
            i,
            tmpQuizNumberBoxMarkup;

        questionNumbersList = $(".questionBoxesList");
        // set up the current question trail  part;
        // create a ui to access all the questions
        for (i = 0, maxQuestions = allQuestionForms.size(); i < maxQuestions; i++) {
            tmpQuizNumberBoxMarkup = GeoMarkup.QuizMarkup.TakeQuiz.QuizBox(i + 1);

            questionNumbersList.append(tmpQuizNumberBoxMarkup);

            tmpQuizNumberBoxMarkup.click(quizNumberBoxClick);
            tmpQuizNumberBoxMarkup.hover(
                function (event) {
                    if (!$(this).hasClass("selectedQuestionNumber")) {
                        $(this).addClass("questionBoxListItemHover");
                    }
                },
                function (event) {
                    $(this).removeClass("questionBoxListItemHover");
                }
            );
        }

        questionNumbersList.append(getShowAllListItem());
        questionNumbersList.append(GeoMarkup.Utility.Clear());
    }

    function getShowAllListItem() {
        var tmpQuizNumberBoxMarkup;
        // get one last item that should display all the questions when clicked
        tmpQuizNumberBoxMarkup = GeoMarkup.QuizMarkup.TakeQuiz.QuizBox(0);
        tmpQuizNumberBoxMarkup.addClass("showAllQuestionsListItem");
        $(".questionNumber", tmpQuizNumberBoxMarkup).html("Сите Прашања!");

        tmpQuizNumberBoxMarkup.click(quizNumberBoxClick);
        tmpQuizNumberBoxMarkup.hover(
                function (event) {
                    if (!$(this).hasClass("selectedQuestionNumber")) {
                        $(this).addClass("questionBoxListItemHover");
                    }
                },
                function (event) {
                    $(this).removeClass("questionBoxListItemHover");
                }
            );

        return tmpQuizNumberBoxMarkup;
    }

    function setupFirstQuestion(allQuestionForms) {
        // hide all but the first question

        allQuestionForms.filter(function (index) {
            return index > 0;
        }).hide();

        $(".questionBoxListItem").filter(function (index) {
            return index == 0;
        }).addClass("selectedQuestionNumber");
    }

    // ===   ------------------------------------------------------------------------
    // ===   Module event handlers  ------------------------------------------------
    // ===   ------------------------------------------------------------------------
    function nextQuestionClick(event) {
        var previous = $("#PreviousQiestion"),
            next,
            currentNumber,
            maxNumber,
            nextNumber;

        if ($(this).hasClass("disabledNavButton")) {
            return;
        }

        //set the needed variables
        previous = $("#PreviousQiestion");
        next = $("#NextQiestion");
        currentNumber = $(".questionBoxListItem.selectedQuestionNumber").attr("data-number");
        maxNumber = $(".questionBoxesList li").size();

        //calculate the next number 
        nextNumber = parseInt(currentNumber) + 1;

        setActiveBox(nextNumber == maxNumber ? 0 : nextNumber);
        showQuestion(nextNumber == maxNumber ? 0 : nextNumber);

        if (nextNumber == maxNumber) {
            $(this).addClass("disabledNavButton");
        }

        previous.removeClass("disabledNavButton");
    }

    function previousQuestionClick(event) {
        var previous,
            next,
            currentNumber,
            maxNumber,
            nextNumber;

        if ($(this).hasClass("disabledNavButton")) {
            return;
        }

        //set the needed variables
        previous = $("#PreviousQiestion");
        next = $("#NextQiestion");
        currentNumber = $(".questionBoxListItem.selectedQuestionNumber").attr("data-number");
        maxNumber = $(".questionBoxesList li").size();

        //calculate the next number 

        nextNumber = parseInt(currentNumber) == 0 ? maxNumber - 1 : parseInt(currentNumber) - 1;


        setActiveBox(nextNumber);
        showQuestion(nextNumber);

        if (nextNumber == 1) {
            $(this).addClass("disabledNavButton");
        }

        next.removeClass("disabledNavButton");
    }


    function answerClicked(event) {
        $(this).toggleClass("selectedAnswer");
        event.stopPropagation();

        processAnswerClick($(this));
    }

    function processAnswerClick(answer) {
        var dialog,
            msg,
            tmpQuestionsList,
            tmpQuestion,
            tmpAnswersList,
            tmpAnswer,
            i,
            j,
            maxQuestions,
            maxAnswers,
            answerId = answer.attr("data-id"),
            questionId = answer.closest(".takeQuestionFormBackground").attr("data-id"),
            answerSelected = answer.hasClass("selectedAnswer");

        // find the question to which the answer belongs in this quiz
        tmpQuestionsList = quiz.Questions;
        for (i = 0, maxQuestions = tmpQuestionsList.length; i < maxQuestions; i++) {
            tmpQuestion = tmpQuestionsList[i];
            if (tmpQuestion.QuestionId == questionId) {
                break;
            }
        }

        //find the answer 
        tmpAnswersList = tmpQuestion.Answers;
        for (j = 0, maxAnswers = tmpAnswersList.length; j < maxAnswers; j++) {
            tmpAnswer = tmpAnswersList[j];
            if (tmpAnswer.AnswerId == answerId) {
                break;
            }
        }

        tmpAnswer.StudentAnswer = answerSelected;
    }

    function quizNumberBoxClick(event) {
        var number = $(this).attr("data-number");

        setActiveBox(number);

        showQuestion(number);
    }

    function setActiveBox(number) {

        if (number != 1) {
            $("#PreviousQiestion").removeClass("disabledNavButton");
        } else {
            $("#PreviousQiestion").addClass("disabledNavButton");
        }


        if (number != 0) {
            $("#NextQiestion").removeClass("disabledNavButton");
        } else {
            $("#NextQiestion").addClass("disabledNavButton");
        }

        $(".questionBoxListItem").removeClass("selectedQuestionNumber");

        $(".questionBoxListItem").filter(function () {
            return $(this).attr("data-number") == number;
        })
            .addClass("selectedQuestionNumber");
    }

    function submitAnswers(event) {
        var submitController = "TakeQuiz",
            submitAction = "SubmitQuiz",
            url,
            data;

        url = GeoAjax.GetUrlForAction(submitController, submitAction);
        data = GeoAjax.SimpleAjaxParam(quiz, "model");

        GeoAjax.StartLoading("body")
        GeoAjax.MakeAjaxPost(url, data, answersSubmited, answerSubmitFail);
    }

    function answersSubmited(msg) {
        GeoAjax.StopLoading()
        var dialog,
            dialogOptions = {};

        // setup values 
        dialogOptions.name = "SubmitetQuiz";
        dialogOptions.title = "Резултати!";

        dialogOptions.width = "400";
        dialogOptions.height = "200";

        dialogOptions.action1 = redirectToProfile;
        dialogOptions.onClose = redirectToProfile;
        dialogOptions.action1Text = "Затвори и врати се на профил!";

        if (msg.IsOk) {
            dialogOptions.contents = GeoMarkup.QuizMarkup.TakeQuiz.ResultMarkup(msg);
            dialogOptions.width = "600";
            dialogOptions.height = "600";
            dialog = GeoDialogFactory.ActionDialog(dialogOptions);
            dialog.Open();
            return;
        }
        else {
            dialogOptions.contents = msg.Message;
            dialog = GeoDialogFactory.ActionDialog(dialogOptions);
            dialog.Open();
            return;
        }
    }

    function answerSubmitFail() {
        GeoAjax.StopLoading()
        var dialog;
        dialog = GeoDialogFactory.InformationDialog("ErrorSubmitQuizResult", "Порака!", "Грешка во испраќање на податоците!", 400, 200);
        dialog.Open();
        return;
    }

    function redirectToProfile() {
        window.location = "/User/Home";
    }
    // ===   ------------------------------------------------------------------------
    // ===   Utilities --------------------------------------------------------------
    // ===   ------------------------------------------------------------------------

    function showQuestion(number) {
        //if the number box is zero show all questions
        if (number == 0) {
            $(".takeQuestionFormBackground").show();
        }
        else {
            // show only the clicked
            $(".takeQuestionFormBackground").hide();
            $(".takeQuestionFormBackground").filter(function (index) {
                return number == $(this).attr("data-box-number");
            }).show();
        }
    }

    return {
        InitializeTakeQuiz: mainInit,
        InitializePickQuiz: initPicker
    };
})();