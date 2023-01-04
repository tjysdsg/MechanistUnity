using StateMachine;

namespace BuildMode.SM
{
    public abstract class BuildModeBaseCondition : Condition
    {
        protected BuildModeManager _buildManager;

        public override void Awake(StateMachine.StateMachine stateMachine)
        {
            _buildManager = stateMachine.GetComponent<BuildModeManager>();
        }
    }
}