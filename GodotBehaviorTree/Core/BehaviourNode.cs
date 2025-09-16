using GodotBehaviorTree.Blackboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodotBehaviorTree.Core
{
    public abstract class BehaviourNode:INode
    {
        protected ICompositeNode? _parent;

        public IBlackboard Blackboard { get; set; } = new DefaultBlackboard();

        public  virtual  void SetParent(ICompositeNode parent)
        {
            this._parent = parent;
            Blackboard= parent.Blackboard;
        }

        public abstract NodeState Tick(double delta);
    }
}
