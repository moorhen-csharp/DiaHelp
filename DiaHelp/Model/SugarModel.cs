using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaHelp.Model
{
    public class SugarModel
    {
       public int Id { get; set; }
       public decimal SugarLevel { get; set; }
       public string MeasurementTime { get; set; }
       public DateTime Date { get; set; }
    }
}
