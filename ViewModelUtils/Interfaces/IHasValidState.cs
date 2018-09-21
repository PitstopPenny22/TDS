using System;

namespace ViewModelUtils.Interfaces
{
    public interface IHasValidState
    {
        bool IsValid { get; }
        event EventHandler IsValidChanged;
    }
}