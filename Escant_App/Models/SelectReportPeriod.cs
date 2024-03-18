using System;
using System.Collections.Generic;
using System.Text;

namespace Escant_App.Models
{
    public class SelectReportPeriod
    {
        public PeriodType PeriodType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
