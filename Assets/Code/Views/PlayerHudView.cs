using TMPro;
using UnityEngine;

namespace Code.Views
{
    internal sealed class PlayerHudView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private TMP_Text _armorText;
        [SerializeField] private TMP_Text _ammoText;
        
        public TMP_Text HealthText => _healthText;
        public TMP_Text ArmorText => _armorText;
        public TMP_Text AmmoText => _ammoText;
    }
}