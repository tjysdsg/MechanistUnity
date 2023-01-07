using UnityEngine;
using UnityEngine.UIElements;
using Core;
using UI;

public class BuildMenuController : BaseUIController
{
    #region Block type selection

    private Button _braceButton;
    private Button _wieldPointButton;
    private Button _hingeButton;

    /// <summary>
    /// This channel is two-way.
    /// We want block type changes reflected in the UI.
    /// 
    /// For example, using shortcut will bypass UI, in this case <see cref="BuildModeManager"/> will notify us
    /// using the same channel
    /// </summary>
    [SerializeField] private BlockTypeEventChannelSO blockTypeSelectionEventChannel;

    #endregion

    #region Status

    private Label _currentBlockTypeLabel;
    private Label _currentStateLabel;
    [SerializeField] private StringEventChannelSO buildStateEventChannel;

    #endregion

    #region TransformHandle

    private Button _positionToolButton;
    private Button _rotationToolButton;
    [SerializeField] private VoidEventChannelSO positionToolButtonEventChannel;
    [SerializeField] private VoidEventChannelSO rotationToolButtonEventChannel;

    #endregion

    protected override void Initialize()
    {
        _braceButton = _root.Q<Button>("beam-button");
        _wieldPointButton = _root.Q<Button>("ball-button");
        _currentBlockTypeLabel = _root.Q<Label>("current-block-type");
        _currentStateLabel = _root.Q<Label>("current-state");

        _braceButton.clicked += () => { NotifyBlockTypeSelectionChanged(BlockType.Beam); };
        _wieldPointButton.clicked += () => { NotifyBlockTypeSelectionChanged(BlockType.Ball); };

        blockTypeSelectionEventChannel.OnEventRaised += ChangeCurrentBlockTypeLabel;
        buildStateEventChannel.OnEventRaised += ChangeBuildState;

        _positionToolButton = _root.Q("transform-handle-panel").Q<Button>("position");
        _rotationToolButton = _root.Q("transform-handle-panel").Q<Button>("rotation");
        _positionToolButton.clicked += () => { positionToolButtonEventChannel.RaiseEvent(); };
        _rotationToolButton.clicked += () => { rotationToolButtonEventChannel.RaiseEvent(); };
    }

    private void NotifyBlockTypeSelectionChanged(BlockType type)
    {
        blockTypeSelectionEventChannel.RaiseEvent(type);
        ChangeCurrentBlockTypeLabel(type);
    }

    /// <summary>
    /// This function is only called when some other objects notified us, so DO NOT notify them back!
    /// </summary>
    private void ChangeCurrentBlockTypeLabel(BlockType type)
    {
        _currentBlockTypeLabel.text = type.ToString();
    }

    private void ChangeBuildState(string name)
    {
        _currentStateLabel.text = name;
    }
}