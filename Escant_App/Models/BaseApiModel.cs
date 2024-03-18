using System;
using System.Collections.Generic;
using System.Text;

namespace Escant_App.Models
{
    public class BaseApiModel
    {
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
