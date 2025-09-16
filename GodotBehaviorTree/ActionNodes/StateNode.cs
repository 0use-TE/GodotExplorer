using Godot;
using Godot.DependencyInjection.Attributes;
using GodotBehaviorTree.Core;
using GodotStateMachine;
using Microsoft.Extensions.Logging;
using System;

namespace GodotBehaviorTree.ActionNodes
{
    public class StateNode : BehaviourNode
    {
        //StateMachine
        private StateMachine? _stateMachine;
        private BaseState _state;
        public StateNode( BaseState baseState)
        {
            _state = baseState;
        }
        public override NodeState Tick(double delta)
        {
            SwitchState();
            return NodeState.Running;
        }
        public override void SetParent(ICompositeNode parent)
        {
            base.SetParent(parent);
            _stateMachine = Blackboard.Load<StateMachine>();
        }
        private void SwitchState()
        {
            var currentState = _stateMachine?.GetCurrentState();
            if (currentState != _state)
            {
                _stateMachine?.ChangeState(_state);
            }
            else
            {

            }
        }
    }
}
