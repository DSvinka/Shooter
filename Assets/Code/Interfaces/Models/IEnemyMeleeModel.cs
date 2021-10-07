using Code.Interfaces.Data;
using Code.Interfaces.Views;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Interfaces.Models
{
    public interface IEnemyMeleeModel: IUnitMeleeModel
    {
        IEnemyMeleeData Data { get; }
        IEnemyMeleeView View { get; }
        NavMeshAgent NavMeshAgent { get; }
        AudioSource AudioSource { get; }
    }
}