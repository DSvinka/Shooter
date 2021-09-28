using System;
using Code.Controllers.Initialization;
using Code.Data;
using Code.Interfaces;
using Code.Interfaces.Controllers;
using Code.Interfaces.Units;
using Code.Views;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Controllers
{
    internal sealed class PlayerController: IController, IInitialization, ICleanup, IUnitController
    {
        private readonly PlayerData _data;
        private readonly PlayerInitialization _initialization;
        private readonly WeaponController _weaponController;
        private readonly PlayerHudController _hudController;

        private PlayerView _playerView;
        
        public float Health { get; private set; }
        public float Armor { get; private set; }

        public PlayerController(PlayerData data, PlayerInitialization initialization, WeaponController weaponController, PlayerHudController hudController)
        {
            _data = data;
            _initialization = initialization;
            _weaponController = weaponController;
            _hudController = hudController;
        }
        
        public void Initialization()
        {
            Health = _data.MaxHealth;
            Armor = _data.MaxArmor;
            _playerView = _initialization.GetPlayer();
            
            var weaponView = _playerView.Weapon;
            if (weaponView == null)
                throw new Exception("У игрока не найдено оружие.");
            
            // TODO: Заменить этот костыль на нормальный подбор оружия
            _weaponController.ChangeWeapon(weaponView);

            _playerView.OnArmored += AddArmor;
            _playerView.OnHealing += AddHealth;
            _playerView.OnDamage += AddDamage;
        }

        public void Cleanup()
        {
            if (_playerView != null)
            {
                _playerView.OnArmored -= AddArmor;
                _playerView.OnHealing -= AddHealth;
                _playerView.OnDamage -= AddDamage;
            }
        }

        private void AddHealth(GameObject healer, IUnit _, float health)
        {
            Health += health;
            if (Health > _data.MaxHealth)
                Health = _data.MaxHealth;
        }

        private void AddArmor(GameObject armorer, IUnit _, float armor)
        {
            Armor += armor;
            if (Armor > _data.MaxArmor)
                Armor = _data.MaxArmor;
        }

        private void AddDamage(GameObject attacker, IUnit _, float damage)
        {
            if (Armor > damage)
            {
                Armor -= damage;
                _hudController.SetArmor((int) Armor);
                return;
            }

            if (Armor != 0)
            {
                damage -= Armor;
                Armor = 0;
                _hudController.SetArmor(0);
            }

            Health -= damage;
            if (Health <= 0)
            {
                Health = 0;
                Death();
            }
            _hudController.SetHealth((int) Health);
        }
        
        private void Death()
        {
            Object.Destroy(_initialization.GetPlayer());
        }
    }
}