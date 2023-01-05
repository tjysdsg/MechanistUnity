using Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public abstract class BaseUIController : MonoBehaviour
    {
        protected UIDocument _document;
        protected VisualElement _root;

        private void OnEnable()
        {
            _document = GetComponent<UIDocument>();
            _root = _document.rootVisualElement;

            Initialize();
        }

        protected abstract void Initialize();
    }
}