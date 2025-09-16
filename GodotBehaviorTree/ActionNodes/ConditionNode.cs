using GodotBehaviorTree.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodotBehaviorTree.ActionNodes
{
    class ConditionNode : BehaviourNode
    {
        private Func<double, bool> _condition;
        public ConditionNode(Func<double, bool> condition)
        {
            this._condition = condition;
        }

        public override NodeState Tick(double delta)
        {
            return _condition(delta) ? NodeState.Success : NodeState.Failure;
        }
    }
}
