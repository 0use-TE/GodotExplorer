using Godot;

namespace PlatformExplorer.player
{
    public class PlayerInput
    {
        public float Horizontal => Input.GetAxis("ui_left", "ui_right");
        public float Vertical => Input.GetAxis("ui_up", "ui_down");

    }
}
