using Godot;

public partial class FloatingAnimator : Node3D
{
    [Export] public float BobAmplitude = 0.3f;
    [Export] public float BobSpeed = 1.0f;
    [Export] public float RotateSpeed = 0.5f;
    [Export] public bool DoRotate = false;

    private float _baseY;
    private float _time;

    public override void _Ready()
    {
        _baseY = Position.Y;
        _time = (float)GD.RandRange(0.0, Mathf.Tau);
    }

    public override void _Process(double delta)
    {
        _time += (float)delta;
        Position = new Vector3(Position.X, _baseY + Mathf.Sin(_time * BobSpeed) * BobAmplitude, Position.Z);
        if (DoRotate)
            RotateY(RotateSpeed * (float)delta);
    }
}
