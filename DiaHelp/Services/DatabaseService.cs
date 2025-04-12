using DiaHelp.Infrastructure;
using DiaHelp.Interface;
using DiaHelp.Model;
using Microsoft.EntityFrameworkCore;
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
        public List<UserModel> GetAllUser() => context.Users.ToList();
        public UserModel GetUser(string username) => context.Users.FirstOrDefault(p => p.Username == username);
        public bool AddUser(UserModel userModel)
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
        public List<SugarModel> GetAllSugarNotes()
        {
            return context.SugarNotes.OrderByDescending(note => note.Date).ToList();
        }
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

        public async Task<List<SugarModel>> GetSugarNotesByPeriod(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await context.SugarNotes
                    .Where(n => n.Date >= startDate && n.Date <= endDate)
                    .OrderByDescending(n => n.Date) // Сортировка по дате (последние сверху)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка: {ex.Message}");
                return [];
            }
        }
    }
}
