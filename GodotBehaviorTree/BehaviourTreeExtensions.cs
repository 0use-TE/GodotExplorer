﻿using GodotBehaviorTree.ActionNodes;
using GodotBehaviorTree.CompositeNodes;
using GodotBehaviorTree.Core;
using GodotStateMachine;
using System;

namespace GodotBehaviorTree
{
    public static class BehaviourTreeExtensions
    {
        public static ICompositeNode Selector(this ICompositeNode parent)
        {
            var selector = new Selector();
            parent.AddChild(selector);
            return selector;
        }
        public static ICompositeNode Sequence(this ICompositeNode parent)
        {
            var sequence = new Sequence();
            parent.AddChild(sequence);
            return sequence;
        }
        public static ICompositeNode Parallel(this ICompositeNode parent, ParallelPolicy successPolicy, ParallelPolicy failurePolicy)
        {
            var parallel = new GodotBehaviorTree.CompositeNodes.Parallel(successPolicy, failurePolicy);
            parent.AddChild(parallel);
            return parallel;
        }
        public static ICompositeNode Action(this ICompositeNode parent, Func<double, NodeState> action)
        {
            var actionNode = new ActionNode(action);
            parent.AddChild(actionNode);
            return parent;
        }

        public static ICompositeNode Condition(this ICompositeNode parent, Func<double, bool> condition)
        {
            var conditionNode = new ConditionNode(condition);
            parent.AddChild(conditionNode);
            return parent;
        }
        public static ICompositeNode SwitchState(this ICompositeNode parent, BaseState state)
        {
            var switchNode = new StateNode(state);
            parent.AddChild(switchNode);
            return parent;
        }

        public static ICompositeNode End(this ICompositeNode parent)
        {
            return parent.GetParent() ?? throw new NullReferenceException("This node has not parent!");
        }
    }
}
