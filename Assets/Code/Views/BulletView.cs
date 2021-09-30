using System;
using Code.Interfaces.Objects;
using UnityEngine;

namespace Code.Views
{
    internal sealed class BulletView : MonoBehaviour, IBullet
    {
        public event Action<BulletView, GameObject> OnCollision = delegate(BulletView bullet, GameObject hit) {};

        private void OnCollisionEnter(Collision other)
        {
            OnCollision.Invoke(this, other.gameObject);
        }
    }
}