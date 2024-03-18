using DataModel.DTO;
using DataModel.DTO.Compras;
using System;
using System.Collections.Generic;
using System.Text;

namespace Escant_App.Models
{
    public class ReportSummaryRequest
    {
        public SelectReportPeriod ReportPeriod { get; set; }
        public List<Orden> Orders { get; set; }
        public List<Orden> PurchaseOrders { get; set; }
    }
}
