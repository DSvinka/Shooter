using System;
using Code.Data;
using Code.Interfaces.Factory;
using Code.Interfaces.Models;
using Code.Models;
using Code.Views;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Factory
{
    internal sealed class WeaponFactory: IFactory, IWeaponFactory
    {
        public WeaponModel CreateWeapon(WeaponData weapon)
        {
            var gameObject = Object.Instantiate(weapon.WeaponPrefab, null, true);
            
            var view = gameObject.GetComponent<WeaponView>();
            if (view == null)
                throw new Exception($"IEnemyView не найден в {gameObject.gameObject.name}");

            var particleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
            if (view == null)
                throw new Exception($"ParticleSystem не найден в {gameObject.gameObject.name}");
            
            var audioSource = gameObject.GetComponent<AudioSource>();
            if (view == null)
                throw new Exception($"AudioSource не найден в {gameObject.gameObject.name}");

            var enemyModel = new WeaponModel(view, weapon, particleSystem, audioSource);

            return enemyModel;
        }
    }
}