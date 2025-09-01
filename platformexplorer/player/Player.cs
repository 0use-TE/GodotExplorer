using Godot;
using GodotStateMachine;

namespace PlatformExplorer.Player;
public partial class Player : Node2D
{
    private CharacterBody2D? _body;

    // 配置参数
    private float _speed = 32 * 4;       // 水平移动速度
    private float _jumpForce = -500f;    // 跳跃向上力（负值）

    // 输入相关
    private PlayerInput _playerInput = new PlayerInput();

    // 状态相关
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

        // Idle
        _playerIdleState.OnEnter += () => _isIdle = true;
        _playerIdleState.AddTransitions(
            (() => Mathf.Abs(_playerInput.Horizontal) > 0.1f, _playerRunState),
            (() => _playerInput.JumpPressed && _body.IsOnFloor(), _playerJumpState)
        );
        _playerIdleState.OnExit += () => _isIdle = false;

        // Run
        _playerRunState.OnEnter += () => _isRun = true;
        _playerRunState.AddTransitions(
            (() => Mathf.Abs(_playerInput.Horizontal) < 0.1f, _playerIdleState)
        );
        _playerRunState.OnPhysicsProcess += (delta) =>
        {
            SetVelocity(x: _playerInput.Horizontal * _speed);
        };
        _playerRunState.OnExit += () => _isRun = false;

        // Jump
        _playerJumpState.OnEnter += () =>
        {
            _isJump = true;
            SetVelocity(y: _jumpForce);
        };
        _playerJumpState.AddTransitions(
            (() => _body.IsOnFloor(), _playerIdleState)
        );
        _playerJumpState.OnPhysicsProcess += (delta) =>
        {
            SetVelocity(x: _playerInput.Horizontal * _speed);
        };
        _playerJumpState.OnExit += () => _isJump = false;

        // 初始状态
        _playerStateMachine.SetInitialState(_playerIdleState);
    }

    public override void _Process(double delta)
    {
        _playerStateMachine.Process(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        _playerStateMachine.PhysicsProcess(delta);

        // 重力叠加
        AddVelocity(y: _body.GetGravity().Y * (float)delta);

        // 移动
        _body.MoveAndSlide();
    }

    // ---- 封装速度修改 ----
    private void SetVelocity(float? x = null, float? y = null)
    {
        _body.Velocity = new Vector2(
            x ?? _body.Velocity.X,
            y ?? _body.Velocity.Y
        );
    }

    private void AddVelocity(float? x = null, float? y = null)
    {
        _body.Velocity += new Vector2(
            x ?? 0,
            y ?? 0
        );
    }
}
