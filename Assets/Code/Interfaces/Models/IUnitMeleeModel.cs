using UnityEngine;

namespace Code.Interfaces.Models
{
    public interface IUnitMeleeModel: IUnitModel
    {
        float Cooldown { get; set; }
        float Pitch { get; set; }
        
        AudioSource AudioSource { get; }
    }
}