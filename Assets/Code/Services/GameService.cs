using Code.Controllers.Initialization;
using Code.Data.DataStores;
using UnityEngine;

namespace Code.Services
{
    internal sealed class GameService
    {
        public Controllers.Starter.Controllers Start(DataStore data)
        {
            var controllers = CreateControllers();
            
            InitGame(controllers, data);
            StartGame(controllers);

            return controllers;
        }

        private GameInitialization InitGame(Controllers.Starter.Controllers controllers, DataStore data)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            return new GameInitialization(controllers, data);
        }
        
        private void StartGame(Controllers.Starter.Controllers controllers)
        {
            controllers.Initialization();
        }

        private Controllers.Starter.Controllers CreateControllers()
        {
            return new Controllers.Starter.Controllers();
        }
    }
}