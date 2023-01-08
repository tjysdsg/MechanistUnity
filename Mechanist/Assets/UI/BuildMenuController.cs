using UnityEngine;
using Core;
using UI;
using UnityEngine.UI;
using TMPro;

public class BuildMenuController : BaseUIController
{
    [SerializeField] private Button _beamButton;
    [SerializeField] private Button _ballButton;
    [SerializeField] private TextMeshProUGUI _currentBlockTypeLabel;
    [SerializeField] private TextMeshProUGUI _currentStateLabel;

    [Tooltip("Event channel used to notify others that user selected a block type to build using the buttons")]
    [SerializeField]
    private BlockTypeEventChannelSO blockTypeSelectionEventChannel;

    protected override void Initialize()
    {
        _beamButton.onClick.AddListener(() => NotifyBlockTypeSelectionChanged(BlockType.Beam));
        _ballButton.onClick.AddListener(() => NotifyBlockTypeSelectionChanged(BlockType.Ball));
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