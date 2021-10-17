using Code.Interfaces.Data;
using static Code.Utils.Extensions.DataUtils;
using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = "Player", menuName = "Data/Units/Player")]
    public sealed class PlayerData : ScriptableObject, IData, IUnitData
    {
        public string Path { get; set; }
        
        #region Пути
        
        [Header("Префабы")]
        [SerializeField] [AssetPath.Attribute(typeof(GameObject))] private string _playerPrefabPath;
        
        #endregion
        
        #region Поля

        [Header("Информация")] 
        [SerializeField] private string _name = "Игрок";
        
        [Header("Характеристики")] 
        [SerializeField] private float _maxHealth = 100f;
        [SerializeField] private float _maxArmor = 200f;
        
        [Header("Передвижение")] 
        [SerializeField] private float _walkingSpeed = 7.5f;
        [SerializeField] private float _runningSpeed = 11.5f;
        [SerializeField] private float _jumpForce = 3.0f;
        [SerializeField] private float _lookSpeed = 2.0f;
        [SerializeField] private float _lookXLimit = 45.0f;
        
        [Header("Взаимодействие")] 
        [SerializeField] private float _maxInteractDistance = 15f;
        [SerializeField] private float _dropItemForce = 200f;
        
        #endregion
        
        #region Объекты
        
        private GameObject _playerPrefab;

        #endregion
        
        #region Публичные Свойства
        
        public GameObject PlayerPrefab => GetData(_playerPrefabPath, _playerPrefab);

        public string Name => _name;

        public float MaxHealth => _maxHealth;
        public float MaxArmor => _maxArmor;

        public float WalkingSpeed => _walkingSpeed;
        public float RunningSpeed => _runningSpeed;
        public float JumpForce => _jumpForce;
        public float LookSpeed => _lookSpeed;
        public float LookXLimit => _lookXLimit;
        
        public float MaxInteractDistance => _maxInteractDistance;
        public float DropItemForce => _dropItemForce;

        #endregion
    }
}