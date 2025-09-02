using Godot;
using GodotStateMachine;
namespace PlatformExplorer.player;
public partial class Player : Node2D
{
    private CharacterBody2D? _body;

    // 配置参数
    private float _speed = 32 * 4;       // 水平移动速度
    private float _jumpForce = -350f;    // 跳跃向上力（负值）
    private int _direction = 1;
    //输入相关
    private PlayerInput _playerInput = new PlayerInput();
    //状态相关
    private StateMachine _playerStateMachine;
    private PlayerIdleState _playerIdleState;
    private PlayerRunState _playerRunState;
    private PlayerJumpState _playerJumpState;

    private AnimatedSprite2D _animatedSprite;

    private bool _isIdle;
    private bool _isRun;
    private bool _isJump;
    public override void _Ready()
    {
        _body = GetNode<CharacterBody2D>(nameof(CharacterBody2D));
        _animatedSprite = _body.GetNode<AnimatedSprite2D>(nameof(AnimatedSprite2D));

        _playerStateMachine = new StateMachine();
        _playerIdleState = new PlayerIdleState(_playerStateMachine);
        _playerRunState = new PlayerRunState(_playerStateMachine);
        _playerJumpState = new PlayerJumpState(_playerStateMachine);


        _playerIdleState.OnEnter += () => _isIdle = true;
        _playerIdleState.AddTransitions(
         (() => Mathf.Abs(_playerInput.Horizontal) > 0.1f, _playerRunState),
         (() => _playerInput.JumpPressed && _body.IsOnFloor(), _playerJumpState));

        _playerIdleState.OnExit += () => _isIdle = false;


        _playerRunState.OnEnter += () => _isRun = true;
        _playerRunState.AddTransitions((() => Mathf.Abs(_playerInput.Horizontal) < 0.1f, _playerIdleState),
            (() => _playerInput.JumpPressed && _body.IsOnFloor(), _playerJumpState)
        );
        _playerRunState.OnPhysicsProcess += (delta) =>
        {
            SetVelocity(x: _playerInput.Horizontal * _speed);
        };
        _playerRunState.OnExit += () => _isRun = false;


        _playerJumpState.OnEnter += () =>
        {
            _isJump = true;
            SetVelocity(y: _jumpForce);
        };
        _playerJumpState.Transitions.Add(() => _body.IsOnFloor(), _playerIdleState);
        _playerJumpState.OnPhysicsProcess += (delta) => SetVelocity(x: _playerInput.Horizontal * _speed);
        _playerJumpState.OnExit += () => _isJump = false;
        //设置默认状态
        _playerStateMachine.SetInitialState(_playerIdleState);
    }


    public override void _Process(double delta)
    {
        _playerStateMachine.Process(delta);
    }


    public override void _PhysicsProcess(double delta)
    {
        _playerStateMachine.PhysicsProcess(delta);
        //设置重力
        AddVelocity(y: _body.GetGravity().Y * (float)delta);
        _body.MoveAndSlide();
    }

    private void SetVelocity(float? x = null, float? y = null)
    {
        var velocity = _body.Velocity;
        if (x.HasValue) velocity.X = x.Value;
        if (y.HasValue) velocity.Y = y.Value;
        _body.Velocity = velocity;
    }

    private void AddVelocity(float? x = null, float? y = null)
    {
        var velocity = _body.Velocity;
        if (x.HasValue) velocity.X += x.Value;
        if (y.HasValue) velocity.Y += y.Value;
        _body.Velocity = velocity;
    }



}

