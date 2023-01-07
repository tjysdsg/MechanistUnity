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

    [Tooltip("Event channel used to notify others that user selected a block type to build using the buttons")]
    [SerializeField]
    private BlockTypeEventChannelSO blockTypeSelectionEventChannel;

    #endregion

    #region Status

    private Label _currentBlockTypeLabel;
    private Label _currentStateLabel;

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

        _positionToolButton = _root.Q("transform-handle-panel").Q<Button>("position");
        _rotationToolButton = _root.Q("transform-handle-panel").Q<Button>("rotation");
        _positionToolButton.clicked += () => { positionToolButtonEventChannel.RaiseEvent(); };
        _rotationToolButton.clicked += () => { rotationToolButtonEventChannel.RaiseEvent(); };
    }

    private void Update()
    {
        _currentBlockTypeLabel.text = _uiState.currentBlockType.ToString();
        _currentStateLabel.text = _uiState.currentBuildState;
    }

    private void NotifyBlockTypeSelectionChanged(BlockType type)
    {
        blockTypeSelectionEventChannel.RaiseEvent(type);
    }
}