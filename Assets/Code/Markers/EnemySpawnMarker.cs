using Code.Managers;
using UnityEngine;

namespace Code.Markers
{
    internal sealed class EnemySpawnMarker : MonoBehaviour
    {
        [SerializeField] private EnemyManager.EnemyType _enemyType;

        public EnemyManager.EnemyType EnemyType => _enemyType;
    }
}