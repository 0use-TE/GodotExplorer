namespace GodotStateMachine
{
    public static class StateExtensions
    {
        public static void AddTransitions(
            this BaseState state,
            params (Func<bool> condition, BaseState nextState)[] transitions)
        {
            foreach (var (condition, nextState) in transitions)
            {
                state.Transitions.Add(condition, nextState);
            }
        }
    }
}
