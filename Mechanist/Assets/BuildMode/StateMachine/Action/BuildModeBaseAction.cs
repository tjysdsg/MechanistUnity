using StateMachine;

namespace BuildMode.SM
{
    public class BuildModeBaseAction : StateAction
    {
        protected BuildModeManager _buildManager;

        public override void Awake(StateMachine.StateMachine stateMachine)
        {
            _buildManager = stateMachine.GetComponent<BuildModeManager>();
        }

        public override void OnUpdate()
        {
            throw new System.NotImplementedException();
        }
    }
}