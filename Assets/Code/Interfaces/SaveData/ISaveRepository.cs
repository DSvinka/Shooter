using System.Collections.Generic;
using Code.Controllers.Initialization;
using Code.Factory;
using Code.Interfaces.Models;
using Code.Models;

namespace Code.Interfaces.SaveData
{
    internal interface ISaveRepository
    {
        void Save();
        bool TryLoad();
        bool CheckSaveFile();
    }
}