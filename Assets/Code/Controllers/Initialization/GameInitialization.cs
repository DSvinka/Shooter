using System;
using Code.Controllers.ObjectPool;
using Code.Data.DataStores;
using Code.Factory;
using Code.Markers;
using Code.Services;
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
            
            ServiceLocator.SetService(new PoolService());

            var playerFactory = new PlayerFactory(_data);
            var enemyFactory = new EnemyFactory(_data);
            var weaponFactory = new WeaponFactory();

            var inputInitialization = new InputInitialization();
            var enemyInitialization = new EnemyInitialization(_data, enemyFactory, enemySpawnMarkers);
            var playerInitialization = new PlayerInitialization(data.PlayerData, playerFactory, playerSpawn.transform);

            var inputController = new InputController();
            var playerHudController = new PlayerHudController(playerInitialization);
            var weaponController = new WeaponController(playerHudController, playerInitialization, weaponFactory);
            
            var enemyController = new EnemyController(enemyInitialization, playerInitialization);

            var playerController = new PlayerController(playerInitialization, weaponController, playerHudController, _data.StickGunData);
            var playerMovementController = new PlayerMovementController(playerInitialization);

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