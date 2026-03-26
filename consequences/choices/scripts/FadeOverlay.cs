using Godot;

public partial class FadeOverlay : CanvasLayer
{
    public static FadeOverlay Instance { get; private set; }

    private ColorRect _rect;
    private Tween _tween;

    public override void _Ready()
    {
        Instance = this;
        Layer = 100;

        _rect = new ColorRect();
        _rect.SetAnchorsPreset(Control.LayoutPreset.FullRect);
        _rect.Color = new Color(0, 0, 0, 1);
        _rect.MouseFilter = Control.MouseFilterEnum.Ignore;
        AddChild(_rect);
    }

    public void FadeIn(float duration = 0.5f) =>
        DoTween(new Color(0, 0, 0, 0), duration, null);

    public void FadeOut(float duration = 0.5f) =>
        DoTween(new Color(0, 0, 0, 1), duration, null);

    public void FadeAndCall(Color color, float duration, System.Action callback) =>
        DoTween(color, duration, callback);

    private void DoTween(Color target, float duration, System.Action callback)
    {
        _tween?.Kill();
        _tween = CreateTween();
        _tween.TweenProperty(_rect, "color", target, duration);
        if (callback != null)
            _tween.TweenCallback(Callable.From(callback));
    }
}
