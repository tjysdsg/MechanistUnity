using StateMachine;

namespace BuildMode.SM
{
    public class BuildModeBaseCondition : Condition
    {
        protected BuildModeManager _buildManager;

        public override void Awake(StateMachine.StateMachine stateMachine)
        {
            _buildManager = stateMachine.GetComponent<BuildModeManager>();
        }

        protected override bool Statement()
        {
            throw new System.NotImplementedException();
        }
    }
}