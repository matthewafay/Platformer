using Godot;

public partial class MusicManager : Node
{
    public static MusicManager Instance { get; private set; }

    public float _MusicVolume = 1.0f;

    private AudioStreamPlayer _player;
    private string _currentScene = "";
    private Tween _tween;

    public override void _Ready()
    {
        Instance = this;
        _player = new AudioStreamPlayer();
        _player.VolumeDb = 0f;
        AddChild(_player);
    }

    public void PlayForScene(string sceneName)
    {
        GD.Print($"[Music] PlayForScene: '{sceneName}'");
        if (sceneName == _currentScene) { GD.Print("[Music] same scene, skip"); return; }
        _currentScene = sceneName;

        string path = $"res://assets/bgmusic/{sceneName}.wav";
        GD.Print($"[Music] loading: {path}");

        AudioStreamWav stream = null;
        try { stream = GD.Load<AudioStreamWav>(path); }
        catch (System.Exception e) { GD.PrintErr($"[Music] load exception: {e.Message}"); }

        if (stream == null)
        {
            GD.Print("[Music] stream null, fading out");
            FadeOut();
            return;
        }

        GD.Print($"[Music] loaded ok. format={stream.Format} rate={stream.MixRate} bytes={stream.Data.Length}");

        _tween?.Kill();

        if (_player.Playing)
        {
            _tween = CreateTween();
            _tween.TweenProperty(_player, "volume_db", -80f, 0.5);
            _tween.TweenCallback(Callable.From(() =>
            {
                _player.Stop();
                _player.Stream = stream;
                _player.VolumeDb = TargetDb();
                _player.Play();
                GD.Print("[Music] crossfade complete, now playing");
            }));
        }
        else
        {
            _player.Stream = stream;
            _player.VolumeDb = TargetDb();
            _player.Play();
            GD.Print($"[Music] play called, _player.Playing={_player.Playing}");
        }
    }

    public void SetMusicVolume(float linear)
    {
        _MusicVolume = Mathf.Clamp(linear, 0f, 1f);
        if (_player.Playing)
            _player.VolumeDb = TargetDb();
    }

    private float TargetDb() => _MusicVolume > 0.001f ? Mathf.LinearToDb(_MusicVolume) : -80f;

    private void FadeOut()
    {
        if (!_player.Playing) return;
        _tween?.Kill();
        _tween = CreateTween();
        _tween.TweenProperty(_player, "volume_db", -80f, 0.8);
        _tween.TweenCallback(Callable.From(() => _player.Stop()));
    }
}
