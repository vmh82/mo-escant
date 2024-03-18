using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.Interafaces
{
    public interface IAuditableEntity
    {
        string Id { get; set; }
        string CreatedBy { get; set; }
        string UpdatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime UpdatedDate { get; set; }
        bool IsDeleted { get; set; }
    }
}
