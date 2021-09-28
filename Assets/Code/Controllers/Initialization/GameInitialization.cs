using System;
using Code.Data.DataStores;
using Code.Factory;
using Code.Markers;
using Object = UnityEngine.Object;

namespace Code.Controllers.Initialization
{
    internal sealed class GameInitialization
    {
        private Starter.Controllers _controllers;
        private DataStore _data;

        public GameInitialization(Starter.Controllers controllers, DataStore data)
        {
            _controllers = controllers;
            _data = data;

            var enemySpawnMarkers = Object.FindObjectsOfType<EnemySpawnMarker>();
            var playerSpawn = Object.FindObjectOfType<PlayerSpawnMarker>();
            if (playerSpawn == null)
                throw new Exception("Спавнер игрока отсуствует!");

            var playerFactory = new PlayerFactory(data);
            var enemyFactory = new EnemyFactory(data);

            var inputInitialization = new InputInitialization();
            var enemyInitialization = new EnemyInitialization(data, enemyFactory, enemySpawnMarkers);
            var playerInitialization = new PlayerInitialization(playerFactory, playerSpawn.transform.position);

            var inputController = new InputController();
            var playerHudController = new PlayerHudController(playerInitialization);
            var weaponController = new WeaponController(playerHudController, playerInitialization);
            var enemyController = new EnemyController(enemyInitialization);
            
            var playerController = new PlayerController(data.PlayerData, playerInitialization, weaponController, playerHudController);
            var playerMovementController = new PlayerMovementController(_data.PlayerData, playerInitialization);

            controllers.Add(playerInitialization);
            controllers.Add(enemyInitialization);
            
            controllers.Add(inputController);
            controllers.Add(playerHudController);
            controllers.Add(weaponController);
            controllers.Add(enemyController);
            
            controllers.Add(playerController);
            controllers.Add(playerMovementController);
        }
    }
}