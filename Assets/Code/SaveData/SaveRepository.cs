using System;
using System.IO;
using System.Linq;
using Code.Controllers.Initialization;
using Code.Data;
using Code.Factory;
using Code.Interfaces.Models;
using Code.Interfaces.SaveData;
using Code.Managers;
using Code.SaveData.Data;
using UnityEngine;
using Object = UnityEngine.Object;

// TODO: Сделать так чтобы локация тоже загружалась.+
namespace Code.SaveData
{
    internal sealed class SaveRepository: ISaveRepository
    {
        private readonly IData<GameSaveData> _data;
        private readonly PlayerInitialization _playerInitialization;
        private readonly EnemyInitialization _enemyInitialization;
        private readonly ZombieFactory _zombieFactory;
        private readonly WeaponFactory _weaponFactory;

        private const string _folderName = "Saves";
        private const string _fileName = "save.dat";
        private readonly string _path;

        public SaveRepository(PlayerInitialization playerInitialization, ZombieFactory zombieFactory, WeaponFactory weaponFactory, EnemyInitialization enemyInitialization)
        {
            _data = new JsonData<GameSaveData>();
            _path = Path.Combine(Application.dataPath, _folderName);

            _playerInitialization = playerInitialization;
            _enemyInitialization = enemyInitialization;
            _zombieFactory = zombieFactory;
            _weaponFactory = weaponFactory;
        }

        public void Save()
        {
            if (!Directory.Exists(Path.Combine(_path)))
                Directory.CreateDirectory(_path);

            var player = _playerInitialization.GetPlayer();
            var enemies = _enemyInitialization.GetEnemies();

            var enemyLength = enemies.Count;
            var enemyArray = new IEnemyModel[enemyLength];
            var enemyDictArray = enemies.ToArray();
            for (var i = 0; i < enemyLength; i++)
            {
                enemyArray[i] = enemyDictArray[i].Value;
            } 

            var weapon = player.Weapon;
            var transform = player.Transform;

            Weapon weaponSave;
            if (weapon != null)
            {
                weaponSave = new Weapon()
                {
                    PathToData = weapon.Data.Path,
                    AmmoInClip = weapon.BulletsLeft // TODO: Нужно использовать это поле.
                };
            }
            else
            {
                weaponSave = new Weapon();
            }

            var savePlayer = new PlayerSaveData
            {
                Position = transform.position,
                Rotation = transform.rotation.eulerAngles,
                Weapon = weaponSave,
                
                Score = player.Score,
                Health = player.Health,
                Armor = player.Armor,
            };

            var enemySaveDatas = new EnemySaveData[enemyLength];
            for (var i = 0; i < enemyLength; i++)
            {
                var enemyModel = enemyArray[i];
                enemySaveDatas[i] = new EnemySaveData
                {
                    Health = enemyModel.Health,
                    Armor = enemyModel.Armor,
                    EnemyType = enemyModel.Data.EnemyType,
                    
                    Position = enemyModel.Transform.position,
                    Rotation = enemyModel.Transform.rotation.eulerAngles,
                    
                    SpawnPosition = enemyModel.SpawnPointPosition,
                    SpawnRotation = enemyModel.SpawnPointRotation,
                };
            }

            var saveGame = new GameSaveData
            {
                Player = savePlayer,
                EnemySaveDatas = enemySaveDatas,
                PhysicItemSaveDatas = null,
            };
            
            _data.Save(saveGame, Path.Combine(_path, _fileName));
        }
        
        public bool TryLoad()
        {
            var file = Path.Combine(_path, _fileName);
            if (!File.Exists(file))
                return false;

            var loadedGame = _data.Load(file);
            var loadedPlayer = loadedGame.Player;
            var loadedEnemies = loadedGame.EnemySaveDatas;
            
            #region Инициализация Противников

            var enemies = _enemyInitialization.GetEnemies();
            foreach (var enemy in enemies)
                Object.Destroy(enemy.Value.GameObject);

            enemies.Clear();
    
            if (loadedEnemies.Length != 0)
            {
                foreach (var enemySaveData in loadedEnemies)
                {
                    switch (enemySaveData.EnemyType)
                    {
                        case EnemyManager.EnemyType.Zombie:
                            var zombie = _zombieFactory.CreateZombie(enemySaveData.SpawnPosition, enemySaveData.SpawnRotation);
                            zombie.Transform.position = enemySaveData.Position;
                            zombie.Transform.eulerAngles = enemySaveData.Rotation;
                            enemies.Add(zombie.GameObject.GetInstanceID(), zombie);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(enemySaveData.EnemyType), "Тип противника не предусмотрен!");
                    }
                }
            }
            
            #endregion

            #region Инициализация Игрока
            
            var player = _playerInitialization.GetPlayer();
            
            // TODO: Почему-то игрок с некоторым шансом может заспавнится на точке спавна а не там где загрузился.
            player.Transform.position = loadedPlayer.Position;
            player.Transform.eulerAngles = loadedPlayer.Rotation;

            player.Health = loadedPlayer.Health;
            player.Armor = loadedPlayer.Armor;
            player.Score = loadedPlayer.Score;
            
            var weapon = loadedPlayer.Weapon;
            if (!string.IsNullOrEmpty(weapon.PathToData))
            {
                var weaponData = AssetPath.Load<WeaponData>(weapon.PathToData);
                var weaponView = _weaponFactory.CreateWeapon(weaponData);
                player.DefaultWeapon = weaponView;
            }

            #endregion

            return true;
        }

        public bool CheckSaveFile()
        {
            var file = Path.Combine(_path, _fileName);
            return File.Exists(file);
        }
    }
}