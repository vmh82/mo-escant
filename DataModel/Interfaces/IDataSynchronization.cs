using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.Interafaces
{
    public interface IDataSynchronization
    {
        bool IsDeleted { get; }
        DateTime UpdatedDate { get; }
    }
}
