using Godot;
using GodotStateMachine;
namespace PlatformExplorer.player;
public partial class Player : Node2D
{
    private CharacterBody2D? _body;
    private AnimatedSprite2D _animatedSprite;


    // 配置参数
    private float _horizontalSpeed = 32 * 2.5f;       // 水平移动速度
    private float _verticalSpeed = 32 * 2.5f;       // 水平移动速度
    private float _speed = 32 * 5;       // 水平移动速度
    private float _jumpForce = -350f;    // 跳跃向上力（负值）
    private int _currentJumpCount = 0;
    private int _maxJumpCount = 2;       // 最大跳跃次数（1表示单跳，2表示双跳）

    //输入相关
    private PlayerInput _playerInput = new PlayerInput();
    //状态相关
    private StateMachine _playerStateMachine;
    private PlayerIdleState _playerIdleState;
    private PlayerWalkState _playerWalkState;



    private bool _isIdle;
    private bool _isWalk;
    public override void _Ready()
    {
        _body = GetNode<CharacterBody2D>(nameof(CharacterBody2D));
        _animatedSprite = _body.GetNode<AnimatedSprite2D>(nameof(AnimatedSprite2D));

        _playerStateMachine = new StateMachine();
        _playerIdleState = new PlayerIdleState(_playerStateMachine);
        _playerWalkState = new PlayerWalkState(_playerStateMachine);

        //Idle
        _playerIdleState.AddEnter(() => _isIdle = true).AddEnter(() => SetVelocity(0, 0)).
                AddTransitions(() => Mathf.Abs(_playerInput.Horizontal) > 0.1f || Mathf.Abs(_playerInput.Vertical) > .1f, _playerWalkState).
                AddExit(() => _isIdle = false);

        _playerWalkState.AddEnter(() => _isWalk = true).
            AddTransitions(() => Mathf.Abs(_playerInput.Horizontal) < 0.1f && Mathf.Abs(_playerInput.Vertical) < .1f, _playerIdleState).
            AddPhysicsProcess((delta) => SetVelocity(_playerInput.Horizontal * _horizontalSpeed, _playerInput.Vertical * _verticalSpeed)).
            AddExit(() => _isWalk = false);


        _playerStateMachine.SetInitialState(_playerIdleState);
    }


    public override void _Process(double delta)
    {
        _playerStateMachine.Process(delta);
        if (_playerInput.Horizontal > 0.1)
            _animatedSprite.FlipH = false;
        else if (_playerInput.Horizontal < -0.1)
            _animatedSprite.FlipH = true;
    }


    public override void _PhysicsProcess(double delta)
    {
        _playerStateMachine.PhysicsProcess(delta);

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

