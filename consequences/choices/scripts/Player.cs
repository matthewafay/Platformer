using Godot;

public partial class Player : CharacterBody3D
{
    private const float Speed = 5.0f;
    private const float MouseSensitivity = 0.003f;

    private Camera3D _camera;

    public override void _Ready()
    {
        _camera = GetNode<Camera3D>("Camera3D");
        Input.MouseMode = Input.MouseModeEnum.Captured;

        string spawnName = "Spawn_" + GameState.Instance.SpawnTarget.Capitalize();
        var spawnNode = GetTree().CurrentScene.FindChild(spawnName, true, false) as Node3D;
        if (spawnNode != null)
            GlobalPosition = spawnNode.GlobalPosition;

        DreamManager.Instance.StartDream();
        FadeOverlay.Instance.FadeIn(0.8f);
        MusicManager.Instance.PlayForScene(GetTree().CurrentScene.Name);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion motion)
        {
            RotateY(-motion.Relative.X * MouseSensitivity);
            _camera.RotateX(-motion.Relative.Y * MouseSensitivity);
            _camera.Rotation = new Vector3(
                Mathf.Clamp(_camera.Rotation.X, -Mathf.Pi / 2f, Mathf.Pi / 2f),
                _camera.Rotation.Y,
                _camera.Rotation.Z);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!IsOnFloor())
            Velocity += GetGravity() * (float)delta;

        var forward = -Transform.Basis.Z;
        var right = Transform.Basis.X;
        var moveDir = Vector3.Zero;

        if (Input.IsKeyPressed(Key.W)) moveDir += forward;
        if (Input.IsKeyPressed(Key.S)) moveDir -= forward;
        if (Input.IsKeyPressed(Key.A)) moveDir -= right;
        if (Input.IsKeyPressed(Key.D)) moveDir += right;

        moveDir.Y = 0f;
        if (moveDir.LengthSquared() > 0f)
        {
            moveDir = moveDir.Normalized();
            Velocity = new Vector3(moveDir.X * Speed, Velocity.Y, moveDir.Z * Speed);
        }
        else
        {
            Velocity = new Vector3(
                Mathf.MoveToward(Velocity.X, 0f, Speed),
                Velocity.Y,
                Mathf.MoveToward(Velocity.Z, 0f, Speed));
        }

        MoveAndSlide();
    }
}
