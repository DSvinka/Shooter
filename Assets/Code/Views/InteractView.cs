using System;
using Code.Managers;
using UnityEngine;

namespace Code.Views
{
    internal sealed class InteractView: MonoBehaviour
    {
        [SerializeField] private GameObject _interactObject;
        [SerializeField] private InteractManager.InteractType _interactType;

        public GameObject Item => _interactObject;
        public InteractManager.InteractType InteractType => _interactType;
        
        public event Action<GameObject, int, int> OnInteract = delegate(GameObject interactObject, int viewID, int unitID) {  };

        public void Interact(int unitID)
        {
            OnInteract.Invoke(_interactObject, gameObject.GetInstanceID(), unitID);
        }
    }
}