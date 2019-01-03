using System.Collections.Generic;
using GeoGrafija.ResultClasses;
using Model;
using Services.ResultClasses;


namespace Services.Interfaces
{
    public interface IUserService
    {
        bool CheckCredentials(User User);
        void SignIn(string UserName, bool persistance);
        void SignOut();

        RegisterResult RegisterUser(User model);

        List<User> GetFilteredUsers(string FilterOutRole);
        List<User> GetUsersByRole(string roleName);
        List<User> GetUsers();
        
        User GetUser(int UserID);
        User GetUser(string UserName);

        #region Student
        
        GenericOperationResult<User> SetStudentTeacher(string studentName, int teacherId);

        int CalculateStudentRank(string studentName);
        int CalculateStudentTotalPoints(string studentName);
        int CalculateStudentTotalQuizPoints(string studentName);
        string CalculateStudentPercentProgress(string studentName);

        GenericOperationResult<StudentQuizResult> GetStudentQuizResult(int resultId);

        #endregion
        
        /// <summary>
        /// Update the number of location views for the user.
        /// </summary>
        /// <param name="userName">The name of the user accessing the location details</param>
        /// <returns>Boolean value indicating the success of the operation</returns>
        bool UpdateUserViewedLocation(string userName);

        /// <summary>
        /// Update the number of resources the user has accessed.
        /// </summary>
        /// <param name="userName">The username of the usr making the request for the resource</param>
        /// <returns></returns>
        bool UpdateUserViewedResource(string userName);

        bool GeneralUpdateUser(User user);
    }
}

