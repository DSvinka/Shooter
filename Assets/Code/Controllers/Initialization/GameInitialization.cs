using System;
using System.Collections.Generic;
using System.Linq;
using Code.Controllers.Enemy;
using Code.Controllers.Player;
using Code.Controllers.Weapon;
using Code.Data.DataStores;
using Code.Factory;
using Code.Listeners;
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
            
            var playerFactory = new PlayerFactory(_data.UnitStore, _data.UIStore);
            var enemyFactory = new EnemyFactory();
            var weaponFactory = new WeaponFactory();
            var zombieFactory = new ZombieFactory(_data.UnitStore, enemyFactory);
            var uiFactory = new UIFactory(_data.UIStore);
            
            var enemyInitialization = new EnemyInitialization(_data.UnitStore, enemyFactory, enemySpawnMarkers);
            var playerInitialization = new PlayerInitialization(_data.UnitStore.PlayerData, playerFactory, playerSpawn.transform);
            var saveRepository = new SaveRepository(playerInitialization, zombieFactory, weaponFactory, enemyInitialization);
            
            playerInitialization.Initialization(saveRepository.CheckSaveFile());
            saveRepository.TryLoad();

            var interactDict = new Dictionary<int, InteractView>();
            var interactViews = Object.FindObjectsOfType<InteractView>();
            if (interactViews.Length != 0)
                interactDict = interactViews.ToDictionary(interactView => interactView.gameObject.GetInstanceID());

            var unitListener = new UnitListener();
            var poolService = new PoolService();
            var notifyMessageBroker = new MessageBrokerService<string>();
            var promiseTimer = new PromiseTimer();

            var inputInitialization = new InputInitialization();
            var uiInitialization = new UIInitialization(uiFactory);

            var promiseTimerController = new PromiseTimerController(promiseTimer);
            
            var inputController = new InputController();
            var playerHudController = new PlayerHudController(uiInitialization, _data.UIStore, notifyMessageBroker, promiseTimer);
            var weaponHudController = new WeaponHudController(playerInitialization, uiInitialization, _data.UIStore);
            var enemyHudController = new EnemyHudController(_data.UIStore, playerInitialization, unitListener, poolService, promiseTimer);
            
            var weaponController = new WeaponController(_data.WeaponStore, playerHudController, playerInitialization, weaponFactory, poolService, promiseTimer);
            var pickupController = new InteractController(weaponController, playerInitialization, interactDict);
            
            var enemyController = new EnemyController(enemyInitialization, playerInitialization, playerHudController, unitListener, notifyMessageBroker);
            var escapeMenuController = new EscapeMenuController(saveRepository, playerInitialization, uiInitialization, enemyInitialization);
            
            var playerController = new PlayerController(playerInitialization, playerHudController);
            var playerMovementController = new PlayerMovementController(playerInitialization);

            controllers.Add(promiseTimerController);
            
            controllers.Add(enemyInitialization);
            controllers.Add(uiInitialization);
            
            controllers.Add(inputController);
            controllers.Add(playerHudController);
            controllers.Add(weaponHudController);
            controllers.Add(enemyHudController);
            controllers.Add(weaponController);
            controllers.Add(pickupController);

            controllers.Add(enemyController);
            controllers.Add(playerController);
            controllers.Add(playerMovementController);
            controllers.Add(escapeMenuController);
        }
    }
}