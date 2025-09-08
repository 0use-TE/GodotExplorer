using Godot;
using Godot.DependencyInjection.Attributes;
using GroveGames.BehaviourTree.Collections;
using GroveGames.BehaviourTree.Nodes;
using Microsoft.Extensions.Logging;
namespace PlatformExplorer.BehaviorTreeTest;
public partial class TestBt : CharacterBody2D
{
    [Inject]
    private ILogger<TestBt> _logger = default!;

    private NavigationAgent2D _navigationAgent2D;
    
    public record TestBtBlackboard(Node2D player);

    // Called when the node enters the scene tree for the first time.
    private Test_Tree test_Tree = default!;
    public override void _Ready()
    {
        _navigationAgent2D = GetNode<NavigationAgent2D>(nameof(NavigationAgent2D));

		test_Tree = new Test_Tree(_navigationAgent2D,this);

        var blackboard = new Blackboard();
		
        var player = GetTree().Root.GetNode<Node2D>("MainScene/Player");

		var testBtBlackboard = new TestBtBlackboard(player);

        blackboard.SetValue(nameof(TestBtBlackboard), testBtBlackboard);

        test_Tree.SetRoot(new Root(blackboard));

        test_Tree.SetupTree();

        test_Tree.Enable();

        AddChild(test_Tree);

        _logger.LogInformation("Test BT Node Added");

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
	}
	public override void _PhysicsProcess(double delta)
	{
		test_Tree.Tick((float)delta);
	}
}
