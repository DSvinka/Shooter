using System;
using System.Collections.Generic;
using System.Linq;
using Code.Data.DataStores;
using Code.Factory;
using Code.Markers;
using Code.Services;
using Code.Views;
using RSG;
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

            var interactDict = new Dictionary<int, InteractView>();
            var interactViews = Object.FindObjectsOfType<InteractView>();
            if (interactViews.Length != 0)
                interactDict = interactViews.ToDictionary(interactView => interactView.gameObject.GetInstanceID());

            var poolService = new PoolService();
            var promiseTimer = new PromiseTimer();

            var playerFactory = new PlayerFactory(_data);
            var enemyFactory = new EnemyFactory(_data);
            var weaponFactory = new WeaponFactory();

            var inputInitialization = new InputInitialization();
            var enemyInitialization = new EnemyInitialization(_data, enemyFactory, enemySpawnMarkers);
            var playerInitialization = new PlayerInitialization(data.PlayerData, playerFactory, playerSpawn.transform);

            var promiseTimerController = new PromiseTimerController(promiseTimer);
            
            var inputController = new InputController();
            var playerHudController = new PlayerHudController(playerInitialization);
            var weaponHudController = new WeaponHudController(playerInitialization, data);
            
            var weaponController = new WeaponController(data, playerHudController, playerInitialization, weaponFactory, poolService, promiseTimer);
            var pickupController = new InteractController(weaponController, playerInitialization, interactDict);
            
            var enemyController = new EnemyController(enemyInitialization, playerInitialization);

            var playerController = new PlayerController(playerInitialization, playerHudController);
            var playerMovementController = new PlayerMovementController(playerInitialization);

            controllers.Add(promiseTimerController);
            
            controllers.Add(playerInitialization);
            controllers.Add(enemyInitialization);
            
            controllers.Add(inputController);
            controllers.Add(playerHudController);
            controllers.Add(weaponHudController);
            controllers.Add(weaponController);
            controllers.Add(pickupController);

            controllers.Add(enemyController);
            controllers.Add(playerController);
            controllers.Add(playerMovementController);
        }
    }
}