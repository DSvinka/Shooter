using static Code.Utils.Extensions.Methods;
using UnityEngine;

namespace Code.Builders
{
    internal sealed class GameObjectVisualBuilder: GameObjectBuilder
    {
        public GameObjectVisualBuilder(GameObject gameObject) : base(gameObject) {}

        public GameObjectVisualBuilder Name(string name)
        {
            _gameObject.name = name;
            return this;
        }

        public GameObjectVisualBuilder Sprite(Sprite sprite)
        {
            var component = _gameObject.GetOrAddComponent<SpriteRenderer>();
            component.sprite = sprite;
            return this;
        }
    }
}