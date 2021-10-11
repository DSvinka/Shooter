using Code.Data;
using Code.Models;
using Code.Views;
using UnityEngine;

namespace Code.Interfaces.Models
{
    internal interface IPlayerModel: IUnitModel
    {
        InteractView ObjectInHand { get; }
        
        Camera Camera { get; }

        PlayerView View { get; }
        PlayerData Data { get; }
        
        WeaponModel Weapon { get; set; }

        void SetObjectInHand(InteractView item);
    }
}