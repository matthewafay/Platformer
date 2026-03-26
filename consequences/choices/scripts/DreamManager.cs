using Godot;

public partial class DreamManager : Node
{
    public static DreamManager Instance { get; private set; }

    [Export] public float DreamDuration = 180.0f;

    public float TimeRemaining = 0.0f;
    public bool IsDreaming = false;

    [Signal] public delegate void DreamTimeUpdatedEventHandler(float secondsLeft);

    public override void _Ready()
    {
        Instance = this;
    }

    public void StartDream()
    {
        TimeRemaining = DreamDuration;
        IsDreaming = true;
    }

    public void StopDream()
    {
        IsDreaming = false;
    }

    public override void _Process(double delta)
    {
        if (!IsDreaming) return;

        TimeRemaining -= (float)delta;
        EmitSignal(SignalName.DreamTimeUpdated, TimeRemaining);

        if (TimeRemaining <= 0.0f)
        {
            IsDreaming = false;
            GameState.Instance.EndDream();
        }
    }
}
