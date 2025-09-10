using GroveGames.BehaviourTree.Nodes;
using System;

namespace PlatformExplorer.BehaviorTreeTest;

public class SequenceCondition : Node
{
    private readonly Func<bool> _condition;
    public SequenceCondition(IParent parent, Func<bool>? condition=null) : base(parent)
    {
        if (condition == null)
            condition = () => true;
		_condition = condition;
    }
    public override NodeState Evaluate(float deltaTime)
    {
        return _condition() ? NodeState.Success : NodeState.Failure;
    }
}
