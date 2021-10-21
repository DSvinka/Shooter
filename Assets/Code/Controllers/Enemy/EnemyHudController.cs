using System.Collections.Generic;
using Code.Controllers.Initialization;
using Code.Data.DataStores;
using Code.Interfaces;
using Code.Listeners;
using Code.Models;
using Code.Services;
using DG.Tweening;
using RSG;
using TMPro;
using UnityEngine;

namespace Code.Controllers.Enemy
{
    internal sealed class EnemyHudController: IController, IExecute, IInitialization, ICleanup
    {
        private readonly PlayerInitialization _playerInitialization;
        private readonly IPromiseTimer _promiseTimer;
        private readonly UnitListener _unitListener;
        private readonly UIStore _uiStore;
        private readonly PoolService _poolService;

        private List<GameObject> _messages;
        private PlayerModel _player;
        
        private const float HIT_MESSAGE_LIFETIME = 1f;
        
        public EnemyHudController(UIStore uiStore, PlayerInitialization playerInitialization, UnitListener unitListener, PoolService poolService, IPromiseTimer promiseTimer)
        {
            _uiStore = uiStore;
            _poolService = poolService;
            _promiseTimer = promiseTimer;
            _unitListener = unitListener;
            _playerInitialization = playerInitialization;
        }
        
        public void Initialization()
        {
            _messages = new List<GameObject>();
            _player = _playerInitialization.GetPlayer();
            
            _unitListener.OnUnitDamage += OnUnitDamage;
        }
        
        public void Cleanup()
        {
            _unitListener.OnUnitDamage -= OnUnitDamage;
        }
        
        public void Execute(float deltaTime)
        {
            if (_messages.Count != 0)
            {
                for (var index = 0; index < _messages.Count; index++)
                {
                    var message = _messages[index];
                    message.transform.LookAt(_player.Transform);
                }
            }
        }

        private void OnUnitDamage(GameObject attacker, Vector3 damagePosition, int unitID, float damage)
        {
            var message = CreateHitDisplay(damagePosition, damage);
            
            _messages.Add(message);
            _promiseTimer.WaitFor(HIT_MESSAGE_LIFETIME)
                .Then(() => RemoveMessage(message));
        }

        private GameObject CreateHitDisplay(Vector3 position, float damage)
        {
            var hitMessage = _poolService.Instantiate(_uiStore.HitMessagePrefab);
            hitMessage.transform.position = position;
            hitMessage.transform.DOScale(Vector3.zero, HIT_MESSAGE_LIFETIME);
            var text = hitMessage.GetComponentInChildren<TMP_Text>();
            text.text = $"-{damage}";

            return hitMessage;
        }

        private void RemoveMessage(GameObject message)
        {
            _poolService.Destroy(message);
            _messages.Remove(message);
        }
    }
}