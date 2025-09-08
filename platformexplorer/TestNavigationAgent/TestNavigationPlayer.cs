using Godot;
using System;

public partial class TestNavigationPlayer : CharacterBody2D
{
	private NavigationAgent2D _navigationAgent;
	private float _speed = 200f;

	// 点击点可视化的节点
	private Sprite2D _targetMarker;

	public override void _Ready()
	{
		_navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");

		// 创建一个紫色圆点作为目标标记
		_targetMarker = new Sprite2D();
		var texture = new ImageTexture();
		var image = Image.Create(16, 16, false, Image.Format.Rgba8);
		image.Fill(new Color(1f, 0f, 1f)); // 紫色
		texture.SetImage(image);
		_targetMarker.Texture = texture;
		_targetMarker.Visible = false;
		_targetMarker.ZIndex = 100; // 确保标记在前面显示
		   AddChild(_targetMarker);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
		{
			// 获取鼠标点击位置（世界坐标）
			Vector2 clickPosition = GetGlobalMousePosition();

			// 设置导航目标
			_navigationAgent.TargetPosition = clickPosition;

			// 显示标记
			_targetMarker.GlobalPosition= clickPosition;
			_targetMarker.Visible = true;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_navigationAgent.IsNavigationFinished())
			return;

		// 获取路径的下一个点
		Vector2 nextPos = _navigationAgent.GetNextPathPosition();

		// 计算移动方向并设置速度
		Vector2 dir = (nextPos - GlobalPosition).Normalized();
		Velocity = dir * _speed;

		MoveAndSlide();
	}
}
