//module that takes care subspacing markup creation functions
var GeoMarkup = (function () {

    // Initialize the GeoMarkup Module
    function init() {
        utilityMarkup.Initialize();
        resourceMarkup.Initialize();
        quizMarkup.Initialize();
        explorationMarkup.Initialize();
    };

    // ===   ------------------------------------------------------------------
    // ===   Sub Module that handles generation of general markup to be used by all sub modules
    // ===   ------------------------------------------------------------------
    var utilityMarkup = (function () {
        function init() {

        };

        function wrapper() {
            var jWrapper = $("<div></div>");
            return jWrapper;
        };

        function clear() {
            var jWrapper = $("<div></div>").addClass("clear");

            return jWrapper;
        };

        function padding() {
            var jPadding = $("<div></div>").addClass("padding");
            return jPadding;
        };

        function unList() {
            var jList = $("<ul></ul>");
            return jList;
        }

        function listItem() {
            var jListItem = $("<li></li>");
            return jListItem;
        }

        function textInput(parameters) {
            var jTextInput = $("<input type='text' />");
            return jTextInput;
        }

        function checkBoxInput(parameters) {
            var jCheckBoxInput = $("<input type='checkbox' />");
            return jCheckBoxInput;
        }

        function hiddenInput() {
            var jHidden = $("<input type='hidden'>");
            return jHidden;
        }

        function heading2() {
            var jHeading = $("<h2></h2>");
            return jHeading;
        }

        return {
            Initialize: init,
            Wrapper: wrapper,
            Padding: padding,
            Clear: clear,
            UList: unList,
            ListItem: listItem,
            TextInput: textInput,
            CheckBoxInput: checkBoxInput,
            HiddenInput: hiddenInput,
            Heading2: heading2
        };
    })();

    // ===   ------------------------------------------------------------------
    // ===   Sub Module that handles markup generation for Quiz UI 
    // ===   ------------------------------------------------------------------
    var quizMarkup = (function () {
        // ===   ------------------------------------------------------------------
        // ===   Configuration and options parameters
        // ===   ------------------------------------------------------------------

        // ===   ------------------------------------------------------------------
        // ===   Functions and methods
        // ===   ------------------------------------------------------------------
        function init() {
        }

        // return a list where new questions can be added
        function getQuestionsList() {
            var jListWrapper = GeoMarkup.Utility.UList();
            var jpListWrapper = GeoMarkup.Utility.Padding();

            jListWrapper.append(jpListWrapper);
            jListWrapper.addClass("questionsList");

            return jListWrapper;
        }

        //return a list where new questions can be added
        function getAnswersList() {
            var jListWrapper = GeoMarkup.Utility.UList();
            var jpListWrapper = GeoMarkup.Utility.Padding();

            jListWrapper.append(jpListWrapper);
            jListWrapper.addClass("answerList");

            return jListWrapper;
        }

        //Returns a form for creating questions wrapped in a list item
        function getQuestionListItem() {
            var jListItemWrapper = GeoMarkup.Utility.ListItem(),
                jpListItemWrapper = GeoMarkup.Utility.Padding(),
                jCalcListItemWrapper,
                jCloseFormWrapper,
                jQuestionNameInputLabel,
                jQuestionNameInputForm,
                jQuestionAnswersButton;


            jListItemWrapper.addClass("questionForm");

            jListItemWrapper.append(jpListItemWrapper);
            jCalcListItemWrapper = GeoUtility.CheckPadding(jListItemWrapper);

            jCloseFormWrapper = GeoMarkup.Utility.Wrapper();
            jCloseFormWrapper.addClass("closeQuestionForm");
            jCalcListItemWrapper.append(jCloseFormWrapper);

            //create the label for the question name/text input
            jQuestionNameInputLabel = GeoMarkup.Utility.Wrapper()
                                     .addClass("editor-label")
                                     .html("Текст на Прашање : ");

            // create the text box for the question name/text
            jQuestionNameInputForm = GeoMarkup.Utility.Wrapper()
                                    .addClass("editor-field")
                                    .append(GeoMarkup.Utility.TextInput().addClass("questionText"))
                                    .append(GeoMarkup.Utility.Wrapper().html("?").addClass("visualQuestionMark"))
                                    .append(GeoMarkup.Utility.Clear());

            // create the show answers button for the question
            jQuestionAnswersButton = GeoMarkup.Utility.Wrapper()
                                    .addClass("questionFormAnswers")
                                    .append(GeoMarkup.Utility.Padding())
                                    .find(".padding")
                                    .html("Одговори")
                                    .end();

            // add the elements to the global wrapper
            jCalcListItemWrapper.append(jQuestionNameInputLabel);
            jCalcListItemWrapper.append(jQuestionNameInputForm);
            jCalcListItemWrapper.append(jQuestionAnswersButton);

            return jListItemWrapper;
        }

        //returns a form for creating answers wrapped in a list item
        function getAnswerListItem() {
            var jListItemWrapper = GeoMarkup.Utility.ListItem(),
                jpListItemWrapper = GeoMarkup.Utility.Padding(),
                jCalcListItemWrapper,
                jCloseFormWrapper,
                jAnswerNameInputLabel,
                jAnswerNameInputForm,
                jAnswerIsTrueInputLabel,
                jAnswerIsTrueInputForm;


            jListItemWrapper.addClass("answerForm");

            jListItemWrapper.append(jpListItemWrapper);
            jCalcListItemWrapper = GeoUtility.CheckPadding(jListItemWrapper);

            jCloseFormWrapper = GeoMarkup.Utility.Wrapper();
            jCloseFormWrapper.addClass("closeAnswerForm");
            jCalcListItemWrapper.append(jCloseFormWrapper);

            //create the label for the answer name/text input
            jAnswerNameInputLabel = GeoMarkup.Utility.Wrapper()
                                     .addClass("editor-label")
                                     .html("Текст на Одговор : ");

            // create the text box for the answer name/text
            jAnswerNameInputForm = GeoMarkup.Utility.Wrapper()
                                    .addClass("editor-field")
                                    .append(GeoMarkup.Utility.TextInput().addClass("answerText"))
                                    .append(GeoMarkup.Utility.Clear());


            //create the label for the answer is true property
            jAnswerIsTrueInputLabel = GeoMarkup.Utility.Wrapper()
                                     .addClass("editor-label")
                                     .html("Точен Одговор : ");

            // create the check box for the answer is true property
            jAnswerIsTrueInputForm = GeoMarkup.Utility.Wrapper()
                .addClass("editor-field")
                .append(GeoMarkup.Utility.CheckBoxInput().addClass("answerIsTrue"))
                .append(GeoMarkup.Utility.Wrapper().html("Да!").addClass("checkboxLabel"));



            // add the elements to the global wrapper
            jCalcListItemWrapper.append(jAnswerNameInputLabel);
            jCalcListItemWrapper.append(jAnswerNameInputForm);
            jCalcListItemWrapper.append(GeoMarkup.Utility.Clear());
            jCalcListItemWrapper.append(jAnswerIsTrueInputLabel);
            jCalcListItemWrapper.append(jAnswerIsTrueInputForm);

            return jListItemWrapper;
        }

        // gets a generic add item button 
        // that can be bound to either add questions or answers
        function getAddItemButton(text) {
            var wrapper,
                pwrapper,
                calcwrapper;

            wrapper = $("<input type='button'></input>");
            wrapper.val(text);

            wrapper.addClass("button green addQuestionButton");

            return wrapper;
        }

        // ===   ------------------------------------------------------------------
        // ===   Functions/Module that create markup for  displaying quizes for picking
        // ===   ------------------------------------------------------------------

        var pickQuiz = {};

        pickQuiz.GetPickQuizList = function getPickQuizList() {
            var jList,
                jpList;


            jList = GeoMarkup.Utility.UList();
            jpList = GeoMarkup.Utility.Padding();

            jList.append(jpList);
            jList.addClass("predefQuizList");

            return jList;
        };

        pickQuiz.GetPickQuizListItem = function getPickQuizListItem(quizInfo) {
            var predefQuizWrapper,
                predefQuizWrapperP,
                calcPredefQuizWrapper,
                quizNameWrapper,
                quizTypeWrapper,
                questionsCountWrapper,
                hiddenIdValWrapper;

            predefQuizWrapper = GeoMarkup.Utility.ListItem();
            predefQuizWrapperP = GeoMarkup.Utility.ListItem();

            predefQuizWrapper.addClass("predefQuizListItem");
            predefQuizWrapper.append(predefQuizWrapperP);

            calcPredefQuizWrapper = GeoUtility.CheckPadding(predefQuizWrapper);

            quizNameWrapper = GeoMarkup.Utility.Wrapper();
            quizNameWrapper.addClass("quizNameWrapper");
            quizNameWrapper.html("<span class='label'>Име на Квиз : </span> <span class='value'> " + quizInfo.QuizName + "</span>");

            questionsCountWrapper = GeoMarkup.Utility.Wrapper();
            questionsCountWrapper.addClass("questionsCountWrapper");
            questionsCountWrapper.html("<span class='label'> Број на прашања  во  Квизот : </span> <span class='value'>" + quizInfo.NumberOfQuestions + "</span>");

            quizTypeWrapper = GeoMarkup.Utility.Wrapper();
            quizTypeWrapper.addClass("quizTypeWrapper");
            quizTypeWrapper.html("<span class='label'> Тип на Квиз  : </span> <span class='value'>" + (quizInfo.QuizTypeName ? quizInfo.QuizTypeName : "Тест тип на квиз") + "</span>");

            hiddenIdValWrapper = GeoMarkup.Utility.HiddenInput();
            hiddenIdValWrapper.addClass("hiddenQuizId");
            hiddenIdValWrapper.val(quizInfo.QuizId);

            calcPredefQuizWrapper.append(quizNameWrapper);
            calcPredefQuizWrapper.append(questionsCountWrapper);
            //calcPredefQuizWrapper.append(quizTypeWrapper);
            calcPredefQuizWrapper.append(hiddenIdValWrapper);

            return predefQuizWrapper;
        };

        // ===   ------------------------------------------------------------------
        // ===   Functions that create markup for  displaying quizes for Taking quizes
        // ===   ------------------------------------------------------------------

        var takeQuiz = {};

        takeQuiz.QuestionWrapper = function (question) {
            var jWrapper,
                jpWrapper,
                jCalcWrapper,
                jAnswersHeadingWrapper,
                jpAnswersHeadingWrapper,
                jAnswersWrapper,
                jAnswersPaddingWrapper,
                jHeading,
                jHiddenQuestionId;


            //get basic objects
            jWrapper = GeoMarkup.Utility.Wrapper();
            jpWrapper = GeoMarkup.Utility.Padding();

            jAnswersWrapper = GeoMarkup.Utility.Wrapper();
            jAnswersPaddingWrapper = GeoMarkup.Utility.Padding();

            jHeading = GeoMarkup.Utility.Heading2();

            // customize object
            jWrapper.addClass("takeQuestionFormBackground formBackground");
            jWrapper.append(jpWrapper);
            jCalcWrapper = GeoUtility.CheckPadding(jWrapper);

            jHeading.addClass("formHeader");
            jHeading.html(question.QuestionText + "  ?");
            jCalcWrapper.append(jHeading);

            //create the answer heading wrapper : 
            jAnswersHeadingWrapper = GeoMarkup.Utility.Wrapper().addClass("takeAnswersHeader");

            jpAnswersHeadingWrapper = GeoMarkup.Utility.Padding();
            jpAnswersHeadingWrapper.html("Oдговори :");

            jAnswersHeadingWrapper.append(jpAnswersHeadingWrapper);
            jCalcWrapper.append(jAnswersHeadingWrapper);

            //customize and appendadd the answers wrapper 
            jAnswersWrapper.addClass("takeAnswersWrapper");
            jAnswersWrapper.append(jAnswersPaddingWrapper);
            jCalcWrapper.append(jAnswersWrapper);

            // set hidden question id;
            jWrapper.attr("data-id", question.QuestionId);

            return jWrapper;
        };

        takeQuiz.AnswerWrapper = function (answer) {
            var jWrapper,
                jpWrapper,
                jCalcWrapper,
                jAnswerTextWrapper,
                jHiddenAnswerId;

            jWrapper = GeoMarkup.Utility.Wrapper();
            jpWrapper = GeoMarkup.Utility.Padding();

            jWrapper.addClass("takeSingleAnswerWrapper");
            jWrapper.append(jpWrapper);
            jCalcWrapper = GeoUtility.CheckPadding(jWrapper);

            //set answer text and checkbox
            jAnswerTextWrapper = GeoMarkup.Utility.Wrapper();
            jAnswerTextWrapper.addClass("label");
            jAnswerTextWrapper.html(answer.AnswerText);

            jCalcWrapper.append(jAnswerTextWrapper);

            //set hidden answer id
            jWrapper.attr("data-id", answer.AnswerId);

            return jWrapper;
        };

        takeQuiz.QuizBox = function (number) {
            var jWrapper,
                jPWrapper,
                jCalcWrapper,
                jNumberWrapper,
                jpNumberWrapper;


            jWrapper = GeoMarkup.Utility.ListItem();
            jPWrapper = GeoMarkup.Utility.Padding();

            jWrapper.addClass("questionBoxListItem");
            jWrapper.append(jPWrapper);

            jCalcWrapper = GeoUtility.CheckPadding(jWrapper);

            jNumberWrapper = GeoMarkup.Utility.Wrapper();
            jpNumberWrapper = GeoMarkup.Utility.Padding();

            jNumberWrapper.addClass("questionNumber");
            jpNumberWrapper.html(number);
            jNumberWrapper.append(jpNumberWrapper);

            jCalcWrapper.append(jNumberWrapper);

            jWrapper.attr("data-number", number);
            return jWrapper;
        };

        takeQuiz.ResultMarkup = function (resultModel, options) {
            var jResultsWrapper,
                jResultsPaddingWrapper,
                jCalcGlboal,
                jHeadingWrapper;

            jResultsWrapper = GeoMarkup.Utility.Wrapper().attr("id", "QuizResultMessage");
            jResultsPaddingWrapper = GeoMarkup.Utility.Padding();

            jResultsWrapper.append(jResultsPaddingWrapper);

            jCalcGlboal = GeoUtility.CheckPadding(jResultsWrapper);

            options = options ? options : {};
            var title = options.title || "Твои резултати за квиз :";
            jHeadingWrapper = GeoMarkup.Utility.Wrapper().addClass("formHeader").html(title);
            jCalcGlboal.append(jHeadingWrapper);
            jCalcGlboal.append(GeoMarkup.Utility.Clear());

            var jPoints = getResultPointsMarkup(resultModel);
            jCalcGlboal.append(jPoints);
            jCalcGlboal.append(GeoMarkup.Utility.Clear());

            var jCorrectQuestions = getCorrectQuestionsMarkup(resultModel);
            jCalcGlboal.append(jCorrectQuestions);
            jCalcGlboal.append(GeoMarkup.Utility.Clear());

            if (resultModel.DetailedResults && resultModel.DetailedResults != "") {
                var jDetailedResults = getDetailQuestionsMarkup(resultModel.DetailedResults, options);
                jCalcGlboal.append(jDetailedResults);
                jCalcGlboal.append(GeoMarkup.Utility.Clear());
            }

            return jResultsWrapper;
        };

        // Get information markup about total won points
        function getResultPointsMarkup(resultModel) {
            var jPoints,
                jPointsLabel,
                jPointsValue;

            jPoints = GeoMarkup.Utility.Wrapper().addClass("points");
            jPointsLabel = GeoMarkup.Utility.Wrapper().addClass("label").html("Освоени Поени :");
            jPointsValue = GeoMarkup.Utility.Wrapper().addClass("value").html(resultModel.StudentPoints + "/" + resultModel.TotalPoints);
            jPoints.append(jPointsLabel);
            jPoints.append(jPointsValue);
            jPoints.append(GeoMarkup.Utility.Clear());

            return jPoints;
        }

        // Get information about total correct question
        function getCorrectQuestionsMarkup(resultModel) {
            var jCc,
                jCcLabel,
                jCcValue;

            jCc = GeoMarkup.Utility.Wrapper().addClass("questions");
            jCcLabel = GeoMarkup.Utility.Wrapper().addClass("label").html("Точни Одговори :");
            jCcValue = GeoMarkup.Utility.Wrapper().addClass("value").html(resultModel.CorrectQuestions);
            jCc.append(jCcLabel);
            jCc.append(jCcValue);
            jCc.append(GeoMarkup.Utility.Clear());

            return jCc;
        }

        // Get detailed result markup for all Questions
        function getDetailQuestionsMarkup(detailedResults, options) {
            var jWrapper,
                jPWrapper,
                jCalcWrapper,
                detailHeading,
                i,
                max,
                detailQuiestions,
                detailQuestion;

            jWrapper = GeoMarkup.Utility.Wrapper().addClass("detailedWrapper");
            jPWrapper = GeoMarkup.Utility.Padding();
            jWrapper.append(jPWrapper);

            jCalcWrapper = GeoUtility.CheckPadding(jWrapper);

            detailHeading = GeoMarkup.Utility.Wrapper().addClass("detailedHeader").html("Точни Одговори :");

            jCalcWrapper.append(detailHeading);

            jCalcWrapper.append(GeoMarkup.Utility.Clear());

            detailQuiestions = detailedResults.DetailedQuestions;
            for (i = 0, max = detailQuiestions.length; i < max; i++) {
                detailQuestion = detailQuiestions[i];
                jCalcWrapper.append(getQuestionDetailResult(detailQuestion, options));
                jCalcWrapper.append(GeoMarkup.Utility.Clear());
            }

            return jWrapper;
        }

        //Get detailed info markup for single question
        function getQuestionDetailResult(questionDetailedResult, options) {
            var jWrapper,
                jPWrapper,
                jCalcWrapper,
                jQuestionTextHeader,
                jPoints,
                jCorrectAnswers,
                jUserAnswers;

            jWrapper = GeoMarkup.Utility.Wrapper().addClass("detailQiestionInfo");
            jPWrapper = GeoMarkup.Utility.Padding();
            jWrapper.append(jPWrapper);

            jCalcWrapper = GeoUtility.CheckPadding(jWrapper);

            jQuestionTextHeader = GeoMarkup.Utility.Wrapper().addClass("questionTextHeader");
            if (questionDetailedResult.Correct) {
                jQuestionTextHeader.addClass("questionAnsweredCorrect");
            } else {
                jQuestionTextHeader.addClass("questionAnsweredWrong");
            }
            jQuestionTextHeader.html(questionDetailedResult.QuestionText + "?");

            jCalcWrapper.append(jQuestionTextHeader);
            jCalcWrapper.append(GeoMarkup.Utility.Clear());

            jPoints = GeoMarkup.Utility.Wrapper().addClass("questionWonPoints");
            jPoints.html("<div class='label'>Вкупно Поени : </div> <div class='value'>" + questionDetailedResult.Points + "</div>");

            jCalcWrapper.append(jPoints);
            jCalcWrapper.append(GeoMarkup.Utility.Clear());


            var answerHeader = options.studentAnswerHeader || "Твои Одговори :";
            jCorrectAnswers = getAnswerBox(questionDetailedResult.CorrectAnswers, "correctAnswerBox answerBox", "Точни Одговори :");
            jUserAnswers = getAnswerBox(questionDetailedResult.UserAnswers, "userAnswerBox answerBox", answerHeader);

            jCalcWrapper.append(jCorrectAnswers);
            jCalcWrapper.append(jUserAnswers);

            jCalcWrapper.append(GeoMarkup.Utility.Clear());

            return jWrapper;
        }

        function getAnswerBox(answers, cssClass, headerMessage) {
            var jWrapper,
                jPWrapper,
                jCalcWrapper,
                jHeading,
                jList,
                i,
                max,
                answer,
                listItem;

            jWrapper = GeoMarkup.Utility.Wrapper().addClass(cssClass); ;
            jPWrapper = GeoMarkup.Utility.Padding();
            jWrapper.append(jPWrapper);

            jCalcWrapper = GeoUtility.CheckPadding(jWrapper);

            jHeading = GeoMarkup.Utility.Wrapper().html(headerMessage).addClass("answerBoxHeader");
            jCalcWrapper.append(jHeading);
            jCalcWrapper.append(GeoMarkup.Utility.Clear());

            jList = GeoMarkup.Utility.UList().addClass("answerBoxList");

            for (i = 0, max = answers.length; i < max; i++) {
                answer = answers[i];
                listItem = GeoMarkup.Utility.ListItem().html(answer).addClass("answerBoxListItem");
                jList.append(listItem);
            }

            if (answers.length == 0 && cssClass.indexOf("userAnswerBox") >= 0) {
                listItem = GeoMarkup.Utility.ListItem().html("Нема одговори").addClass("answerBoxListItem noAnswerListItem");
                jList.append(listItem);
            }

            jList.append(GeoMarkup.Utility.Clear());

            jCalcWrapper.append(jList);
            jCalcWrapper.append(GeoMarkup.Utility.Clear());

            return jWrapper;
        }

        return {
            Initialize: init,
            GetAddItemButton: getAddItemButton,
            GetQuestionsList: getQuestionsList,
            GetQuestionsListItem: getQuestionListItem,
            GetAnswerList: getAnswersList,
            GetAnswerListItem: getAnswerListItem,
            PickQuiz: pickQuiz,
            TakeQuiz: takeQuiz
        };
    })();

    // ===   ------------------------------------------------------------------
    // ===   Sub Module that handles markup generation for Resources UI 
    // ===   ------------------------------------------------------------------
    var resourceMarkup = (function () {

        // ===   ------------------------------------------------------------------
        // ===   Resource markup module parameters and constant values
        // ===   ------------------------------------------------------------------    

        var sectionWidth = 400;
        var sectionMargin = 10;
        var sectionLeftMargin = 10;
        var sectionRightMargin = 10;

        var hoverHueChange = 20;

        // Configuration options to be returned as public for access from other modules or functions to query
        // Read only
        var options = {
            // section paramters
            SECTION_WIDTH: sectionWidth,
            SECTION_MARGIN: sectionMargin,
            SECTION_LEFT_MARGIN: sectionLeftMargin,
            SECTION_RIGHT_MARGIN: sectionRightMargin

            // Resource Ui Parameters 
        };

        // ===   ------------------------------------------------------------------
        // ===   Resource Markup module functions and methods
        // ===   ------------------------------------------------------------------

        // [PUBLIC]
        function init() {

        };

        // [PUBLIC] returns the global resourecs ui. Building this might go via passing ana array of sections from the 
        //  resourecs module, that have been created per locations
        function getResourcesGlobalUi(sections, locationId, locationName, nameBox) {
            var jwrapper = GeoMarkup.Utility.Wrapper();
            jwrapper.addClass("resourceUiWrapper");

            jwrapper.append(GeoMarkup.Utility.Padding());
            var jpwrapper = GeoUtility.CheckPadding(jwrapper);

            //iterate through all the sections and add them to the ui

            jpwrapper.append(nameBox);
            nameBox.hover(hoverInHueChange, hoverOutHueChange);
            jpwrapper.append(GeoMarkup.Utility.Clear());

            for (var i = 0; i < sections.length; i++) {
                var jSection = $(sections[i]);
                jpwrapper.append(jSection);
            }

            jpwrapper.append(GeoMarkup.Utility.Clear());

            return jwrapper;
        };

        function hoverInHueChange(event) {
            var rgb = GeoUtility.GetRgbValuesForProperty("background-color", $(this));

            var hoverRgb = GeoUtility.ChangeColorHue(rgb, -hoverHueChange);
            $(this).css("background-color", hoverRgb);
        };

        function hoverOutHueChange(event) {
            var rgb = GeoUtility.GetRgbValuesForProperty("background-color", $(this));

            var hoverRgb = GeoUtility.ChangeColorHue(rgb, hoverHueChange);

            $(this).css("background-color", hoverRgb);
        };

        // [PUBLIC] returns a single resource type section,  and inserts all the resources in the type section
        // gets called form the resource module, and then passed via array to the resource global ui
        function getResourceSection(resourceTypeName, color, resources, options) {
            var jwrapper = GeoMarkup.Utility.Wrapper();
            jwrapper.addClass("resourceSeciton");
            jwrapper.addClass("formBackground");
            jwrapper.css("width", sectionWidth);
            jwrapper.css("margin", sectionLeftMargin);
            jwrapper.css("margin-left", sectionLeftMargin);
            jwrapper.css("margin-right", sectionRightMargin);

            jwrapper.append(GeoMarkup.Utility.Padding());

            var jpwrapper = GeoUtility.CheckPadding(jwrapper);

            jpwrapper.append(getResourceNameInBox(resourceTypeName, color));

            // Add the resource links to the section
            // TODO : ADD RESOURCE LINKS

            return jwrapper;
        };

        // [PRIVATE] retuns a name box for the resorce seciton, color coding the background
        function getResourceNameInBox(name, color) {
            var jResourceNameBox = GeoMarkup.Utility.Wrapper();
            jResourceNameBox.addClass("resourceSectionHeadingBox");

            jResourceNameBox.append(GeoMarkup.Utility.Padding());
            var jpResourceNameBox = GeoUtility.CheckPadding(jResourceNameBox);

            var jnameWrapper = GeoMarkup.Utility.Wrapper();
            jnameWrapper.addClass("text objectTitle");
            jnameWrapper.html(name);

            jpResourceNameBox.append(jnameWrapper);

            jResourceNameBox.css("background-color", GeoUtility.ColorHexString(color));
            return jResourceNameBox;
        };

        // ===   ------------------------------------------------------------------
        // ===   Markup generation for Resources Creation Process
        // ===   ------------------------------------------------------------------
        var resourceCreation = {

        };

        resourceCreation.GetCreatorModalWindow = function (createFunction, typeName, typeId) {
            // give me a dialog window for creating resources of a certain type.
            // should have create and cancel functionality and probably rich editing
            var actionDialogOptions;

        };

        return {
            Initialize: init,
            GetSection: getResourceSection,
            GetUi: getResourcesGlobalUi,
            Options: options,
            ResourceCreation: resourceCreation
        };
    })();

    // ===   ------------------------------------------------------------------
    // ===   Sub Module that handles markup generation for Exploration UI
    // ===   ------------------------------------------------------------------
    var explorationMarkup = (function () {

        function init() {

        }

        function calloutMarkup(data) {
        }

        return {
            Initialize: init,
            CalloutMarkup: calloutMarkup
        };
    })();


    return {
        Initialize: init,
        Utility: utilityMarkup,
        ResourceMarkup: resourceMarkup,
        QuizMarkup: quizMarkup
    };
})();


