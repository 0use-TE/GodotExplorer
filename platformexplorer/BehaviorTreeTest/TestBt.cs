using Godot;
using Godot.DependencyInjection.Attributes;
using GodotBehaviorTree;
using GodotBehaviorTree.Core;
using GodotStateMachine;
using Microsoft.Extensions.Logging;
namespace PlatformExplorer.BehaviorTreeTest;

public partial class TestBt : CharacterBody2D
{
	[Inject]
	private ILogger<TestBt> _logger = default!;

	private NavigationAgent2D _navigationAgent2D;

	private StateMachine _enemyStateMachine;
	private EnemyIdle _enemyIdle;
	private EnemyFollow _enemyFollow;

	// 
	private bool _isIdle;
	private bool _isFollow;

	//Tree
	private BehaviorTree _tree;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_navigationAgent2D = GetNode<NavigationAgent2D>(nameof(NavigationAgent2D));
		//StateMachine
		_enemyStateMachine = new StateMachine();
		_enemyIdle = new EnemyIdle(_enemyStateMachine);
		_enemyFollow = new EnemyFollow(_enemyStateMachine);

		_enemyIdle.AddEnter(() => _isIdle = true)
			.AddPhysicsProcess((delta) =>
			{
				GD.Print("Idle");
			})
			.AddExit(() => _isIdle = false);

		_enemyFollow.AddEnter(() => _isFollow = true)
			.AddPhysicsProcess((delta =>
			{
				GD.Print("Follow");
			}))
			.AddExit(() => _isFollow = false);

		_enemyStateMachine.SetInitialState(_enemyIdle);

		_logger.LogInformation("Enemy stateMachine is init!");

		_tree= BehaviorTree.CreateTree()
			.ConfigurateStateMachine(_enemyStateMachine);

		_tree.BuildTree()
			.Selector()
				.Sequence()
					.Condition((delta) => true)
					//跟随玩家
					.SwitchState(_enemyFollow)
			.End()
					.SwitchState(_enemyIdle);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public override void _PhysicsProcess(double delta)
	{
		//BT tick
		_tree.Tick(delta);
		//StateMachine's process.
		_enemyStateMachine.PhysicsProcess(delta);
	}
}
