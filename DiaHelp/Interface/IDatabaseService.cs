using DiaHelp.Model;

namespace DiaHelp.Interface
{
    public interface IDatabaseService
    {

        List<UserModel> GetAllUser();
        UserModel GetUser(string email);
        bool AddUser(UserModel userModel);

        bool UpdateUser(UserModel userModel);
        Task<List<SugarModel>> GetAllSugarNotesAsync();
        bool AddSugarNote(SugarModel sugarNote);

        bool ClearAllSugarNotes();

        Task<List<SugarModel>> GetSugarNotesByPeriod(DateTime startDate, DateTime endDate);

        Task<List<FoodModel>> GetAllFoodNotesAsync();
        Task<List<FoodModel>> GetFoodNotesByPeriod(DateTime startDate, DateTime endDate);
        bool ClearAllFoodNotes();
    }
}
