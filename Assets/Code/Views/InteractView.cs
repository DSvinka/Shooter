using System;
using UnityEngine;

namespace Code.Views
{
    internal sealed class InteractView: MonoBehaviour
    {
        [SerializeField] private GameObject _interactObject;
        
        public event Action<GameObject, int, int> OnInteract = delegate(GameObject interactObject, int viewID, int unitID) {  };

        public void Interact(int unitID)
        {
            OnInteract.Invoke(_interactObject, gameObject.GetInstanceID(), unitID);
        }
    }
}