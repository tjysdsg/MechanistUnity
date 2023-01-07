using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UIElements;

public class BallConnectionEditorUIController : BaseUIController
{
    private DropdownField _connectionTypeSelector;

    protected override void Initialize()
    {
        _connectionTypeSelector = _root.Q<DropdownField>("connection-type-selector");
        _connectionTypeSelector.choices = new List<string> { "Fixed", "Hinge" };
        _connectionTypeSelector.index = 0;
    }
}