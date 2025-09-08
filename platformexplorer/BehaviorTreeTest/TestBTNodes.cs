using Godot;
using GroveGames.BehaviourTree.Nodes;
using GroveGames.BehaviourTree.Nodes.Composites;
using System;
using static PlatformExplorer.BehaviorTreeTest.TestBt;
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

		public override NodeState Evaluate(float delta)
        {
          var blackboard=  Blackboard.GetValue<TestBtBlackboard>(nameof(TestBtBlackboard));
            if (blackboard?.player != null)
            {
                GD.Print("Player detected");
                return NodeState.Success;
            }
            GD.Print("No player detected");
            return NodeState.Failure;
		}
	}
	// 行走节点
	public class Test_Walk : BTNode
	{
		private NavigationAgent2D _navigationAgent2D;
		private CharacterBody2D _characterBody2D;
		public Test_Walk(IParent parent, NavigationAgent2D navigationAgent2D, CharacterBody2D characterBody2D) : base(parent)
		{
			_navigationAgent2D = navigationAgent2D;
			_characterBody2D = characterBody2D;
		}

		public override NodeState Evaluate(float deltaTime)
		{
			var blackboard = Blackboard.GetValue<TestBtBlackboard>(nameof(TestBtBlackboard));
			var player = blackboard?.player.GetNode<CharacterBody2D>(nameof(CharacterBody2D));

			// 检查玩家节点是否有效
			if (player == null)
			{
				GD.PrintErr("玩家节点未找到！");
				_characterBody2D.Velocity = Vector2.Zero; // 停止移动
				return NodeState.Failure;
			}
			
			// 更新目标位置为玩家的实时位置
			_navigationAgent2D.TargetPosition = player.GlobalPosition;

			// 获取下一个路径点
			Vector2 nextPos = _navigationAgent2D.GetNextPathPosition();
			Vector2 currentPos = _characterBody2D.GlobalPosition;

			// 计算到下一个路径点的距离和方向
			float distanceToNext = (nextPos - currentPos).Length();
			Vector2 dir = distanceToNext > 0 ? (nextPos - currentPos).Normalized() : Vector2.Zero;

			// 设置速度，防止抖动
			if (distanceToNext < 5.0f)
				_characterBody2D.Velocity = Vector2.Zero; // 距离很近时停止，防止抖动
			else
				_characterBody2D.Velocity = dir * 16; // 正常移动速度

			// 调试信息
			GD.Print($"玩家位置: {player.GlobalPosition}, 目标: {_navigationAgent2D.TargetPosition}, 下一路径点: {nextPos}, 距离: {distanceToNext}, 导航完成: {_navigationAgent2D.IsNavigationFinished()}");

			// 执行移动
			_characterBody2D.MoveAndSlide();

			// 持续跟踪，返回 Running
			return NodeState.Running;
		}
	}
		public partial class Test_Tree : GroveGames.BehaviourTree.GodotBehaviourTree
    {
        //        Root
        //└── Selector
        //     ├── Sequence(攻击分支)
        //     │     ├── Condition(HasPlayer)       // 必须发现玩家
        //     │     ├── Condition(IsInAttackRange) // 必须在攻击范围
        //     │     ├── Cooldown(AttackCD)         // 冷却时间限制
        //     │     └── Attack                     // 攻击动作
        //     │
        //     ├── Sequence(追击分支)
        //     │     ├── Condition(HasPlayer)       // 必须发现玩家
        //     │     ├── Inverter(IsTooClose)       // 不要太近
        //     │     └── WalkToPlayer               // 追击动作
        //     │
        //     └── Idle                             // 没有玩家，闲置
        private NavigationAgent2D _navigationAgent2D;
        private CharacterBody2D _characterBody2D;
        public Test_Tree(NavigationAgent2D navigationAgent2D,CharacterBody2D characterBody2D)
        {
            _navigationAgent2D = navigationAgent2D;
			_characterBody2D = characterBody2D;
		}
		public override void SetupTree()
        {
            // 根节点下挂 Selector
            var selector = Root.Selector();

            //追击分支
            
            var followSequence = selector.Sequence();
            followSequence.Attach(new Test_HasPlayer(followSequence)); //必须发现玩家
            followSequence.Attach(new Test_Walk(followSequence,_navigationAgent2D,_characterBody2D)); //追击动作

			//闲置
			selector.Attach(new Test_Idle(selector));

        }
    }
}