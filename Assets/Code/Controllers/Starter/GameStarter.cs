using Code.Controllers.Initialization;
using Code.Data.DataStores;
using UnityEngine;

namespace Code.Controllers.Starter
{
    internal sealed class GameStarter : MonoBehaviour
    {
        [SerializeField] private DataStore _data;
        private Controllers _controllers;

        public DataStore Data
        {
            get => _data;
            set => _data = value;
        }

        private void Start()
        {
            _controllers = new Controllers();
            
            var game = new GameInitialization(_controllers, _data);
            
            _controllers.Initialization();
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            _controllers.Execute(deltaTime);
        }

        private void LateUpdate()
        {
            var deltaTime = Time.deltaTime;
            _controllers.LateExecute(deltaTime);
        }

        private void OnDestroy()
        {
            _controllers.Cleanup();
        }
    }
}
