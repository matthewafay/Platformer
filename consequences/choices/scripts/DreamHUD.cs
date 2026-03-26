using Godot;

public partial class DreamHUD : CanvasLayer
{
    public static DreamHUD Instance { get; private set; }

    private Label _timerLabel;
    private Label _dayLabel;
    private Label _moodLabel;

    public override void _Ready()
    {
        Instance = this;
        Layer = 50;

        var panel = new VBoxContainer();
        panel.AnchorRight = 1.0f;
        panel.OffsetLeft = -76f;
        panel.OffsetRight = -4f;
        panel.OffsetTop = 4f;
        AddChild(panel);

        _dayLabel = new Label();
        _dayLabel.HorizontalAlignment = HorizontalAlignment.Right;
        _dayLabel.AddThemeFontSizeOverride("font_size", 8);
        _dayLabel.Text = "Day 1";
        panel.AddChild(_dayLabel);

        _timerLabel = new Label();
        _timerLabel.HorizontalAlignment = HorizontalAlignment.Right;
        _timerLabel.AddThemeFontSizeOverride("font_size", 8);
        _timerLabel.Text = "3:00";
        panel.AddChild(_timerLabel);

        _moodLabel = new Label();
        _moodLabel.HorizontalAlignment = HorizontalAlignment.Right;
        _moodLabel.AddThemeFontSizeOverride("font_size", 8);
        _moodLabel.Text = "~";
        panel.AddChild(_moodLabel);
    }

    public override void _Process(double delta)
    {
        if (!DreamManager.Instance.IsDreaming) return;

        float t = DreamManager.Instance.TimeRemaining;
        int mins = Mathf.FloorToInt(t / 60f);
        int secs = Mathf.FloorToInt(t % 60f);
        _timerLabel.Text = $"{mins}:{secs:D2}";
        _dayLabel.Text = $"Day {GameState.Instance.DayCount}";

        float ud = GameState.Instance.UpperDowner;
        _moodLabel.Text = ud > 2f ? "^ upper" : ud < -2f ? "v downer" : "~ neutral";
    }
}
