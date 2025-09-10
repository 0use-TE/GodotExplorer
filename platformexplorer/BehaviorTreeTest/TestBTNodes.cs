using Godot;
using GroveGames.BehaviourTree.Nodes;
using GroveGames.BehaviourTree.Nodes.Composites;
using System;
using BTNode = GroveGames.BehaviourTree.Nodes.Node; // 别名解决冲突
using IParent = GroveGames.BehaviourTree.Nodes.IParent; // 别名简化 IParent

namespace PlatformExplorer.BehaviorTreeTest
{
    // 空闲节点
    public class Test_Idle : BTNode
    {
        public Test_Idle(IParent parent) : base(parent) { }

        public override NodeState Evaluate(float delta)
        {
            GD.Print("Enemy is idle");
            return NodeState.Running;
        }
    }
    public class Test_HasPlayer : SequenceCondition
    {
        public Test_HasPlayer(IParent parent, Func<bool>? condition = null) : base(parent, condition)
        {
        }

        public override NodeState Evaluate(float deltaTime)
        {
            return base.Evaluate(deltaTime);
        }
    }
    // 行走节点
    public class Test_Walk : BTNode
    {
        public Test_Walk(IParent parent) : base(parent)
        {
        }

        public override NodeState Evaluate(float deltaTime)
        {
            return base.Evaluate(deltaTime);
        }
    }
    public partial class Test_Tree : GroveGames.BehaviourTree.GodotBehaviourTree
    {
        public override void SetupTree()
        {
            // 根节点下挂 Selector
            var selector = Root.Selector();

            //追击分支
            var followSequence = selector.Sequence();
            followSequence
                .Attach(new Test_HasPlayer(followSequence))//发现玩家
                .Attach(new Test_Walk(followSequence));//追击动作
            //闲置
            selector.Attach(new Test_Idle(selector));

        }
    }
}