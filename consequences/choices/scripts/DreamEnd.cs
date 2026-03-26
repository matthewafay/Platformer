using Godot;

public partial class DreamEnd : Control
{
    private bool _advanced = false;

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
        DreamManager.Instance.StopDream();
        FadeOverlay.Instance.FadeIn(0.6f);
        MusicManager.Instance.PlayForScene("DreamEnd");

        GetNode<Label>("VBox/DayLabel").Text = "Day  " + GameState.Instance.DayCount;
        float ud = GameState.Instance.UpperDowner;
        string mood = ud > 2.0f ? "Upper" : ud < -2.0f ? "Downer" : "Neutral";
        GetNode<Label>("VBox/MoodLabel").Text = "Mood:  " + mood;

        GetNode<Timer>("WakeTimer").Timeout += OnWakeTimerTimeout;
        GetNode<Timer>("WakeTimer").Start();
        SetProcessUnhandledInput(true);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("interact") || @event.IsActionPressed("ui_accept"))
            Advance();
    }

    private void OnWakeTimerTimeout() => Advance();

    private void Advance()
    {
        if (_advanced) return;
        _advanced = true;
        SetProcessUnhandledInput(false);
        var next = GameState.Instance.GetRandomDreamScene();
        GameState.Instance.UpperDowner = 0.0f;
        GameState.Instance.StaticDynamic = 0.0f;
        GameState.Instance.SpawnTarget = next.Spawn;
        FadeOverlay.Instance.FadeAndCall(new Color(0, 0, 0, 1), 0.5f,
            () => GetTree().ChangeSceneToFile(next.Scene));
    }
}
