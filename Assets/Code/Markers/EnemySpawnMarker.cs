using Code.Data;
using Code.Interfaces.Units;
using UnityEngine;

namespace Code.Markers
{
    public enum Enemies
    {
        Target
    }

    internal sealed class EnemySpawnMarker : MonoBehaviour
    {
        [SerializeField] private Enemies _enemyType;

        public Enemies EnemyType => _enemyType;
    }
}