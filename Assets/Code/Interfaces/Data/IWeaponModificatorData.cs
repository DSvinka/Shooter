using Code.Managers;
using UnityEngine;

namespace Code.Interfaces.Data
{
    public interface IWeaponModificatorData
    {
        GameObject ModificatorPrefab { get; }
        Sprite Icon { get; }
        
        Vector3 AdditionalPosition { get; }
    }
}