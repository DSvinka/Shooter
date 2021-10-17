using UnityEngine;

namespace Code.Interfaces.Data
{
    public interface IWeaponModificatorData: IData
    {
        GameObject ModificatorPrefab { get; }
        Sprite Icon { get; }
        
        Vector3 AdditionalPosition { get; }
    }
}