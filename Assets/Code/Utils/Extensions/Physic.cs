using UnityEngine;

namespace Code.Utils.Extensions
{
    internal static class Physic
    {
        public static void DisableAllPhysics(this GameObject gameObject, bool disableCollision)
        {
            if (gameObject.TryGetComponent(out Rigidbody rigidbody))
            {
                if (disableCollision)
                {
                    rigidbody.isKinematic = true;
                }
                else
                {
                    rigidbody.useGravity = false;
                    rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                }
            }
        }
        public static void DisableAllCollision(this GameObject gameObject)
        {
            var colliders = gameObject.GetComponents<Collider>();
            if (colliders.Length != 0)
            {
                foreach (var collider in colliders)
                {
                    collider.enabled = false;
                }
            }
        }
    }
}