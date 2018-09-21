using System;
using System.Collections.Generic;

namespace ViewModelUtils.Interfaces
{
    public interface IHasTyresList<T>
    {
        List<T> TyresList { get; }
        event EventHandler TyresListUpdated;
    }
}