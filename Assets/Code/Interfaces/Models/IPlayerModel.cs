using Code.Data;
using Code.Models;
using Code.Views;
using UnityEngine;

namespace Code.Interfaces.Models
{
    internal interface IPlayerModel: IUnitModel
    {
        Camera Camera { get; }
        PlayerView View { get; }
        PlayerData Data { get; }
        WeaponModel Weapon { get; set; }
    }
}