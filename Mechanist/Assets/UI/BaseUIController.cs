using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public abstract class BaseUIController : MonoBehaviour
    {
        [SerializeField] protected UIStateSO _uiState;
        [SerializeField] private string _panelClassName = "panel";

        protected UIDocument _document;
        protected VisualElement _root;

        private void OnEnable()
        {
            _document = GetComponent<UIDocument>();
            _root = _document.rootVisualElement;

            // register callbacks to remember if mouse is currently over an UI element (.panel class)
            List<VisualElement> list = _root.Query(className: _panelClassName).ToList();
            foreach (var v in list)
            {
                v.RegisterCallback<MouseEnterEvent>(MouseEnterCallback);
                v.RegisterCallback<MouseLeaveEvent>(MouseLeaveCallback);
            }

            Initialize();
        }

        private void MouseEnterCallback(MouseEnterEvent evt)
        {
            // check the target because this event trickles down
            var v = (VisualElement)evt.target;
            // TODO: don't assume the mouse always leaves a panel before entering another?
            if (v.ClassListContains(_panelClassName))
                _uiState.isMouseOverUIElements = true;
        }

        private void MouseLeaveCallback(MouseLeaveEvent evt)
        {
            // check the target because this event trickles down
            var v = (VisualElement)evt.target;
            if (v.ClassListContains(_panelClassName))
                _uiState.isMouseOverUIElements = false;
        }

        protected abstract void Initialize();
    }
}