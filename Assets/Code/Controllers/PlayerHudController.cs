using Code.Controllers.Initialization;
using Code.Data.DataStores;
using Code.Interfaces;
using Code.Services;
using Code.Utils.Extensions;
using Code.Views;
using DG.Tweening;
using RSG;
using TMPro;
using UnityEngine;

namespace Code.Controllers
{
    internal sealed class PlayerHudController: IController, IInitialization, ICleanup
    {
        private readonly UIInitialization _uiInitialization;
        
        private readonly MessageBrokerService<string> _messageBrokerService;
        private readonly IPromiseTimer _promiseTimer;
        private readonly UIStore _uiStore;

        private HudView _hud;

        private const float MESSAGE_ANIMATION_TIME = 0.5f;
        private const float MESSAGE_ALIVE_TIME = 5f;
        private const float MESSAGE_MOVE_X = 500f;

        private int _health = -1;
        private int _armor = -1;
        private int _ammo = -1;
        private int _maxAmmo = -1;
        private int _score = -1;

        public PlayerHudController(UIInitialization uiInitialization, UIStore uiStore, MessageBrokerService<string> messageBrokerService, IPromiseTimer promiseTimer)
        {
            _uiInitialization = uiInitialization;
            
            _messageBrokerService = messageBrokerService;
            _promiseTimer = promiseTimer;
            _uiStore = uiStore;
        }

        public void Initialization()
        {
            _hud = _uiInitialization.GetHud();
            _messageBrokerService.OnPublish += NewNotifyMessage;
            
            SetMaxAmmo(0);
            SetAmmo(0);
            SetScore(0);
        }
        
        public void Cleanup()
        {
            _messageBrokerService.OnPublish -= NewNotifyMessage;
        }

        private void NewNotifyMessage(object source, string messageText)
        {
            var message = Object.Instantiate(_uiStore.NotifyMessagePrefabPath, _hud.NotifyContent);
            var transform = message.transform;

            message.GetComponentInChildren<TMP_Text>().text = messageText;

            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, MESSAGE_ANIMATION_TIME);
            _promiseTimer.WaitFor(MESSAGE_ALIVE_TIME)
                .Then(() => transform.DOScale(Vector3.zero, MESSAGE_ANIMATION_TIME).OnComplete(() => Object.Destroy(message)));
        }
        
        public void SetScore(int score)
        {
            if (_score == score)
                return;
            
            _score = score;
            _hud.ScoreText.text = FormatNumbers.Cut(_score);
        }
        public void AddScore(int score)
        {
            _score += score;
            _hud.ScoreText.text = FormatNumbers.Cut(_score);
        }
    
        public void SetMaxAmmo(int maxAmmo)
        {
            if (_maxAmmo == maxAmmo) 
                return;
        
            _hud.AmmoText.text = $"Ammo: {_ammo}/{maxAmmo}";
            _maxAmmo = maxAmmo;
        }
    
        public void SetHealth(int health)
        {
            if (_health == health) 
                return;
        
            _hud.HealthText.text = $"Health: {health}";
            _health = health;
        }
    
        public void SetArmor(int armor)
        {
            if (armor == _armor)
                return;

            _hud.ArmorText.text = $"Armor: {armor}";
            _armor = armor;
        }
    
        public void SetAmmo(int ammo)
        {
            if (_ammo == ammo)
                return;

            _hud.AmmoText.text = $"Ammo: {ammo}/{_maxAmmo}";
            _ammo = ammo;
        }
    }
}