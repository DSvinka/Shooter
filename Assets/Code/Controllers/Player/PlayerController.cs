using Code.Controllers.Initialization;
using Code.Interfaces;
using Code.Models;
using Code.PlayerModifiers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Controllers.Player
{
    internal sealed class PlayerController: IController, IInitialization, ICleanup
    {
        private readonly PlayerInitialization _initialization;
        private readonly PlayerHudController _hudController;
        
        private PlayerModel _player;
        private PlayerModifier _playerModifier;
        
        public PlayerController(PlayerInitialization initialization, PlayerHudController hudController)
        {
            _initialization = initialization;
            _hudController = hudController;
        }
        
        public void Initialization()
        {
            _player = _initialization.GetPlayer();
            
            // TODO: (Временный костыль) Добавить подбор модификаторов
            _playerModifier = new PlayerModifier(_player);
            _playerModifier.Add(new SpeedModifier(_player, 5f, 10f));
            _playerModifier.Add(new ArmorModifier(_player, 150));
            _playerModifier.Add(new JumpModifier(_player, 5f));
            _playerModifier.Handle();
            
            _hudController.SetHealth((int) _player.Health);
            _hudController.SetArmor((int) _player.Armor);
            _hudController.SetScore(_player.Score);

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
            _hudController.SetHealth((int) _player.Health);
        }

        private void AddArmor(GameObject armorer, int _, float armor)
        {
            _player.Armor += armor;
            if (_player.Armor > _player.Data.MaxArmor)
                _player.Armor = _player.Data.MaxArmor;
            _hudController.SetArmor((int) _player.Armor);
        }

        private void AddDamage(GameObject attacker, Vector3 damagePosition, int _, float damage)
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
            _player.CanMove = false;
            if (_player.Weapon != null)
            {
                Object.Destroy(_player.Weapon.GameObject);
                _player.Weapon = null;
            }
            
            _player.GameObject.GetComponent<Rigidbody>().freezeRotation = false;
        }
    }
}