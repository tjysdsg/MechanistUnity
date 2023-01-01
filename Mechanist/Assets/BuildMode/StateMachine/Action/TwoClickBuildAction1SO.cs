using Block;
using BuildMode;
using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "TwoClickBuildAction1", menuName = "State Machines/Actions/Two Click Build Action1")]
public class TwoClickBuildAction1SO : StateActionSO<TwoClickBuildAction1>
{
    protected override StateAction CreateAction() => new TwoClickBuildAction1();
}

public class TwoClickBuildAction1 : StateAction
{
    protected new TwoClickBuildAction1SO OriginSO => (TwoClickBuildAction1SO)base.OriginSO;
    private BuildModeManager _buildManager;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        _buildManager = stateMachine.GetComponent<BuildModeManager>();
    }

    /// <summary>
    /// The first step in a two click build, the first object is selected
    /// </summary>
    ///
    /// <remarks>
    /// The actual block is created in <see cref="TwoClickBuildAction2"/>
    /// </remarks>
    public override void OnUpdate()
    {
        if (_buildManager.selectionHitInfo == null) return;
        AttachableBlock selectedBlock =
            _buildManager.selectionHitInfo.Value.transform.GetComponent<AttachableBlock>();
        if (selectedBlock != null)
        {
            _buildManager.twoClickBuildFirstBlock = selectedBlock;
            _buildManager.selectionHitInfo = null;
        }
    }
}