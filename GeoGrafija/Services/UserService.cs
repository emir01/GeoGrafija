using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using Common.Static_Dictionary;
using GeoGrafija.ResultClasses;
using Model;
using Model.Interfaces;

using Services.Interfaces;
using Services.ResultClasses;

namespace Services
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IStudentQuizResultRepository _quizResultRepository;
        
        public UserService(IUserRepository repository, IStudentQuizResultRepository quizResultRepository)
        {
            _userRepository = repository;
            _quizResultRepository = quizResultRepository;
        }

        public UserService(IUserRepository repository)
        {
            _userRepository = repository;
            
        }

        public bool CheckCredentials(User User)
        {
            var user = _userRepository.GetUser(User.UserName);

            if (user == null)
            {
                return false;
            }

            if (!user.Password.Equals(User.Password))
            {
                return false;
            }

            return true;
        }

        public void  SignIn(string UserName, bool persistance)
        {
            if (String.IsNullOrEmpty(UserName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            FormsAuthentication.SetAuthCookie(UserName, persistance);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        public RegisterResult RegisterUser(User model)
        {
            var result = new RegisterResult();
            result.Status = true;
            var userResult = _userRepository.GetUser(model.UserName);

            if (userResult == null)
            {
                var AllUsers = _userRepository.GetAllUsers();
                var emailUser = from p in AllUsers
                                where p.Email == model.Email
                                select p;

                if (emailUser.Count() > 0)
                {
                    result.Status = false;
                    result.ErrorCodes.Add(RegisterErrorCodes.EmailAlreadyTaken);
                }
                else
                {
                    var user = new User()
                    {
                        UserName = model.UserName,
                        Password = model.Password,
                        Email = model.Email,
                        CurrentRank = model.CurrentRank,
                    };

                    _userRepository.AddUser(user);
                }
            }
            else
            {
                result.Status = false;
                result.ErrorCodes.Add(RegisterErrorCodes.UsernameAlreadyTaken);
            }

            return result;
        }

        public List<User> GetFilteredUsers(string FilterOutRole)
        {
            var allUsers = _userRepository.GetAllUsers();
            var filterNotAdmins = from user in allUsers
                                  where !user.UserHasRole(FilterOutRole.ToLower())
                                  select user;
            
            return filterNotAdmins.ToList();
        }

        public List<User> GetUsersByRole(string roleName)
        {
            var allUsers = _userRepository.GetAllUsers();

            var usersByRoles = allUsers.Where(x => x.Roles.Where(y => y.RoleName == roleName).Count() > 0);

            return usersByRoles.ToList();
        }

        public List<User> GetUsers()
        {
            var allUsers = _userRepository.GetAllUsers();
            return allUsers.ToList();
        }

        public User GetUser(int UserID)
        {
            return _userRepository.GetUser(UserID);
        }

        public User GetUser(string UserName)
        {
            return _userRepository.GetUser(UserName);
        }

        public GenericOperationResult<User> SetStudentTeacher(string studentName, int teacherId)
        {
            var result = OperationResult.GetGenericResultObject<User>();

            try
            {
                var student = GetUser(studentName);

                if (student == null)
                {
                    result.AddMessage("Не постои таков студент");
                    return result;
                }

                var teacher = GetUser(teacherId);

                if (teacher == null)
                {
                    result.AddMessage("Не постои таков професор");
                    return result;
                }

                student.ParentUserId = teacher.UserID;
                _userRepository.UpdateUser(student);
                result.SetStatus(true);
                result.SetSuccess();
                var updateUser = GetUser(studentName);
                result.SetData(updateUser);
                return result;
            }
            catch (Exception e)
            {
                result.SetException();
                result.AddExceptionMessage(e.Message);
                result.AddMessage(e.Message);
            }

            return result;
        }

        public int CalculateStudentRank(string studentName)
        {
            var rank = 0;

            var currentStudent = GetUser(studentName);

            if (currentStudent == null)
            {
                return rank;
            }

            var allStudents = GetUsersByRole(RoleNames.STUDENT);

            var sumStudents = (from s in allStudents
                               select new
                                          {
                                              StudentId = s.UserID,
                                              StudentTotalPoints = s.StudentQuizResults.Sum(x=>x.PointsStudent)
                                          });
            var rankedStudents = sumStudents.OrderByDescending(x => x.StudentTotalPoints);

            foreach (var rankedStudent in rankedStudents)
            {
                if (rankedStudent.StudentId != currentStudent.UserID)
                {
                    rank++;
                }
                else
                {
                    rank++;
                    break;
                }
            }

            return rank;
        }

        public int CalculateStudentTotalPoints(string studentName)
        {
            var points = 0;

            var currentStudent = GetUser(studentName);

            points = currentStudent.StudentQuizResults.Sum(x => x.PointsStudent);

            return points;
        }

        public int CalculateStudentTotalQuizPoints(string studentName)
        {
            var totalQuizPoints = 0;

            var currentStudent = GetUser(studentName);

            totalQuizPoints = currentStudent.StudentQuizResults.Sum(x => x.PointsTotal);

            return totalQuizPoints;
        }

        public string CalculateStudentPercentProgress(string studentName)
        {
            var progress = "";

            var currentStudent = GetUser(studentName);

            if (currentStudent.Rank.ParentRankId == null)
            {
                return "100%";
            }

            var currentRank = currentStudent.Rank;
            var nextRank = currentStudent.Rank.ParentRank;
            
            var currentStudentPoints = CalculateStudentTotalPoints(currentStudent.UserName);

            var normalizedStudentPoints = currentStudentPoints - currentRank.RequiredPoints;

            int percentageFromNext = (100*normalizedStudentPoints)/(nextRank.RequiredPoints-currentRank.RequiredPoints);

            return percentageFromNext.ToString() + "%";
        }

        public GenericOperationResult<StudentQuizResult> GetStudentQuizResult(int resultId)
        {
            var result = OperationResult.GetGenericResultObject<StudentQuizResult>();

            try
            {
                var quizResult = _quizResultRepository.GetItem(resultId);

                if (quizResult == null)
                {
                    result.AddMessage("Не постои резултати со идентификатор : "+ resultId);
                    return result;
                }
                else
                {
                    result.SetSuccess();
                    result.SetData(quizResult);
                    return result;
                }
            }
            catch(Exception e)
            {
                result.SetException();
                result.AddExceptionMessage(e.Message);
                result.AddMessage(e.Message);
                return result;
            }
        }

        /// <summary>
        /// Update the number of location views for the user.
        /// </summary>
        /// <param name="userName">The name of the user accessing the location details</param>
        /// <returns>Boolean value indicating the success of the operation</returns>
        public bool UpdateUserViewedLocation(string userName)
        {
            try
            {
                var user = _userRepository.GetUser(userName);

                if (user == null)
                {
                    return false;
                }

                if (user.OpenedLocationDetails != null)
                {
                    user.OpenedLocationDetails = user.OpenedLocationDetails + 1;
                }
                else
                {
                    user.OpenedLocationDetails = 1;
                }

                _userRepository.UpdateUser(user);

                return true;
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// Update the number of resources the user has accessed.
        /// </summary>
        /// <param name="userName">The username of the usr making the request for the resource</param>
        /// <returns></returns>
        public bool UpdateUserViewedResource(string userName)
        {
            try
            {
                var user = _userRepository.GetUser(userName);

                if (user == null)
                {
                    return false;
                }

                if (user.OpenedResources != null)
                {
                    user.OpenedResources = user.OpenedResources + 1;
                }
                else
                {
                    user.OpenedResources =  1;
                    
                }

                _userRepository.UpdateUser(user);

                return true;
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }

        public bool GeneralUpdateUser(User user)
        {
            try
            {
                _userRepository.UpdateUser(user);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}