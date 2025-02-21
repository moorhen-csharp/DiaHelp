using DiaHelp.Infrastructure;
using DiaHelp.Interface;
using DiaHelp.Model;
using System.Diagnostics;

namespace DiaHelp.Services
{
    public class DatabaseService(ApplicationContext context) : IDatabaseService
    {
        public bool ClearAllSugarNotes()
        {
            try
            {
                var allNotes = context.SugarNotes.ToList();
                context.SugarNotes.RemoveRange(allNotes);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка при удалении записей: {ex.Message}");
                return false;
            }
        }
        //ЮЗЕР
        public List<User> GetAllUser() => context.Users.ToList();
        public User GetUser(string username) => context.Users.FirstOrDefault(p => p.Username == username);
        public bool AddUser(User userModel)
        {
            try
            {
                if (context.Users.Any(u => u.Email == userModel.Email))
                {
                    return false;
                }

                userModel.Password = BCrypt.Net.BCrypt.HashPassword(userModel.Password);
                context.Users.Add(userModel);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        //САХАР
        public List<SugarModel> GetAllSugarNotes() => context.SugarNotes.ToList();
        public bool AddSugarNote(SugarModel sugarNote)
        {
            try
            {
                sugarNote.Id = context.SugarNotes.Count() == 0 ? 1 : context.SugarNotes.Max(p => p.Id) + 1;
                context.SugarNotes.Add(sugarNote);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
