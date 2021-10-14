using System;

namespace Code.SaveData.Data
{
    [Serializable]
    internal sealed class GameSaveData
    {
        public PlayerSaveData Player;
        public EnemySaveData[] EnemySaveDatas;
        public PhysicItemSaveData[] PhysicItemSaveDatas;

        public override string ToString() => 
            $"<color=green>Player</color> {Player} \n" +
            "\n" +
            $"<color=green>Enemies Count</color> {EnemySaveDatas.Length} \n" +
            $"<color=green>Physic Items Count</color> {PhysicItemSaveDatas.Length} \n";

    }
}