using DiaHelp.Model;

namespace DiaHelp.Interface
{
    public interface IDatabaseService
    {

        List<UserModel> GetAllUser();
        UserModel GetUser(string email);
        bool AddUser(UserModel userModel);

        List<SugarModel> GetAllSugarNotes();
        bool AddSugarNote(SugarModel sugarNote);

        bool ClearAllSugarNotes();
    }
}
