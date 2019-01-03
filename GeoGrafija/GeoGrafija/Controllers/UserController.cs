using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Common.Classroom;
using Common.Static_Dictionary;
using GeoGrafija.CustomFilters;
using GeoGrafija.ModelResolver;
using GeoGrafija.ViewModels;
using GeoGrafija.ViewModels.Profiles;
using GeoGrafija.ViewModels.TakeQuizViewModels;
using GeoGrafija.ViewModels.UserViewModels;
using Model;
using Services.Interfaces;
using Services.ResultClasses;

namespace GeoGrafija.Controllers
{
    [RoleOrganizer]
    [RolesActionFilter(RequiredRoles = "")]
    public class UserController : Controller
    {
        public IUserService UserService { get; set; }
        public IRolesService RoleService { get; set; }
        public ILocationService LocationService { get; set; }
        public IResourceService ResourceService { get; set; }
        public ITeacherQuizesService TeacherQuizesService { get; set; }

        public UserController(IUserService service, IRolesService roleService, ILocationService locationService, ITeacherQuizesService teacherQuizesService, IResourceService resourceService)
        {
            UserService = service;
            RoleService = roleService;
            LocationService = locationService;
            ResourceService = resourceService;
            TeacherQuizesService = teacherQuizesService;
        }

        public ActionResult LogOn()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Home");
            }

