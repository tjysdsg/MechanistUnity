using Core;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace UI
{
    public abstract class BaseUIController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected UIStateSO _state = null;

        private void OnEnable()
        {
            Assert.IsNotNull(_state);
            Initialize();
        }

        protected abstract void Initialize();

        public void OnPointerEnter(PointerEventData eventData)
        {
            _state.isMouseOverUIElements = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _state.isMouseOverUIElements = false;
        }
    }
}