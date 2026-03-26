using Godot;

public partial class SettingsMenu : CanvasLayer
{
    public static SettingsMenu Instance { get; private set; }

    private VBoxContainer _mainPage;
    private VBoxContainer _displayPage;
    private VBoxContainer _audioPage;

    public override void _Ready()
    {
        Instance = this;
        Layer = 200;
        Visible = false;

        var bg = new ColorRect();
        bg.Color = new Color(0, 0, 0, 0.85f);
        bg.SetAnchorsPreset(Control.LayoutPreset.FullRect);
        AddChild(bg);

        _mainPage    = BuildMainPage();
        _displayPage = BuildDisplayPage();
        _audioPage   = BuildAudioPage();

        AddChild(_mainPage);
        AddChild(_displayPage);
        AddChild(_audioPage);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!@event.IsActionPressed("ui_cancel")) return;

        if (!Visible)
        {
            Toggle();
        }
        else if (!_mainPage.Visible)
        {
            ShowPage(_mainPage);
        }
        else
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        Visible = !Visible;
        if (Visible) ShowPage(_mainPage);
        Input.MouseMode = Visible ? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Captured;
    }

    // ── Pages ────────────────────────────────────────────────────────────────

    private VBoxContainer BuildMainPage()
    {
        var box = MakeBox();
        AddTitle(box, "PAUSED");
        box.AddChild(new HSeparator());
        Btn(box, "RESUME",  Toggle);
        Btn(box, "DISPLAY", () => ShowPage(_displayPage));
        Btn(box, "AUDIO",   () => ShowPage(_audioPage));
        box.AddChild(new HSeparator());
        Btn(box, "QUIT", () => GetTree().Quit());
        return box;
    }

    private VBoxContainer BuildDisplayPage()
    {
        var box = MakeBox();
        AddTitle(box, "DISPLAY");
        box.AddChild(new HSeparator());
        Btn(box, "Fullscreen",  () => DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen));
        Btn(box, "Borderless",  () => DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen));
        Btn(box, "Windowed",    () => DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed));
        box.AddChild(new HSeparator());
        Btn(box, "640 x 480",   () => ResizeWindow(640,  480));
        Btn(box, "960 x 720",   () => ResizeWindow(960,  720));
        Btn(box, "1280 x 960",  () => ResizeWindow(1280, 960));
        Btn(box, "1920 x 1080", () => ResizeWindow(1920, 1080));
        box.AddChild(new HSeparator());
        Btn(box, "< BACK", () => ShowPage(_mainPage));
        return box;
    }

    private VBoxContainer BuildAudioPage()
    {
        var box = MakeBox();
        AddTitle(box, "AUDIO");
        box.AddChild(new HSeparator());

        AddLabel(box, "Master");
        var master = MakeSlider(Mathf.DbToLinear(AudioServer.GetBusVolumeDb(0)));
        master.ValueChanged += v =>
            AudioServer.SetBusVolumeDb(0, v > 0.001 ? Mathf.LinearToDb((float)v) : -80f);
        box.AddChild(master);

        AddLabel(box, "Music");
        var music = MakeSlider(MusicManager.Instance._MusicVolume);
        music.ValueChanged += v => MusicManager.Instance.SetMusicVolume((float)v);
        box.AddChild(music);

        box.AddChild(new HSeparator());
        Btn(box, "< BACK", () => ShowPage(_mainPage));
        return box;
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void ShowPage(VBoxContainer page)
    {
        _mainPage.Visible    = false;
        _displayPage.Visible = false;
        _audioPage.Visible   = false;
        page.Visible = true;
    }

    private VBoxContainer MakeBox()
    {
        var box = new VBoxContainer();
        box.SetAnchorsPreset(Control.LayoutPreset.Center);
        box.GrowHorizontal = Control.GrowDirection.Both;
        box.GrowVertical   = Control.GrowDirection.Both;
        box.CustomMinimumSize = new Vector2(130, 0);
        box.AddThemeConstantOverride("separation", 3);
        return box;
    }

    private void AddTitle(Control parent, string text)
    {
        var lbl = new Label();
        lbl.Text = text;
        lbl.HorizontalAlignment = HorizontalAlignment.Center;
        lbl.AddThemeFontSizeOverride("font_size", 10);
        parent.AddChild(lbl);
    }

    private void AddLabel(Control parent, string text)
    {
        var lbl = new Label();
        lbl.Text = text;
        lbl.AddThemeFontSizeOverride("font_size", 7);
        parent.AddChild(lbl);
    }

    private void Btn(Control parent, string text, System.Action action)
    {
        var btn = new Button();
        btn.Text = text;
        btn.AddThemeFontSizeOverride("font_size", 8);
        btn.Pressed += action;
        parent.AddChild(btn);
    }

    private HSlider MakeSlider(float initial)
    {
        var s = new HSlider();
        s.MinValue = 0.0;
        s.MaxValue = 1.0;
        s.Step     = 0.05;
        s.Value    = Mathf.Clamp(initial, 0f, 1f);
        s.CustomMinimumSize = new Vector2(120, 10);
        return s;
    }

    private void ResizeWindow(int w, int h)
    {
        DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
        DisplayServer.WindowSetSize(new Vector2I(w, h));
        var screen = DisplayServer.ScreenGetSize();
        DisplayServer.WindowSetPosition((screen - new Vector2I(w, h)) / 2);
    }
}
