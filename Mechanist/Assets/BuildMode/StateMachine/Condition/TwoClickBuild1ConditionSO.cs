using BuildMode;
using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "TwoClickBuild1Condition",
    menuName = "State Machines/Conditions/Two Click Build1Condition")]
public class TwoClickBuild1ConditionSO : StateConditionSO<TwoClickBuild1Condition>
{
    protected override Condition CreateCondition() => new TwoClickBuild1Condition();
}

public class TwoClickBuild1Condition : Condition
{
    protected new TwoClickBuild1ConditionSO OriginSO => (TwoClickBuild1ConditionSO)base.OriginSO;

    private BuildModeManager _buildManager;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        _buildManager = stateMachine.GetComponent<BuildModeManager>();
    }

    protected override bool Statement()
    {
        return _buildManager.twoClickBuilding && _buildManager.twoClickBuildFirstBlock == null;
    }
}