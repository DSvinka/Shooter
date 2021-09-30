using System;
using Code.Views;
using UnityEngine;

namespace Code.Interfaces.Objects
{
    internal interface IBullet
    {
        event Action<BulletView, GameObject> OnCollision;
    }
}