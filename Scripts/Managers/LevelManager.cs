namespace Sankari;

public partial class LevelManager
{
    public Dictionary<string, Level> Levels { get; set; } = new();

    public Dictionary<string, PackedScene> Scenes { get; set; } = new();

    public string CurrentLevel { get; set; }

    private Node NodeLevel { get; }

    public LevelManager(Node nodeLevel)
    {
        NodeLevel = nodeLevel;
        var godotFileManager = new GodotFileManager();
        godotFileManager.LoadDir("Scenes/Levels", (dir, fileName) =>
        {
            if (!dir.CurrentIsDir())
                Scenes[fileName.Replace(".tscn", "")] = ResourceLoader.Load<PackedScene>($"res://Scenes/Levels/{fileName}");
        });

        AddLevel("Level A1", "grassy_1", 0.9f, false);
        AddLevel("Level A2", "grassy_2", 0.9f);

        // unused
        AddLevel("Level B1", "", 1, false);

        // unused
        AddLevel("Level C1", "", 1, false);
    }

    private void AddLevel(string name, string music = "", float musicPitch = 1, bool locked = true) =>
        Levels.Add(name, new Level(name) {
            Locked = locked,
            Music = music,
            MusicPitch = musicPitch
        });

    public async Task LoadLevel()
    {
        await GameManager.Transition.AlphaToBlackAndBack();
        LoadALevel();
    }

    public void LoadLevelFast() => LoadALevel();

    private void LoadALevel()
    {
        // remove map
        GameManager.DestroyMap();

        // remove level if any
        NodeLevel.QueueFreeChildren();

        // load level
        var scenePath = $"res://Scenes/Levels/{CurrentLevel}.tscn";
        if (!File.FileExists(scenePath))
        {
            Logger.LogWarning("Level has not been made yet!");
            return;
        }

        var levelPacked = ResourceLoader.Load<PackedScene>(scenePath);
        var level = (LevelScene)levelPacked.Instantiate();
        level.PreInit();
        NodeLevel.AddChild(level);

        var curLevel = Levels[CurrentLevel];

        GameManager.Audio.PlayMusic(curLevel.Music, curLevel.MusicPitch);
    }

    public async Task CompleteLevel(string levelName)
    {
        // the level scene is no longer active, clean it up
        GameManager.Notifications.RemoveListener(GameManager.LevelScene, Event.OnGameClientLeft);
        GameManager.LevelScene = null;

        await GameManager.Transition.AlphaToBlackAndBack();

        // remove level
        NodeLevel.QueueFreeChildren();

        // mark level as completed
        Levels[levelName].Completed = true;

        foreach (var level in Levels[levelName].Unlocks)
            if (Levels.ContainsKey(level))
                Levels[level].Locked = false;

        // load map
        GameManager.LoadMap();
    }
}

public partial class Level 
{
    public string Name { get; set; }
    public string Music { get; set; }
    public float MusicPitch { get; set; }
    public bool Locked { get; set; }
    public bool Completed { get; set; }
    public Vector2 Position { get; set; }
    public List<string> Unlocks { get; set; }

    public Level(string name)
    {
        Name = name;
        Unlocks = new();
        MusicPitch = 1.0f;

        var levelId = name.Split(" ")[1];
        var letter = levelId.Substring(0, 1);
        var num = int.Parse(levelId.Substring(1));
        
        num += 1;

        Unlocks.Add($"Level {letter}{num}"); // if this is Level A1, then this adds a unlock for Level A2
    }
}
