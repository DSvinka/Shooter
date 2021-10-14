using System;
using Code.Managers;
using Code.Utils.Extensions;

namespace Code.SaveData.Data
{
    [Serializable]
    internal sealed class EnemySaveData
    {
        public float Health;
        public float Armor;
        
        public EnemyManager.EnemyType EnemyType;

        public Vector3Serializable Position;
        public Vector3Serializable Rotation;
        
        public Vector3Serializable SpawnPosition;
        public Vector3Serializable SpawnRotation;

        public override string ToString() =>
            $"<color=green>EnemyType</color> {EnemyType}\n " +
            "\n" +
            $"<color=red>Health</color> {Health} \n " +
            $"<color=red>Armor</color> {Armor} \n " +
            "\n" +
            $"<color=red>Position</color> {Position} " +
            $"\n<color=red>Rotation</color> {Rotation}";
    }
}