using Godot;

public partial class GrayMan : CharacterBody3D
{
    [Export] public float MoveSpeed = 0.7f;
    [Export] public float ContactDistance = 1.4f;

    private Node3D _player;
    private bool _triggered = false;

    public override void _Ready()
    {
        var players = GetTree().GetNodesInGroup("player");
        if (players.Count > 0)
            _player = players[0] as Node3D;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_player == null || _triggered) return;

        float dist = GlobalPosition.DistanceTo(_player.GlobalPosition);
        if (dist < ContactDistance)
        {
            _triggered = true;
            GameState.Instance.EndDream();
            return;
        }

        var dir = (_player.GlobalPosition - GlobalPosition).Normalized();
        dir.Y = 0f;

        if (!IsOnFloor())
            Velocity += GetGravity() * (float)delta;

        Velocity = new Vector3(dir.X * MoveSpeed, Velocity.Y, dir.Z * MoveSpeed);
        MoveAndSlide();

        if (dir.LengthSquared() > 0.01f)
        {
            var lookTarget = GlobalPosition + new Vector3(dir.X, 0, dir.Z);
            LookAt(lookTarget, Vector3.Up);
        }
    }
}
