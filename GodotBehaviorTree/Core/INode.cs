using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodotBehaviorTree.Core
{

    public interface INode
    {
        void SetParent(ICompositeNode parent);
        NodeState Tick(double delta);
        IBlackboard Blackboard { get; set; }
    }
}
