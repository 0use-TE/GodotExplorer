using Godot;

namespace PlatformExplorer.player
{
    public class PlayerInput
    {
        public float Horizontal => Input.GetAxis("ui_left", "ui_right");
        public bool JumpPressed => Input.IsActionJustPressed("ui_accept");
    }
}
