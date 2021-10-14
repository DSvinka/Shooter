using Code.Interfaces.Bridges;
using Code.Interfaces.Data;
using Code.Interfaces.Views;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Interfaces.Models
{
    public interface IEnemyModel: IUnitModel
    {
        float AttackCooldown { get; set; }
        
        IEnemyView View { get; }
        IEnemyData Data { get; }
        
        IAttack AttackBridge { get; }
        IMove MoveBridge { get; }
        
        NavMeshAgent NavMeshAgent { get; }

        void SetComponents(NavMeshAgent navMeshAgent, AudioSource audioSource);
        void SetBridges(IMove move, IAttack attack);
    }
}