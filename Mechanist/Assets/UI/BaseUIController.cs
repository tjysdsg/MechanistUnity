using Core;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace UI
{
    public abstract class BaseUIController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected UIStateSO _uiState = null;

        private void OnEnable()
        {
            Assert.IsNotNull(_uiState);
            Initialize();
        }

        protected abstract void Initialize();

        public void OnPointerEnter(PointerEventData eventData)
        {
            _uiState.isMouseOverUIElements = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _uiState.isMouseOverUIElements = false;
        }
    }
}