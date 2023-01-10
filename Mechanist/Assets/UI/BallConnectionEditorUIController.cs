using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class BallConnectionEditorUIController : BaseUIController
{
    [SerializeField] private Transform _panel;
    [SerializeField] private TMP_Dropdown _connectionTypeSelector;
    [SerializeField] private Button _rotateHingeButton;

    [SerializeField] private BlockConnectionTypeEventChannelSO _blockConnectionTypeEventChannel;
    [SerializeField] private VoidEventChannelSO _rotateHingeConnectionEventChannel;

    private readonly BlockConnectionType[] _blockConnectionTypes =
        (BlockConnectionType[])Enum.GetValues(typeof(BlockConnectionType)).Cast<BlockConnectionType>();

    private Dictionary<BlockConnectionType, int> _blockConnectionTypeToIndex =
        new Dictionary<BlockConnectionType, int>();

    private readonly string[] _blockConnectionTypeNames = Enum.GetNames(typeof(BlockConnectionType));

    protected override void Initialize()
    {
        for (int i = 0; i < _blockConnectionTypes.Length; ++i)
            _blockConnectionTypeToIndex[_blockConnectionTypes[i]] = i;

        var options = new List<TMP_Dropdown.OptionData>();
        foreach (var name in _blockConnectionTypeNames)
        {
            options.Add(new TMP_Dropdown.OptionData(name));
        }

        _connectionTypeSelector.options = options;
        _connectionTypeSelector.value = 0;
        _connectionTypeSelector.onValueChanged.AddListener(OnConnectionTypeChanged);

        _rotateHingeButton.onClick.AddListener(() => _rotateHingeConnectionEventChannel.RaiseEvent());
    }

    private void Update()
    {
        // set this menu visible only if we're actually editing a connection
        if (!_state.blockConnectionEditorUIData.isEditingBallConnection)
        {
            _panel.gameObject.SetActive(false);
            return;
        }

        _panel.gameObject.SetActive(true);

        _connectionTypeSelector.value = _blockConnectionTypeToIndex[_state.blockConnectionEditorUIData.connectionType];

        // show rotate hinge button if the connection being edited is a hinge
        if (_state.blockConnectionEditorUIData.connectionType == BlockConnectionType.Hinge)
            _rotateHingeButton.gameObject.SetActive(true);
        else
            _rotateHingeButton.gameObject.SetActive(false);
    }

    private void OnConnectionTypeChanged(int index)
    {
        var t = _blockConnectionTypes[index];
        _blockConnectionTypeEventChannel.RaiseEvent(t);

        // necessary to avoid assigning previous value to the dropdown in Update()
        _state.blockConnectionEditorUIData.connectionType = t;
    }
}