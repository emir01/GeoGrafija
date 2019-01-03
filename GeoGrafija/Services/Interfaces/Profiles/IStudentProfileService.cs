using Model;

namespace Services.Interfaces.Profiles
{
    interface IStudentProfileService
    {
        int GetStudentRank(string studentUsername);
        int GetStudentRank(User student);
    }
}
