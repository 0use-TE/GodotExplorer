using Godot;

namespace PlatformExplorer.Player
{
    public class PlayerInput
    {
        public float Horizontal => Input.GetAxis("ui_left", "ui_right");
        public bool JumpPressed => Input.IsActionJustPressed("ui_up");
    }
}
