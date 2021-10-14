using System;
using Code.Utils.Extensions;

namespace Code.SaveData.Data
{
    [Serializable]
    internal sealed class PhysicItemSaveData
    {
        public string PathToData;
        public string Name;

        public Vector3Serializable Position;
        public Vector3Serializable Rotation;

        public override string ToString() =>
            $"<color=green>Name</color> {Name} \n" +
            $"<color=red>Position</color> {Position} \n" +
            $"<color=red>Rotation</color> {Rotation}";
    }
}