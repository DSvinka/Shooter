﻿namespace Code.Interfaces.Data
{
    public interface IEnemyMeleeData: IEnemyData
    {
        public float AttackDistance { get; }
        public float AttackRate { get; }
        public float AttackDamage { get; }
    }
}