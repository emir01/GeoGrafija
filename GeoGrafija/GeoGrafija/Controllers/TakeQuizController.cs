using System.Linq;
using System.Web.Mvc;
using Common.Static_Dictionary;
using GeoGrafija.CustomFilters;
using GeoGrafija.ViewModels.TakeQuizViewModels;
using Services.Enums;
using Services.Interfaces;

namespace GeoGrafija.Controllers
{
    [Authorize]
    [RoleOrganizer]
    [RolesActionFilter(RequiredRoles = "Student|Teacher|Admin")]
    public class TakeQuizController : Controller
    {
        private readonly IStudnetQuizesService _takeQuizService;
        private readonly IUserService _userService;

        public TakeQuizController(IStudnetQuizesService takeQuizService, IUserService userService)
        {
            _takeQuizService = takeQuizService;
            _userService = userService;
        }

        //
        // GET : TakeQuiz/
        public  ActionResult Index()
        {
            //return  the view that will allow the user to pick a quiz to do.
            return View();
        }
        
        //
        // Ajax POST: /TakeQuiz/PredifnedQuizes 
        [HttpPost]
        public ActionResult PredifnedQuizes()
        {
            // list predefined quizes   
            var userName = User.Identity.Name;
            
            var result = _takeQuizService.GetAvailableQuies(userName, QuizGetType.Pregenarated);

            //create the view model if everything is ok
            if (result.IsOK)
            {
                return Json(new AvailableQuizesViewModel(result.GetData().ToList()));
            }
            else
            {
                return Json(new AvailableQuizesViewModel());
            }
        }

        //
        // GET : /TakeQuiz/TakingQuiz/id
        public ActionResult TakingQuiz(int? quizId)
        {

            if (quizId == null)
            {
                return RedirectToAction("Home", "User");
            }

            var studentName = User.Identity.Name;

            var student = _userService.GetUser(studentName);

            // If the current user hasnt got the student role redirect to profile.
            if (!student.UserHasRole(RoleNames.STUDENT))
            {
                return RedirectToAction("Home", "User");
            }

            // if the student has already taken this
            if (student.StudentQuizResults.Any(x => x.QuizId == quizId))
            {
                return RedirectToAction("Home", "User");
            }

            return View(); // the view where the quiz will be rendered by javascript
        }

        //
        // AJAX : Return a quiz object that will be displayed and the quiz will start on the client
        [HttpPost]
        public ActionResult GetQuiz(int quizId)
        {
            // get quiz from sevice based on the quiz id
            var result = _takeQuizService.TakeQuiz(quizId, User.Identity.Name);

            if (result.IsOK)
            {
                var quiz = result.GetData().Quiz;
                var student = result.GetData().Student;

                var viewModel = new TakeQuizDisplayViewModel(quiz, student);
                viewModel.IsOk = true;
                return Json(viewModel);
            }
            else
            {
                var viewModel = new TakeQuizDisplayViewModel(null, null);
                viewModel.IsOk = false;
                return Json(viewModel);
            }
        }

        [HttpPost]
        public ActionResult SubmitQuiz(TakeQuizDisplayViewModel model)
        {
            var studentName = User.Identity.Name;
            var quiz = model.GetQuizFromModel();
         
            var result = _takeQuizService.ProcessResults(quiz,studentName);

            if (result.IsOK)
            {
                var quizResult = result.GetData();
                var viewModel = new QuizResultViewModel(studentName, quizResult);
                viewModel.IsOk = true;
                return Json(viewModel);
            }
            else
            {
                var quizResult = result.GetData();
                var viewModel = new QuizResultViewModel(studentName, quizResult);
                viewModel.IsOk = false;
                viewModel.Message = result.Messages.FirstOrDefault();
                return Json(viewModel);
            }
        }
    }
}
