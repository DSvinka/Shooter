using static Code.Utils.Extensions.Methods;
using UnityEngine;

namespace Code.Builders
{
    internal sealed class GameObjectPhysicsBuilder : GameObjectBuilder
    {
        public GameObjectPhysicsBuilder(GameObject gameObject) : base(gameObject) {}

        public GameObjectPhysicsBuilder BoxCollider()
        {
            _gameObject.GetOrAddComponent<BoxCollider>();
            return this;
        }

        public GameObjectPhysicsBuilder Rigidbody(float mass)
        {
            var component = _gameObject.GetOrAddComponent<Rigidbody>();
            component.mass = mass;
            return this;
        }
    }
}