using Godot;
using Godot.DependencyInjection.Attributes;
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

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _navigationAgent2D = GetNode<NavigationAgent2D>(nameof(NavigationAgent2D));
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


        //Start build behaviourtree
        _logger.LogInformation("Start build BehaviourTree");

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {

    }
    public override void _PhysicsProcess(double delta)
    {
        //BT tick
        //StateMachine's process.
        _enemyStateMachine.PhysicsProcess(delta);
    }
}
