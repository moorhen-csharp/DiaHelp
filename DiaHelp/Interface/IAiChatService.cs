using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaHelp.Interface
{
    public interface IAiChatService
    {
        Task<string> GetDiabetesResponseAsync(string message);
    }
}
