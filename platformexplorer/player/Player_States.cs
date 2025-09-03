using GodotStateMachine;

namespace PlatformExplorer.player
{
    public partial class Player
    {
        internal class PlayerIdleState : BaseState
        {
            public PlayerIdleState(StateMachine sm) : base(sm) { }
        }

        internal class PlayerWalkState : BaseState
        {
            public PlayerWalkState(StateMachine sm) : base(sm) { }
        }
        internal class PlayerAttackState : BaseState
        {
            public PlayerAttackState(StateMachine sm) : base(sm) { }
        }
    }
}

