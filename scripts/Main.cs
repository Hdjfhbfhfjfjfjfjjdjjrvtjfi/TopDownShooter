using Godot;

public class Main : Node
{
    [Export]
    private PackedScene LevelScene { get; set; }
    [Export]
    private PackedScene HighScoresScene { get; set; }
    private const string HighScoresDataPath = "res://High_scores.save";
    private HTTPRequest HttpModule { get; set; }
    private CanvasLayer Canvas { get; set; }
    public override void _Ready()
    {
        File file = new File();
        if (!file.FileExists(HighScoresDataPath))
        {
            file.Open(HighScoresDataPath, File.ModeFlags.WriteRead);
            file.StoreVar(new int[5] { 0, 0, 0, 0, 0 });
        }
        file.Close();
        Canvas = GetNode<CanvasLayer>("MainMenu");
        HttpModule = GetNode<HTTPRequest>("HTTPRequest");
    }
    public void HighScoresScreen()
    {
        HttpModule.GetData();
    }
    public void OnHTTPRequestReturnData(int[] data)
    {
        HighScores HighScoresInstance = HighScoresScene.Instance<HighScores>();
        HighScoresInstance.Connect(nameof(HighScores.ExitFromHighScoresScreen), this, nameof(OnExitHighScoreScreen));
        HighScoresInstance.Name = "HighScores";
        HighScoresInstance.Hide();
        AddChild(HighScoresInstance);
        HighScoresInstance.init(data);
        HighScoresInstance.Show();
    }
    public void Start()
    {
        Level LevelInstance = LevelScene.Instance<Level>();
        LevelInstance.Name = "Level";
        AddChild(LevelInstance);
        LevelInstance.Connect(nameof(Level.PlayerKilled), this, nameof(OnPlayerKilled));
        Canvas.Hide();
    }
    public void OnExitHighScoreScreen()
    {
        GetNode<HighScores>("HighScores").QueueFree();
        Canvas.Show();
    }
    public void OnPlayerKilled(int CountOfWaves)
    {
        HttpModule.SetData(CountOfWaves);
        GetNode<Level>("Level").QueueFree();
        Canvas.Show();
    }
}
