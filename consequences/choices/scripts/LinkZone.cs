using Godot;

public partial class LinkZone : Area3D
{
    [Export] public string LinkType = "dynamic";
    [Export] public string StaticTarget = "";
    [Export] public string StaticSpawn = "";
    [Export] public float MoodUd = 0.0f;
    [Export] public float MoodSd = 0.0f;
    [Export] public Color FadeColor = new Color(0, 0, 0, 1);

    private bool _triggered = false;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node3D body)
    {
        if (_triggered) return;
        if (!body.IsInGroup("player")) return;
        _triggered = true;
        GameState.Instance.AddMood(MoodUd, MoodSd);
        FadeOverlay.Instance.FadeAndCall(FadeColor, 0.4f, DoLink);
    }

    private void DoLink()
    {
        if (LinkType == "static" && StaticTarget != "")
        {
            GameState.Instance.SpawnTarget = StaticSpawn;
            GetTree().ChangeSceneToFile(StaticTarget);
        }
        else
        {
            var dest = GameState.Instance.GetRandomDreamScene();
            GameState.Instance.SpawnTarget = dest.Spawn;
            GetTree().ChangeSceneToFile(dest.Scene);
        }
    }
}
