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

        List<UserModel> GetAllUser();
        UserModel GetUser(string email);
        bool AddUser(UserModel userModel);

        List<SugarModel> GetAllSugarNotes();
        bool AddSugarNote(SugarModel sugarNote);
        bool ClearAllSugarNotes();
    }
}
