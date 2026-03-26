using Godot;

public partial class Door : Area3D
{
    [Export] public string TargetScene = "";
    [Export] public string SpawnTarget = "";

    private bool _playerNearby = false;
    private bool _transitioning = false;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (_playerNearby && !_transitioning && @event.IsActionPressed("interact"))
        {
            if (string.IsNullOrEmpty(TargetScene))
            {
                GD.PrintErr($"Door '{Name}' has no TargetScene set.");
                return;
            }
            _transitioning = true;
            GetViewport().SetInputAsHandled();
            GameState.Instance.SpawnTarget = SpawnTarget;
            FadeOverlay.Instance.FadeAndCall(new Color(0, 0, 0, 1), 0.4f,
                () => GetTree().ChangeSceneToFile(TargetScene));
        }
    }

    private void OnBodyEntered(Node3D body)
    {
        if (!body.IsInGroup("player")) return;
        _playerNearby = true;
        GetNode<Label3D>("HintLabel").Visible = true;
    }

    private void OnBodyExited(Node3D body)
    {
        if (!body.IsInGroup("player")) return;
        _playerNearby = false;
        GetNode<Label3D>("HintLabel").Visible = false;
    }
}
