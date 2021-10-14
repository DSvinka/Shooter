using TMPro;
using UnityEngine;

namespace Code.Views
{
    internal sealed class PlayerHudView : MonoBehaviour
    {
        [Header("Информация о игроке")]
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private TMP_Text _armorText;
        [SerializeField] private TMP_Text _ammoText;
        
        [Header("Модификация оружия")]
        [SerializeField] private GameObject _modificatorMenu;
        [SerializeField] private Transform _aimContent;
        [SerializeField] private Transform _barrelContent;

        public Transform AimContent => _aimContent;
        public Transform BarrelContent => _barrelContent;
        public GameObject ModificatorMenu => _modificatorMenu;
        
        public TMP_Text HealthText => _healthText;
        public TMP_Text ArmorText => _armorText;
        public TMP_Text AmmoText => _ammoText;
    }
}