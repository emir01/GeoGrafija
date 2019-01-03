using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeoGrafija.CustomFilters;
using GeoGrafija.ViewModels.QuizViewModels;
using GeoGrafija.ViewModels.SharedViewModels;
using Services.Interfaces;

namespace GeoGrafija.Controllers
{
    [Authorize]
    [RoleOrganizer]
    [RolesActionFilter(RequiredRoles = "Teacher|Admin")]
    public class QuizController : Controller
    {
        private readonly ITeacherQuizesService _teacherQuizesService;
        private readonly IUserService _userService;

        public QuizController(ITeacherQuizesService teacherQuizesService, IUserService userService)
        {
            this._teacherQuizesService = teacherQuizesService;
            _userService = userService;
        }

        //
        // GET: /Quiz/
        public ActionResult Index()
        {
            var teacherName = User.Identity.Name;

            var serviceQueryResult = _teacherQuizesService.GetAllQuizes(teacherName);
            
            var model = new DisplayQuizesViewModel(serviceQueryResult.GetData());

            return View(model);
        }

        //
        // GET : /Quiz/Edit/id
        public ActionResult Edit(int id)
        {
            throw new NotImplementedException();
        }

        //
        // GET : /Quiz/Edit/Details
        public ActionResult Details(int id)
        {
            throw new NotImplementedException();
        }

        //
        // GET : /Quiz/Edit/Delete
        public ActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }

        //
        // GET : /Quiz/Create
        public ActionResult Create()
        {
            var model = new CreateQuizViewModel();
            return View(model);
        }

        //
        // Ajax Post : /Quiz/Create
        // The Create Quiz action is a ajax post aciton because of the dynamic nature of the 
        // quiz properties
        [HttpPost]
        public ActionResult Create(CreateQuizViewModel jsonQuiz)
        {
            //if everything is valid with the passed in data
            if (ModelState.IsValid)
            {
                //get a model quiz object from view model
                var quiz = jsonQuiz.GetQuiz();
                var user = _userService.GetUser(User.Identity.Name);

                if(user == null)
                {
                    return Json(new SimpleJsonMessageViewModel("Корисникот не е најавен",1,0,false));
                }

                quiz.TeacherId = user.UserID;

                var result = _teacherQuizesService.CreateFullQuiz(quiz);

                if (result.IsOK)
                {
                    
                    return Json(new SimpleJsonMessageViewModel("Успешно креиран квиз", 0,0,true));
                }
                else
                {
                    return Json(new SimpleJsonMessageViewModel("Неуспешно зачуван тест. Обиди се повторно", 3, 0,false));
                }
            }
            else
            {
                // the sent model does not pass model validation
                return Json(new SimpleJsonMessageViewModel("Погрешно внесени податоци. Обиди се повторно",2,0,false));
            }
        }


        //
        //Ajax Post : /Quiz/ValidateQuizName
        [HttpPost]
        public ActionResult ValidateQuizName(string name)
        {
            var result = _teacherQuizesService.GetAllQuizes();

            if (result.IsOK)
            {
                var allQuizes = result.GetData();
                var sameName = allQuizes.Where(x => x.Name.Equals(name)).Count() > 0;
                return Json(new { valid = !sameName });    
            }
            else
            {
                return Json(new { valid = false });    
            }
        }
    }
}
