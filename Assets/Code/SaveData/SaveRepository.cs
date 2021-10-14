using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Code.Controllers;
using Code.Controllers.Initialization;
using Code.Data;
using Code.Factory;
using Code.Interfaces.Models;
using Code.Interfaces.SaveData;
using Code.Managers;
using Code.Models;
using Code.SaveData.Data;
using UnityEngine;

namespace Code.SaveData
{
    internal sealed class SaveRepository: ISaveRepository
    {
        private readonly IData<GameSaveData> _data;

        private const string _folderName = "Saves";
        private const string _fileName = "data.bat";
        private readonly string _path;

        public SaveRepository()
        {
            _data = new JsonData<GameSaveData>();
            _path = Path.Combine(Application.dataPath, _folderName);
        }

        public void Save(PlayerModel player, IEnemyModel[] enemyModels)
        {
            if (!Directory.Exists(Path.Combine(_path)))
                Directory.CreateDirectory(_path);

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
                
                Health = player.Health,
                Armor = player.Armor,
            };

            var enemySaveDatas = new EnemySaveData[enemyModels.Length];
            for (var i = 0; i < enemyModels.Length; i++)
            {
                var enemyModel = enemyModels[i];
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

        public Dictionary<int, IEnemyModel> Load(PlayerInitialization playerInitialization, ZombieFactory zombieFactory, WeaponFactory weaponFactory)
        {
            var file = Path.Combine(_path, _fileName);
            if (!File.Exists(file))
                return null;

            var loadedGame = _data.Load(file);
            var loadedPlayer = loadedGame.Player;
            var loadedEnemies = loadedGame.EnemySaveDatas;
            
            #region Инициализация Противников

            var enemies = new Dictionary<int, IEnemyModel>();
            if (loadedEnemies.Length != 0)
            {
                foreach (var enemySaveData in loadedEnemies)
                {
                    switch (enemySaveData.EnemyType)
                    {
                        case EnemyManager.EnemyType.Zombie:
                            var zombie = zombieFactory.CreateZombie(enemySaveData.SpawnPosition, enemySaveData.SpawnRotation);
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
            
            var player = playerInitialization.GetPlayer();

            player.Transform.position = loadedPlayer.Position;
            player.Transform.eulerAngles = loadedPlayer.Rotation;
            
            player.Health = loadedPlayer.Health;
            player.Armor = loadedPlayer.Health;
            
            var weapon = loadedPlayer.Weapon;
            if (!string.IsNullOrEmpty(weapon.PathToData))
            {
                var weaponData = AssetPath.Load<WeaponData>(weapon.PathToData);
                var weaponView = weaponFactory.CreateWeapon(weaponData);
                player.DefaultWeapon = weaponView;
            }

            #endregion

            return enemies;
        }
    }
}