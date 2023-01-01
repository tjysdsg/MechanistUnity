using BuildMode;
using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "TwoClickBuild2Condition",
    menuName = "State Machines/Conditions/Two Click Build2Condition")]
public class TwoClickBuild2ConditionSO : StateConditionSO<TwoClickBuild2Condition>
{
    protected override Condition CreateCondition() => new TwoClickBuild2Condition();
}

public class TwoClickBuild2Condition : Condition
{
    protected new TwoClickBuild2ConditionSO OriginSO => (TwoClickBuild2ConditionSO)base.OriginSO;

    private BuildModeManager _buildManager;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        _buildManager = stateMachine.GetComponent<BuildModeManager>();
    }

    protected override bool Statement()
    {
        return _buildManager.twoClickBuilding && _buildManager.twoClickBuildFirstBlock != null;
    }
}