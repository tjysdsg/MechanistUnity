using StateMachine;

namespace BuildMode.SM
{
    public abstract class BuildModeBaseAction : StateAction
    {
        protected BuildModeManager _buildManager;

        public override void Awake(StateMachine.StateMachine stateMachine)
        {
            _buildManager = stateMachine.GetComponent<BuildModeManager>();
        }
    }
}