using Godot;
namespace PlatformExplorer.player;
public partial class Player : Node2D
{
    private CharacterBody2D? _body;
    private AnimationTree? _animationTree;

    // 配置参数
    private float _speed = 32 * 4;       // 水平移动速度
    private float _jumpForce = -500f;    // 跳跃向上力（负值）

    private float _inputX = 0f;



    public override void _Ready()
    {
        _body = GetNode<CharacterBody2D>(nameof(CharacterBody2D));
        _animationTree = _body.GetNode<AnimationTree>(nameof(AnimationTree));


    }


    public override void _Process(double delta)
    {
        _inputX = Input.GetAxis("ui_left", "ui_right");

    }


    public override void _PhysicsProcess(double delta)
    {
    }

}

