using Escant_App.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Escant_App.Models
{
    public enum SortDirection
    {
        [StringValue("asc")]
        None,

        [StringValue("asc")]
        Asc,

        [StringValue("desc")]
        Desc,
    }
}
