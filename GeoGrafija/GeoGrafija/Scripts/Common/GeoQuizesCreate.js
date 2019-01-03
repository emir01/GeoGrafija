// ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
// ===   Module used in displaying the specifics and handling  of the quiz creation ui  ---------------------------------------------------------------------------------------------
// ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
var GeoQuizesCreate = (function () {

    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    // ===   configuration options and parameters   ---------------------------------------------------------------------------------------------
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    var jAddQuestionButton,
        jAddAnswerButton,
        jQuestionsList,
        jAnswerList,
        quiz,
        saveQuizController = "Quiz",
        saveQuizAction = "Create",
        validateQuizNameController = "Quiz",
        validateQuizNameAction = "ValidateQuizName";

    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    // ===   Classes and Objects    ---------------------------------------------------------------------------------------------
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------

    // QUIZ ===========================================
    function Quiz(name) {
        this.Name = name || "";
        this.Questions = [];
    }

    Quiz.prototype.GetNextAvailableQuestionId = function () {
        if (this.Questions.length == 0) {
            return 1;
        }
        else {
            return this.Questions.length + 1;
        }
    };

    Quiz.prototype.AddQuestion = function (question) {
        var i,
            max,
            exsists = false,
            atIndex,
            tmpQuestion;

        for (i = 0, max = this.Questions.length; i < max; i++) {
            tmpQuestion = this.Questions[i];
            if (tmpQuestion.Id == question.Id) {
                exsists = true;
                atIndex = i;
                break;
            }
        }

        if (exsists) {
            this.Questions[atIndex] = question;
        }
        else {
            this.Questions.push(question);
        }
    };

    Quiz.prototype.GetQuestion = function (questionId) {
        var i,
            max,
            tmpQuestion;

        for (i = 0, max = this.Questions.length; i < max; i++) {
            tmpQuestion = this.Questions[i];
            if (tmpQuestion.Id == questionId) {
                return tmpQuestion;
            }
        }

        return undefined;
    };

    Quiz.prototype.DeleteQuestion = function (questionId) {
        var i,
            max,
            tmpQuestion,
            tmpIndex = -1;

        if (this.Questions.length == 0) {
            return;
        }

        for (i = 0, max = this.Questions.length; i < max; i++) {
            tmpQuestion = this.Questions[i];
            if (tmpQuestion.Id == questionId) {
                tmpIndex = i;
                break;
            }
        }

        if (tmpIndex >= 0) {
            this.Questions.splice(tmpIndex, 1);
        }
    };


    // QUESTION ===========================================
    function Question(Text, id) {
        this.Text = "Bogus";
        this.Id = id || 0;
        this.Answers = [];
    }

    Question.prototype.GetNextAvailableAnswerId = function () {
        if (this.Answers.length == 0) {
            return 1;
        }
        else {
            return this.Answers.length + 1;
        }
    };

    Question.prototype.AddAnswer = function (answer) {
        var i,
            max,
            exsists = false,
            atIndex,
            tmpAnswer;

        for (i = 0, max = this.Answers.length; i < max; i++) {
            tmpAnswer = this.Answers[i];
            if (tmpAnswer.Id == answer.Id) {
                exsists = true;
                atIndex = i;
                break;
            }
        }

        if (exsists) {
            this.Answers[atIndex] = answer;
        }
        else {
            this.Answers.push(answer);
        }
    };

    Question.prototype.GetAnswer = function (answerId) {
        var i,
            max,
            tmpAnswer;

        for (i = 0, max = this.Answers.length; i < max; i++) {
            tmpAnswer = this.Answers[i];
            if (tmpAnswer.Id == answerId) {
                return tmpAnswer;
            }
        }

        return undefined;
    };

    Question.prototype.DeleteAnswer = function (answerId) {
        var i,
            max,
            tmpAnswer,
            tmpIndex = -1;

        if (this.Answers.length == 0) {
            return;
        }

        for (i = 0, max = this.Answers.length; i < max; i++) {
            tmpAnswer = this.Answers[i];
            if (tmpAnswer.Id == answerId) {
                tmpIndex = i;
                break;
            }
        }

        if (tmpIndex >= 0) {
            this.Answers.splice(tmpIndex, 1);
        }
    };

    Question.prototype.SetText = function (Text) {
        this.Text = Text;
    };

    // ANSWER  ===========================================
    function Answer(answerText, id, questionId, isTrue) {
        this.AnswerText = answerText || "";
        this.Id = id || 0;
        this.QuestionId = questionId || 0;
        this.IsCorrect = isTrue || false;
    }

    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    // ===   General Values and Configuration parameters   ---------------------------------------------------------------------------------------------
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    var animationTime = 150;
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    // ===   General Funcitons and Methods   ---------------------------------------------------------------------------------------------
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------

    function init() {
        setupBasicUi();
        setupBasicEventHandling();
        quiz = new Quiz();
    }

    function setupBasicEventHandling(parameters) {
        // setup event handler for the submit button
        $("#CreateQuzizButton").click(createQuiz);
    }

    function setupBasicUi() {
        var jQWrapper,
            jQCalcWrapper,
            jAWrapper,
            jACalcWrapper;

        jQWrapper = $(".questionContainer").first();
        jQCalcWrapper = GeoUtility.CheckPadding(jQWrapper);

        //add the list that will hold the question forms.
        jQuestionsList = GeoMarkup.QuizMarkup.GetQuestionsList();
        jQCalcWrapper.append(jQuestionsList);

        // add the button that adds another question form.
        // this stays at the bottom of the questions list
        jAddQuestionButton = GeoMarkup.QuizMarkup.GetAddItemButton("Додади прашање");
        jAddQuestionButton.click(addQuizQuestionForm);
        jQCalcWrapper.append(jAddQuestionButton);

        // Setup the answer form : 
        jAWrapper = $(".answersContainer").first();
        jACalcWrapper = GeoUtility.CheckPadding(jAWrapper);

        // add list that will hold the answers forms
        jAnswerList = GeoMarkup.QuizMarkup.GetAnswerList();
        jACalcWrapper.append(jAnswerList);

        //add the button that will add new answers
        jAddAnswerButton = GeoMarkup.QuizMarkup.GetAddItemButton("Додади Одговор");
        jAddAnswerButton.click(addAnswerForm);
        jACalcWrapper.append(jAddAnswerButton);

    }

    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    // ===   Event Handlers   -------------------------------------------------------------------------------------------------------------------------------------
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------

    // triget when click on add question button.
    function addQuizQuestionForm(event) {
        var jListItem = GeoMarkup.QuizMarkup.GetQuestionsListItem();

        var id = quiz.GetNextAvailableQuestionId();
        quiz.AddQuestion(new Question("", id));

        //set the id of the created question to the list item
        jListItem.attr("data-id", id);
        $(".closeQuestionForm", jListItem).click(hideAndRemoveQuizQuestion);

        $(".questionFormAnswers", jListItem).click(showQuestionAnswersForm);

        jQuestionsList.append(jListItem);
        jListItem.slideDown(animationTime);
        event.stopPropagation();

    }

    // trigered when click on remove question from question field
    function hideAndRemoveQuizQuestion(event) {
        var jQuestionForm = $(this);
        var jQuestionFormListItem = jQuestionForm.closest("li");

        var id = jQuestionFormListItem.attr("data-id");
        quiz.DeleteQuestion(id);

        jQuestionFormListItem.slideUp(animationTime,function () {
            jQuestionFormListItem.remove();
        });
    }

    function showQuestionAnswersForm(event) {
        var jAnswersWrapper = $("#AnswersForm"),
            tmpAnswer,
            tmpNewQuestion,
            tmpOldQuestion;

        //show the answers form if it has not been shown yet
        jAnswersWrapper.slideDown(animationTime);

        //get the list item  of the currently selected question 
        tmpOldQuestion = quiz.GetQuestion($(".questionForm.selectedQuestionForm").first().attr("data-id"));

        //remove the selectedQuestionForm from all Question Forms
        $(".questionForm").removeClass("selectedQuestionForm");
        // get the list item for the clicked question
        var jListItem = $(this).closest("li");
        jListItem.addClass("selectedQuestionForm");

        //get the new question object that should be saved in the quiz 
        tmpNewQuestion = quiz.GetQuestion(jListItem.attr("data-id"));

        //if there was a prev selected question
        if (tmpOldQuestion) {
            //save the current aray of answers for the previously selected old question 
            $(".answerForm").each(function () {
                tmpAnswer = new Answer();
                tmpAnswer.Id = parseInt($(this).closest("li").attr("data-id"));
                tmpAnswer.QuestionId = tmpOldQuestion.Id;
                tmpAnswer.AnswerText = $(".answerText", $(this)).val();
                tmpAnswer.IsTrue = $(".answerIsTrue", $(this)).attr("checked") ? true : false;

                tmpOldQuestion.AddAnswer(tmpAnswer);
            });
            //store back the old question 
            quiz.AddQuestion(tmpOldQuestion);
        }

        // remove all the previous answers to the previously selected question
        $(".answerForm").remove();

        //draw the saved answers for the newly selected question
        $.each(tmpNewQuestion.Answers, function (index, answer) {
            var tmpAnswerForm;

            //get an answer form and preset some of the values as gotten from the answer
            tmpAnswerForm = GeoMarkup.QuizMarkup.GetAnswerListItem();
            tmpAnswerForm.attr("data-id", answer.Id);

            $(".answerText", tmpAnswerForm).val(answer.AnswerText);
            $(".answerIsTrue", tmpAnswerForm).attr("checked", answer.IsTrue);

            $(".answerList").append(tmpAnswerForm);

            tmpAnswerForm.show();
            $(".closeAnswerForm", tmpAnswerForm).click(hideAndRemoveAnswer);
        });
    }

    //============== ANSWERS TRIGERS  and EVENTS
    // triget when click on add answer button.
    function addAnswerForm(event) {
        //get an answer form list item and set up the event for the close button
        var jListItemAnswer,
            questionId,
            availableId,
            newAnswer,
            selectedQuestion;

        jListItemAnswer = GeoMarkup.QuizMarkup.GetAnswerListItem();
        $(".closeAnswerForm", jListItemAnswer).click(hideAndRemoveAnswer);

        // set an id for the jListItemAnswerForm based on the available ids for the answers on the currently]
        // selected question
        questionId = $(".questionForm.selectedQuestionForm").closest("li").attr("data-id");
        selectedQuestion = quiz.GetQuestion(questionId);
        availableId = selectedQuestion.GetNextAvailableAnswerId();

        //create and store a new answer object
        newAnswer = new Answer();
        newAnswer.Id = availableId;
        newAnswer.QuestionId = selectedQuestion.Id;
        newAnswer.AnswerText = "";

        selectedQuestion.AddAnswer(newAnswer);

        jListItemAnswer.attr("data-id", availableId);
        jAnswerList.append(jListItemAnswer);
        jListItemAnswer.slideDown(animationTime);
        event.stopPropagation();
    }

    // trigered when click on remove answer  from answer form
    function hideAndRemoveAnswer(event) {
        var jAnswerForm,
            jAnswerFormListItem,
            currentQuestion,
            answerId;

        //get the current answer form and list item
        jAnswerForm = $(this);
        jAnswerFormListItem = jAnswerForm.closest("li");

        //get the current question  
        currentQuestion = quiz.GetQuestion($(".questionForm.selectedQuestionForm").closest("li").attr("data-id"));
        //delete the answer from the current question array.
        answerId = jAnswerFormListItem.attr("data-id");
        currentQuestion.DeleteAnswer(answerId);

        jAnswerFormListItem.slideUp(animationTime,function () {
            jAnswerFormListItem.remove();
        });
    }

    function createQuiz(event) {
        var prevalid,
            tmpOldQuestion,
            allQuestions,
            tmpAnswer,
            valid,
            quizName;

        event.stopPropagation();
        event.preventDefault();

        // get the information for the currently selected question
        // as that information is not saved by changing answer forms for question
        tmpOldQuestion = quiz.GetQuestion($(".questionForm.selectedQuestionForm").first().attr("data-id"));
        if (tmpOldQuestion) {
            //save the current aray of answers for the previously selected old question 
            $(".answerForm").each(function () {
                tmpAnswer = new Answer();
                tmpAnswer.Id = parseInt($(this).closest("li").attr("data-id"));
                tmpAnswer.QuestionId = tmpOldQuestion.Id;
                tmpAnswer.AnswerText = $(".answerText", $(this)).val();
                tmpAnswer.IsTrue = $(".answerIsTrue", $(this)).attr("checked") ? true : false;

                tmpOldQuestion.AddAnswer(tmpAnswer);
            });
            //store back the old question 
            quiz.AddQuestion(tmpOldQuestion);
        }

        //save all the questions texts 
        allQuestions = $(".questionForm");
        allQuestions.each(function (index, element) {
            var tmpId,
                tmpText,
                currQuestion;

            tmpId = $(element).attr("data-id");
            tmpText = $.trim($(".questionText", element).val());

            currQuestion = quiz.GetQuestion(tmpId);

            currQuestion.Text = tmpText;
            quiz.AddQuestion(currQuestion);
        });

        //set the quiz name 
        quizName = $("#Name").val();
        quiz.Name = quizName;

        valid = validateQuiz();
        if (!valid) {
            return;
        }

        validateQuizNameServerSide();

        event.stopPropagation();
        event.preventDefault();
    };

    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    // ===   Utilities    -----------------------------------------------------------------------------------------------------------------------------------------
    // ===   ------------------------------------------------------------------------------------------------------------------------------------------------------
    function validateQuizNameServerSide() {
        var jData,
            url;

        url = GeoAjax.GetUrlForAction(validateQuizNameController, validateQuizNameAction);
        jData = GeoAjax.SimpleAjaxParam(quiz.Name, "name");

        GeoAjax.MakeAjaxPost(url, jData, quizNameCheckSuccessCallback, quizNameCheckFailCallback);
    }

    function quizNameCheckSuccessCallback(msg) {
        var dialog;
        if (msg.valid) {
            saveQuiz();
            return true;
        }
        else {
            dialog = GeoDialogFactory.InformationDialog("DuplicateQuizName", "Порака", " Веќе постои квиз со такво име!", 400, 200);
            dialog.Open();
            return false;
        }
    }

    function quizNameCheckFailCallback(msg) {
        var dialog = GeoDialogFactory.InformationDialog("DuplicateQuizNameCheckFail", "Порака", "Грешка при проверка на квиз!", 400, 200);
        dialog.Open();
        return false;
    }

    function validateQuiz() {
        var dialog,
            i,
            j,
            tmpQuestion,
            tmpAnswer,
            tmpAnswers,
            maxQuestions,
            maxAnswers,
            trueAnswers;

        //validate quiz name
        if (quiz.Name.length <= 2) {
            dialog = GeoDialogFactory.InformationDialog("InvalidQuizNameQuestions", "Порака", "Квизот мора да има име со барем 3 карактери!", 400, 200);
            dialog.Open();
            return false;
        }

        //validate questions
        if (quiz.Questions.length === 0) {
            dialog = GeoDialogFactory.InformationDialog("InvalidQuestionsLength", "Порака", "Квизот мора да има барем едно прашање!", 400, 200);
            dialog.Open();
            return false;
        }

        // check all questions 
        for (i = 0, maxQuestions = quiz.Questions.length; i < maxQuestions; i++) {
            // chek question name 
            tmpQuestion = quiz.Questions[i];

            if (tmpQuestion.Text.length <= 2) {
                dialog = GeoDialogFactory.InformationDialog("InvalidQuestionName", "Порака", "Сите Прашања мора да имаат текст со минимум 3 карактери!", 400, 200);
                dialog.Open();
                return false;
            }

            //check number of answers for question
            if (tmpQuestion.Answers.length <= 1) {
                dialog = GeoDialogFactory.InformationDialog("InvalidQuestionAnswers", "Порака", "Сите Прашања мора да имаат барем  2 одговори!", 400, 200);
                dialog.Open();
                return false;
            }

            //check  the answers for this question if everything else so far was valis
            trueAnswers = 0;
            tmpAnswers = tmpQuestion.Answers;
            for (j = 0, maxAnswers = tmpAnswers.length; j < maxAnswers; j++) {
                tmpAnswer = tmpAnswers[j];
                if (tmpAnswer.AnswerText.length <= 2) {
                    dialog = GeoDialogFactory.InformationDialog("InvalidAnswerName", "Порака", "Сите Одговори мора да имаат текст со минимум 3 карактери!", 400, 200);
                    dialog.Open();
                    return false;
                }

                if (tmpAnswer.IsTrue) {
                    trueAnswers = trueAnswers + 1;
                }
            }

            if (trueAnswers == 0) {
                dialog = GeoDialogFactory.InformationDialog("QuestionNoAnswer", "Порака", "Сите Прашања мора да имаат барем  1 точен одговор!", 400, 200);
                dialog.Open();
                return false;
            }
        }

        return true;
    }

    //store the quiz
    function saveQuiz() {
        var jsonData,
            url;

        jsonData = GeoAjax.SimpleAjaxParam(quiz, "jsonQuiz");
        url = GeoAjax.GetUrlForAction(saveQuizController, saveQuizAction);

        GeoUtility.Freeze();
        GeoAjax.MakeAjaxPost(url, jsonData, saveQuizSuccess, saveQuizFail);
    }

    function saveQuizSuccess(msg) {
        GeoUtility.UnFreeze();
        var returnType,
            returnMessage,
            returnData,
            dialog;

        returnMessage = msg.Message;
        returnType = msg.Type;
        returnData = msg.Data;
        // display Message
        // and do acording to type
        dialog = GeoDialogFactory.InformationDialog("SaveCallSuccessType" + returnType, "Порака", returnMessage, 400, 200);
        dialog.Open();
    }

    function saveQuizFail(msg) {
        GeoUtility.UnFreeze();
        var dialog = GeoDialogFactory.InformationDialog("FailSaveQuiz", "Порака", "Неуспешно зачуван квиз. Обиди се повторно!", 400, 200);
        dialog.Open();
        return false;

    }
    return {
        Initialize: init
    };
})();