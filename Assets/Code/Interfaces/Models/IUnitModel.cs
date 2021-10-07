using UnityEngine;

namespace Code.Interfaces.Models
{
    public interface IUnitModel
    {
        public float Health { get; set; }
        public float Armor { get; set; }
        
        public Transform Transform { get; set; }
        public GameObject GameObject { get; set; }
    }
}