﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaHelp.Model
{
    public class SugarModel
    {
       public int Id { get; set; }
        public double SugarLevel { get; set; }
        public string MeasurementTime { get; set; }
        public string HealthType { get; set; }
        public int InsulinDose { get; set; }
        public DateTime Date { get; set; }
    }
}
