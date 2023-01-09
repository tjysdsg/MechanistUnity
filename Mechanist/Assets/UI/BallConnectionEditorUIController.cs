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

    private readonly string[] _blockConnectionTypeNames = Enum.GetNames(typeof(BlockConnectionType));

    protected override void Initialize()
    {
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
        if (!_uiState.blockConnectionEditorUIData.isEditingBallConnection)
        {
            _panel.gameObject.SetActive(false);
            return;
        }

        _panel.gameObject.SetActive(true);

        // show rotate hinge button if the connection being edited is a hinge
        if (_uiState.blockConnectionEditorUIData.connectionType == BlockConnectionType.Hinge)
            _rotateHingeButton.gameObject.SetActive(true);
        else
            _rotateHingeButton.gameObject.SetActive(false);
    }

    private void OnConnectionTypeChanged(int index)
    {
        _blockConnectionTypeEventChannel.RaiseEvent(_blockConnectionTypes[index]);
    }
}