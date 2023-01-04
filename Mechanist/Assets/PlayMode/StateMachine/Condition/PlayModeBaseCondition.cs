using StateMachine;

namespace PlayMode.SM
{
    public abstract class PlayModeBaseCondition : Condition
    {
        protected PlayModeManager _playManager;

        public override void Awake(StateMachine.StateMachine stateMachine)
        {
            _playManager = stateMachine.GetComponent<PlayModeManager>();
        }
    }
}