using System;
using Code.Data;
using Code.Interfaces.Factory;
using Code.Models;
using Code.Views;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Factory
{
    internal sealed class WeaponFactory: IFactory, IWeaponFactory
    {
        public WeaponModel CreateWeapon(WeaponView view, WeaponData data)
        {
            var gameObject = view.gameObject;
            var particleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
            if (particleSystem == null)
                throw new Exception($"ParticleSystem не найден в {gameObject.gameObject.name}");
            
            if (!gameObject.TryGetComponent(out AudioSource audioSource))
                throw new Exception($"AudioSource не найден в {gameObject.gameObject.name}");

            var model = new WeaponModel(view, data, particleSystem, audioSource);

            return model;
        }
        
        public WeaponModel CreateWeapon(WeaponData weapon)
        {
            var gameObject = Object.Instantiate(weapon.WeaponPrefab, null, true);
            
            if (!gameObject.TryGetComponent(out WeaponView view))
                throw new Exception($"IEnemyView не найден в {gameObject.gameObject.name}");

            var model = CreateWeapon(view, weapon);

            return model;
        }
    }
}