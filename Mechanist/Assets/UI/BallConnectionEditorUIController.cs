using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using TMPro;
using UI;
using UnityEngine;

public class BallConnectionEditorUIController : BaseUIController
{
    [SerializeField] private Transform _panel;
    [SerializeField] private TMP_Dropdown _connectionTypeSelector;
    [SerializeField] private BlockConnectionTypeEventChannelSO _blockConnectionTypeEventChannel;

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
    }

    private void Update()
    {
        if (!_uiState.isEditingBallConnection)
        {
            _panel.gameObject.SetActive(false);
            return;
        }

        _panel.gameObject.SetActive(true);
    }

    private void OnConnectionTypeChanged(int index)
    {
        _blockConnectionTypeEventChannel.RaiseEvent(_blockConnectionTypes[index]);
    }
}