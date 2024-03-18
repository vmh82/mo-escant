using DataModel.Interafaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel
{
    public class AuditableEntity : IAuditableEntity
    {
        public string Id { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
