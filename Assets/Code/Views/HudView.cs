using TMPro;
using UnityEngine;

namespace Code.Views
{
    internal sealed class HudView : MonoBehaviour
    {
        [Header("Информация о игроке")]
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private TMP_Text _armorText;
        [SerializeField] private TMP_Text _ammoText;
        
        [Header("Модификация оружия")]
        [SerializeField] private GameObject _modificatorMenu;
        [SerializeField] private Transform _aimContent;
        [SerializeField] private Transform _barrelContent;
        
        [Header("Количество Очков")]
        [SerializeField] private GameObject _scoreBoard;
        [SerializeField] private TMP_Text _scoreText;

        [Header("Уведомления - Сообщения")] 
        [SerializeField] private Transform _notifyContent;

        public Transform AimContent => _aimContent;
        public Transform BarrelContent => _barrelContent;
        public GameObject ModificatorMenu => _modificatorMenu;
        
        public TMP_Text HealthText => _healthText;
        public TMP_Text ArmorText => _armorText;
        public TMP_Text AmmoText => _ammoText;
        
        public GameObject ScoreBoard => _scoreBoard;
        public TMP_Text ScoreText => _scoreText;
        
        public Transform NotifyContent => _notifyContent;
    }
}