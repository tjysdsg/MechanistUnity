using System;
using System.Collections.Generic;
using Core;
using UI;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

public class BallConnectionEditorUIController : BaseUIController
{
    [SerializeField] private BlockConnectionTypeEventChannelSO _blockConnectionTypeEventChannel;

    private DropdownField _connectionTypeSelector;

    protected override void Initialize()
    {
        _connectionTypeSelector = _root.Q<DropdownField>("connection-type-selector");
        _connectionTypeSelector.choices = new List<string>(Enum.GetNames(typeof(BlockConnectionType)));
        _connectionTypeSelector.index = 0;

        _connectionTypeSelector.RegisterValueChangedCallback(OnConnectionTypeChanged);
    }

    private void Update()
    {
        if (!_uiState.isEditingBallConnection)
        {
            _root.visible = false;
            return;
        }

        _root.visible = true;
    }

    private void OnConnectionTypeChanged(ChangeEvent<string> e)
    {
        BlockConnectionType t = BlockConnectionType.Fixed;
        Assert.IsTrue(
            BlockConnectionType.TryParse(e.newValue, out t)
        );
        _blockConnectionTypeEventChannel.RaiseEvent(t);
    }
}