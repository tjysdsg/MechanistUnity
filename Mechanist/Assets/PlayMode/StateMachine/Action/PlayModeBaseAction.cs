using StateMachine;

namespace PlayMode.SM
{
    public abstract class PlayModeBaseAction : StateAction
    {
        protected PlayModeManager _playManager;

        public override void Awake(StateMachine.StateMachine stateMachine)
        {
            _playManager = stateMachine.GetComponent<PlayModeManager>();
        }
    }
}