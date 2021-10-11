using Code.Managers;
using UnityEngine;

namespace Code.Interfaces.Data
{
    public interface IWeaponModificatorData
    {
        GameObject ModificatorPrefab { get; }
        Vector3 AdditionalPosition { get; }
        WeaponManager.WeaponType WeaponTypes { get; }
    }
}