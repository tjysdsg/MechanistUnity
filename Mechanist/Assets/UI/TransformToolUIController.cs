using UnityEngine;
using UI;
using UnityEngine.UI;

public class TransformToolUIController : BaseUIController
{
    [SerializeField] private Transform _panel;
    [SerializeField] private Button _positionButton;
    [SerializeField] private Button _rotationButton;

    [SerializeField] private VoidEventChannelSO positionToolButtonEventChannel;
    [SerializeField] private VoidEventChannelSO rotationToolButtonEventChannel;

    protected override void Initialize()
    {
        _positionButton.onClick.AddListener(() => positionToolButtonEventChannel.RaiseEvent());
        _rotationButton.onClick.AddListener(() => rotationToolButtonEventChannel.RaiseEvent());
    }

    private void Update()
    {
        if (!_state.isEditingBall)
        {
            _panel.gameObject.SetActive(false);
            return;
        }

        _panel.gameObject.SetActive(true);
    }
}