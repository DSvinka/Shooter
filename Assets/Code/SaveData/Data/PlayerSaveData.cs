using System;
using Code.Utils.Extensions;

namespace Code.SaveData.Data
{
    [Serializable]
    internal struct Weapon
    {
        public string PathToData;
        public int AmmoInClip;
    }
    
    [Serializable]
    internal sealed class PlayerSaveData
    {
        public int Score;
        
        public float Health;
        public float Armor;
        
        public Weapon Weapon;

        public Vector3Serializable Position;
        public Vector3Serializable Rotation;

        public override string ToString() =>
            $"<color=red>Health</color> {Health} \n " +
            $"<color=red>Armor</color> {Armor} \n " +
            "\n" +
            $"<color=red>Position</color> {Position} \n" +
            $"<color=red>Rotation</color> {Rotation}";
    }
}