            return View();
        }

        //POST : LogOn
        [HttpPost]
        public ActionResult LogOn(User user, string returnUrl)
        {
            if (UserService.CheckCredentials(user))
            {
                UserService.SignIn(user.UserName, false);

                if (!String.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Home");
                }
            }
            else
            {
                //Return Errors
                ModelState.AddModelError("InvalidUser", "Внесовте погрешни информации. Обидете се Повторно");
                return View();
            }
        }

        //GET
        // Home
        [Authorize]
        public ActionResult Home()
        {
            var user = UserService.GetUser(User.Identity.Name);

            var viewModel = new ProfileViewModel(user);

            // check the student role and set the extra profile view model information
            if (user.UserHasRole(RoleNames.ADMIN))
            {
                var adminViewModel = GetAdminProfileViewModel(user);
                viewModel.AdminViewModel = adminViewModel;
                ViewBag.NavigationType = "_AdminNavigation";
                return View(viewModel);
            }

            if (user.UserHasRole(RoleNames.STUDENT))
            {
                var studentViewModel = GetStudentProfileViewModel(user);
                viewModel.StudentViewModel = studentViewModel;
                ViewBag.NavigationType = "_StudentNavigation";
                return View(viewModel);
            }

            if (user.UserHasRole(RoleNames.TEACHER))
            {
                var teacherViewModel = GetTeacherProfileViewModel(user);
                viewModel.TeacherViewModel = teacherViewModel;
                ViewBag.NavigationType = "_TeacherNavigation";
                return View(viewModel);
            }

            return View(viewModel);
        }

        // GET 
        // LOG_OFF
        public ActionResult LogOff()
        {
            UserService.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //GET: Register
        public ActionResult Register()
        {
            var model = new RegisterViewModel();
            return View(model);
        }

        //POST : Register
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            var registeringEnabled = ConfigurationManager.AppSettings["RegisterEnabled"];

            if (registeringEnabled == "true")
            {
                if (ModelState.IsValid)
                {
                    var user = SimpleFactories.GetUserFromRegisterViewModel(model);
                    
                    var result = UserService.RegisterUser(user);
                    if (result.Status)
                    {
                        UserService.SignIn(model.UserName, false);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        for (int i = 0; i < result.ErrorCodes.Count; i++)
                        {
                            ModelState.AddModelError("", RegisterResult.GetErrorMessage(result.ErrorCodes[i]));
                        }
                    }
                }

                return View(model);
            }
            else
            {
                ModelState.AddModelError("", "Регистирањето е оневозможено. Обидете се подоцна");
                return View(model);
            }
        }


        #region Ajax Actions

        [HttpPost]
        public ActionResult SetStudentTeacher(int teacherId)
        {
            var studentName = User.Identity.Name;
            var result = UserService.SetStudentTeacher(studentName, teacherId);

            if (result.IsOK)
            {
                var viewModel = new NewTeacherResultViewModel(result.GetData());
                viewModel.ChangeOk = true;
                return Json(viewModel);
            }
            else
            {
                var viewModel = new NewTeacherResultViewModel(result.GetData());
                viewModel.ChangeOk = false;
                viewModel.Message = result.Messages.FirstOrDefault();
                return Json(viewModel);
            }
        }

        [HttpPost]
        public ActionResult GetStudentQuizResultDetails(int resultId)
        {
            var result = UserService.GetStudentQuizResult(resultId);

            if (result.IsOK)
            {
                var viewModel = new QuizResultViewModel(User.Identity.Name, result.GetData());
                viewModel.IsOk = true;
                return Json(viewModel);
            }
            else
            {
                var viewModel = new QuizResultViewModel(User.Identity.Name, result.GetData());
                viewModel.IsOk = false;
                viewModel.Message = result.Messages[0];
                return Json(viewModel);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveTeacherClassRoom(Classroom classRoom, int? teacherId)
        {
            if (teacherId == null)
            {
                return Json(new { message = "Не се пратени податоци за професор!", success = false, status = false });
            }

            var user = UserService.GetUser((int)teacherId);

            if (user == null)
            {
                return Json(new { message = "Нема професор со даден ID!", success = false, status = false });
            }

            if (classRoom == null)
            {
                return Json(new { message = "Не се пратени податоци за училница!", success = false, status = false });
            }

            classRoom.LastEdit = DateTime.Now;
            var classRoomXml = ClassroomSerializer.SerializeClassroomToXml(classRoom);

            if (String.IsNullOrWhiteSpace(classRoomXml))
            {
                return Json(new { message = "Не може да се прочитаат податоците за училница!", success = false, status = false });
            }

            user.TeacherClassroomDefinition = classRoomXml;

            var success = UserService.GeneralUpdateUser(user);

            if (success)
            {
                return Json(new { message = "Успешно зачувана училница!", success = true, status = true });
            }
            else
            {
                return Json(new { message = "Не може да се зачуваат податоците за нова училница!", success = false });
            }
        }

        [HttpPost]
        public ActionResult GetClassroom(int teacherId)
        {
            var user = UserService.GetUser(teacherId);

            if (String.IsNullOrWhiteSpace(user.TeacherClassroomDefinition))
            {
                return Json(new Classroom(true));
            }

            else
            {
                var classRoom = ClassroomSerializer.DesirializeFromXml(user.TeacherClassroomDefinition);

                if (classRoom == null)
                {
                    return Json(new Classroom());
                }
                else
                {
                    return Json(classRoom);
                }
            }
        }

        #endregion

        #region Extra Profile View Models

        private TeacherProfileViewModel GetTeacherProfileViewModel(User teacher)
        {
            var teacherViewModel = new TeacherProfileViewModel();
            var teacherName = User.Identity.Name;

            foreach (var student in teacher.Students)
            {
                var studentInfo = new MyStudentInfoViewModel();

                studentInfo.StudentId = student.UserID;
                studentInfo.UserName = student.UserName;

                studentInfo.Rank = UserService.CalculateStudentRank(student.UserName);
                studentInfo.StudentPoints = UserService.CalculateStudentTotalPoints(student.UserName);
                studentInfo.TotalPoints = UserService.CalculateStudentTotalQuizPoints(student.UserName);

                teacherViewModel.MyStudents.Add(studentInfo);
            }

            return teacherViewModel;
        }

        private StudentProfileViewModel GetStudentProfileViewModel(User student)
        {
            var studentViewModel = new StudentProfileViewModel();

            // Get all the available teachers.
            var allTeachers = UserService.GetUsersByRole(RoleNames.TEACHER);
            studentViewModel.AvailableTeachers = allTeachers;

            var studentName = User.Identity.Name;

            // Get Global Student Rank
            studentViewModel.GlobalRank = UserService.CalculateStudentRank(studentName);

            // Get Student total points
            studentViewModel.TotalStudentPoints = UserService.CalculateStudentTotalPoints(studentName);

            // Get student total quiz points
            studentViewModel.TotalQuizPoints = UserService.CalculateStudentTotalQuizPoints(studentName);

            studentViewModel.NumberOfLocationsDetailsOpened = student.OpenedLocationDetails ?? 0;
            studentViewModel.NumberOfResourcesOpened = student.OpenedResources ?? 0;

            var totalStudents = UserService.GetUsersByRole(RoleNames.STUDENT).Count;

            studentViewModel.StudentsAfterYou = totalStudents - studentViewModel.GlobalRank;
            studentViewModel.StudentsBeforeYou = totalStudents - (studentViewModel.GlobalRank + 1);

            studentViewModel.RankName = student.Rank.RankName;
            studentViewModel.RankImage = student.Rank.RankImage;
            studentViewModel.RankOrder = student.Rank.RankOrder;

            if (student.Rank.ParentRank == null)
            {
                studentViewModel.NextPointBorder = -1;
            }
            else
            {
                studentViewModel.NextPointBorder = student.Rank.ParentRank.RequiredPoints;
            }

            studentViewModel.ProgressPercent = UserService.CalculateStudentPercentProgress(studentName);

            if (student.Teacher != null)
            {
                studentViewModel.HasTeacher = true;
                var classRoomXml = student.Teacher.TeacherClassroomDefinition;

                if (!string.IsNullOrWhiteSpace(classRoomXml))
                {
                    var classroom = ClassroomSerializer.DesirializeFromXml(classRoomXml);

                    if (classroom != null)
                    {
                        studentViewModel.Classroom = classroom;
                    }
                }
            }
            else
            {
                studentViewModel.HasTeacher = false;
                studentViewModel.Classroom = null;
            }
            return studentViewModel;
        }

        private AdminProfileViewModel GetAdminProfileViewModel(User admin)
        {
            var adminViewModel = new AdminProfileViewModel();

            adminViewModel.TotalUsers = UserService.GetUsers().Count;
            adminViewModel.StudentNumber = UserService.GetUsersByRole(RoleNames.STUDENT).Count;
            adminViewModel.TeacherNumber = UserService.GetUsersByRole(RoleNames.TEACHER).Count;
            adminViewModel.AdminNumber = UserService.GetUsersByRole(RoleNames.ADMIN).Count;

            adminViewModel.LocationsNumber = LocationService.GetAllLocations().Count;
            adminViewModel.LocationTypesNumber = LocationService.GetAllLocationTypes().Count;
            adminViewModel.ResourcesNumber = ResourceService.GetAllResources().GetData().Count();
            adminViewModel.AverageResourcePerLocation = (adminViewModel.ResourcesNumber / (double)adminViewModel.LocationsNumber);

            adminViewModel.QuizesNumber = TeacherQuizesService.GetAllQuizes().GetData().Count();
            adminViewModel.QuestionsNumber = TeacherQuizesService.GetAllQuizQuestions().GetData().Count();
            adminViewModel.StudentTakenQuizesNumber = TeacherQuizesService.GetAllStudentResults().GetData().Count();

            return adminViewModel;
        }

        #endregion
    }
}
