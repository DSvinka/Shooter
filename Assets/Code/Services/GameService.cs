using System;
using Code.Controllers.Initialization;
using Code.Data.DataStores;
using Code.Factory;
using Code.Markers;
using Code.SaveData;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Services
{
    internal sealed class GameService
    {
        public Controllers.Starter.Controllers Start(DataStore data)
        {
            var controllers = CreateControllers();
            
            InitGame(controllers, data);
            
            controllers.Initialization();

            return controllers;
        }

        private GameInitialization InitGame(Controllers.Starter.Controllers controllers, DataStore data)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            return new GameInitialization(controllers, data);
        }

        private Controllers.Starter.Controllers CreateControllers()
        {
            return new Controllers.Starter.Controllers();
        }
    }
}