using Code.Interfaces.Bridges.Weapon.Shoots;
using Code.Interfaces.Views;
using Code.Models;
using UnityEngine;

namespace Code.Bridges.Weapon.Shoots.Damage
{
    internal sealed class NormalDamage: IDamage
    {
        private PlayerModel _player;
        private WeaponModel _weapon;
        
        public NormalDamage(PlayerModel player, WeaponModel weapon)
        {
            _player = player;
            _weapon = weapon;
        }
        
        public void Damage(GameObject gameObject, Vector3 shootPoint)
        {
            if (gameObject.TryGetComponent(out IUnitView unitView))
                unitView.AddDamage(_player.GameObject, shootPoint, _weapon.Data.Damage);
        }
    }
}