using System;
using System.Collections.Generic;
using System.Linq;
using Code.Data.DataStores;
using Code.Factory;
using Code.Markers;
using Code.SaveData;
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
            
            var playerFactory = new PlayerFactory(_data);
            var enemyFactory = new EnemyFactory(_data);
            var weaponFactory = new WeaponFactory();
            var zombieFactory = new ZombieFactory(_data, enemyFactory);
            
            var playerInitialization = new PlayerInitialization(data.PlayerData, playerFactory, playerSpawn.transform);
            playerInitialization.Initialization();
            
            var saveRepository = new SaveRepository();
            var enemies = saveRepository.Load(playerInitialization, zombieFactory, weaponFactory);

            var interactDict = new Dictionary<int, InteractView>();
            var interactViews = Object.FindObjectsOfType<InteractView>();
            if (interactViews.Length != 0)
                interactDict = interactViews.ToDictionary(interactView => interactView.gameObject.GetInstanceID());

            var poolService = new PoolService();
            var promiseTimer = new PromiseTimer();

            var inputInitialization = new InputInitialization();
            var enemyInitialization = new EnemyInitialization(_data, enemyFactory, enemySpawnMarkers);
            if (enemies != null)
                enemyInitialization.SetEnemies(enemies);

            var promiseTimerController = new PromiseTimerController(promiseTimer);
            
            var inputController = new InputController();
            var playerHudController = new PlayerHudController(playerInitialization);
            var weaponHudController = new WeaponHudController(playerInitialization, data);
            
            var weaponController = new WeaponController(data, playerHudController, playerInitialization, weaponFactory, poolService, promiseTimer);
            var pickupController = new InteractController(weaponController, playerInitialization, interactDict);
            
            var enemyController = new EnemyController(enemyInitialization, playerInitialization);

            var playerController = new PlayerController(playerInitialization, playerHudController);
            var playerMovementController = new PlayerMovementController(playerInitialization);

            var savesController = new SavesController(saveRepository, playerInitialization, enemyInitialization);
            controllers.Add(savesController);

            controllers.Add(promiseTimerController);
            
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