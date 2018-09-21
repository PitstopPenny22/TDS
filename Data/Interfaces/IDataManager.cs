using System;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IDataManager
    {
        Task LoadAndParseData();
        event EventHandler TracksDetailsPopulated;
        event EventHandler TyresDetailsPopulated;
    }
}