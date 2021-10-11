using Code.Controllers.Initialization;
using Code.Data;
using Code.Interfaces;
using Code.Models;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Controllers
{
    internal sealed class PlayerController: IController, IInitialization, ICleanup
    {
        private readonly PlayerInitialization _initialization;
        private readonly PlayerHudController _hudController;
        
        private PlayerModel _player;
        
        public PlayerController(PlayerInitialization initialization, PlayerHudController hudController)
        {
            _initialization = initialization;
            _hudController = hudController;
        }
        
        public void Initialization()
        {
            _player = _initialization.GetPlayer();

            var view = _player.View;
            view.OnArmored += AddArmor;
            view.OnHealing += AddHealth;
            view.OnDamage += AddDamage;
        }

        public void Cleanup()
        {
            var view = _player.View;
            if (view != null)
            {
                view.OnArmored -= AddArmor;
                view.OnHealing -= AddHealth;
                view.OnDamage -= AddDamage;
            }
        }

        private void AddHealth(GameObject healer, int _, float health)
        {
            _player.Health += health;
            if (_player.Health > _player.Data.MaxHealth)
                _player.Health = _player.Data.MaxHealth;
        }

        private void AddArmor(GameObject armorer, int _, float armor)
        {
            _player.Armor += armor;
            if (_player.Armor > _player.Data.MaxArmor)
                _player.Armor = _player.Data.MaxArmor;
        }

        private void AddDamage(GameObject attacker, int _, float damage)
        {
            if (_player.Armor > damage)
            {
                _player.Armor -= damage;
                _hudController.SetArmor((int) _player.Armor);
                return;
            }

            if (_player.Armor != 0)
            {
                damage -= _player.Armor;
                _player.Armor = 0;
                _hudController.SetArmor(0);
            }

            _player.Health -= damage;
            if (_player.Health <= 0)
            {
                _player.Health = 0;
                Death();
            }
            _hudController.SetHealth((int) _player.Health);
        }
        
        private void Death()
        {
            Object.Destroy(_player.View);
        }
    }
}