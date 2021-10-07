using Code.Managers;
using UnityEngine;

namespace Code.Interfaces.Data
{
    public interface IEnemyData: IUnitData
    {
        public float AttackDistance { get; }
        public float AttackRate { get; }
        public float AttackDamage { get; }
        
        public AudioClip AttackClip { get; }
        public AudioClip GetDamageClip { get; }
        
        public float MaxRandomSoundPitch { get; }
        public float MinRandomSoundPitch { get; }
        
        public EnemyManager.EnemyType EnemyType { get; }
    }
}