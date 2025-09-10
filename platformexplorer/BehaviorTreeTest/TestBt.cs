using Godot;
using Godot.DependencyInjection.Attributes;
using GodotStateMachine;
using GroveGames.BehaviourTree.Collections;
using GroveGames.BehaviourTree.Nodes;
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

    // Called when the node enters the scene tree for the first time.
    private Test_Tree test_Tree = default!;
    public override void _Ready()
    {
        _navigationAgent2D = GetNode<NavigationAgent2D>(nameof(NavigationAgent2D));
        //BT
        test_Tree = new Test_Tree();

        var blackboard = new Blackboard();

        test_Tree.SetRoot(new Root(blackboard));

        test_Tree.SetupTree();
        test_Tree.Enable();
        AddChild(test_Tree);

        _logger.LogInformation("Test BT Node Added");

        //StateMachine
        _enemyStateMachine = new StateMachine();
        _enemyIdle = new EnemyIdle(_enemyStateMachine);
        _enemyFollow = new EnemyFollow(_enemyStateMachine);

        _enemyIdle.AddEnter(() => _isIdle = true)
            .AddExit(() => _isIdle = false);

        _enemyFollow.AddEnter(() => _isFollow = true)
            .AddExit(() => _isFollow = false);

        _enemyStateMachine.SetInitialState(_enemyIdle);

        _logger.LogInformation("Enemy stateMachine is init!");


    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {

    }
    public override void _PhysicsProcess(double delta)
    {
        //BT tick
        test_Tree.Tick((float)delta);
        //StateMachine's process.
        _enemyStateMachine.PhysicsProcess(delta);
    }
}
