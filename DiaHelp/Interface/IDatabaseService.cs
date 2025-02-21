using DiaHelp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaHelp.Interface
{
    public interface IDatabaseService
    {

        List<User> GetAllUser();
        User GetUser(string username);
        bool AddUser(User userModel);

        List<SugarModel> GetAllSugarNotes();
        bool AddSugarNote(SugarModel sugarNote);
        bool ClearAllSugarNotes();
    }
}
