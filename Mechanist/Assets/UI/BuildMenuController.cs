using UnityEngine;
using UnityEngine.UIElements;
using Core;

public class BuildMenuController : MonoBehaviour
{
    /// <summary>
    /// This channel is two-way.
    /// We want block type changes reflected in the UI.
    /// 
    /// For example, using shortcut will bypass UI, in this case <see cref="BuildModeManager"/> will notify us
    /// using the same channel
    /// </summary>
    [SerializeField] private BlockTypeEventChannelSO blockTypeSelectionEventChannel;

    private Button _braceButton;
    private Button _wieldPointButton;
    private Button _hingeButton;
    private Label _currentBlockTypeLabel;

    private UIDocument _document;
    private VisualElement _root;

    private void OnEnable()
    {
        _document = GetComponent<UIDocument>();
        _root = _document.rootVisualElement;

        _braceButton = _root.Q<Button>("brace-button");
        _wieldPointButton = _root.Q<Button>("wield-point-button");
        _hingeButton = _root.Q<Button>("hinge-button");
        _currentBlockTypeLabel = _root.Q<Label>("current-block-type");

        _braceButton.clicked += () => { NotifyBlockTypeSelectionChanged(BlockType.Brace); };
        _wieldPointButton.clicked += () => { NotifyBlockTypeSelectionChanged(BlockType.WieldPoint); };
        _hingeButton.clicked += () => { NotifyBlockTypeSelectionChanged(BlockType.Hinge); };

        blockTypeSelectionEventChannel.OnEventRaised += ChangeCurrentBlockTypeLabel;
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
}