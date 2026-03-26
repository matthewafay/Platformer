using Godot;

public partial class GameState : Node
{
    public static GameState Instance { get; private set; }

    public string SpawnTarget = "outdoor";
    public int DayCount = 1;
    public float UpperDowner = 0.0f;
    public float StaticDynamic = 0.0f;

    public struct DreamSceneData
    {
        public string Scene;
        public string Spawn;
        public float Ud;
        public float Sd;
    }

    public DreamSceneData[] DreamScenes = new DreamSceneData[]
    {
        new DreamSceneData { Scene = "res://scenes/OutdoorRoom.tscn", Spawn = "outdoor",  Ud =  0.0f, Sd =  0.5f },
        new DreamSceneData { Scene = "res://scenes/HouseInterior.tscn", Spawn = "interior", Ud =  1.0f, Sd = -1.0f },
        new DreamSceneData { Scene = "res://scenes/DarkAlley.tscn",    Spawn = "alley",    Ud = -3.0f, Sd =  1.0f },
        new DreamSceneData { Scene = "res://scenes/SurrealGarden.tscn", Spawn = "garden",   Ud =  3.0f, Sd =  2.0f },
        new DreamSceneData { Scene = "res://scenes/HotDogDream.tscn", Spawn = "hotdog", Ud = 2.5f, Sd = 3.0f },
        new DreamSceneData { Scene = "res://scenes/CorridorEndless.tscn", Spawn = "corridor", Ud = -2.0f, Sd =  2.0f },
        new DreamSceneData { Scene = "res://scenes/VoidSpace.tscn",       Spawn = "void",     Ud =  0.0f, Sd =  1.5f },
        new DreamSceneData { Scene = "res://scenes/AbandonedFair.tscn",   Spawn = "fairground", Ud = -2.5f, Sd = 1.0f },
    };

    public override void _Ready()
    {
        Instance = this;
        DisplayServer.WindowSetSize(new Vector2I(1920, 1080));
        var center = DisplayServer.ScreenGetSize() / 2 - new Vector2I(960, 540);
        DisplayServer.WindowSetPosition(center);
    }

    public void AddMood(float ud, float sd)
    {
        UpperDowner = Mathf.Clamp(UpperDowner + ud, -10.0f, 10.0f);
        StaticDynamic = Mathf.Clamp(StaticDynamic + sd, -10.0f, 10.0f);
    }

    public void EndDream()
    {
        DayCount++;
        DreamManager.Instance.StopDream();
        FadeOverlay.Instance.FadeAndCall(new Color(0, 0, 0, 1), 0.5f,
            () => GetTree().ChangeSceneToFile("res://scenes/DreamEnd.tscn"));
    }

    public DreamSceneData GetRandomDreamScene()
    {
        if (UpperDowner > 5.0f) return DreamScenes[3];
        if (UpperDowner < -5.0f) return DreamScenes[2];
        return DreamScenes[GD.RandRange(0, DreamScenes.Length - 1)];
    }
}
