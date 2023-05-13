using Godot;

public class Main : Node
{
    [Export]
    private PackedScene LevelScene { get; set; }
    [Export]
    private PackedScene HighScoresScene { get; set; }
    private const string HighScoresDataPath = "res://highscores.save";
    private CanvasLayer Canvas { get; set; }
    public override void _Ready()
    {
        Canvas = GetNode<CanvasLayer>("MainMenu");
    }
    public void HighScoresScreen()
    {
        HighScores HighScoresInstance = HighScoresScene.Instance<HighScores>();
        HighScoresInstance.Connect(nameof(HighScores.ExitFromHighScoresScreen), this, nameof(OnExitHighScoreScreen));
        HighScoresInstance.Name = "HighScores";
        AddChild(HighScoresInstance);
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
        File file = new File();
        file.Open(HighScoresDataPath, File.ModeFlags.Read);
        int[] scores = (int[])file.GetVar();
        for (int i = 0; i < scores.Length; i++)
        {
            if (CountOfWaves > scores[i])
            {
                scores[i] = CountOfWaves;
                break;
            }
        }
        file.Open(HighScoresDataPath, File.ModeFlags.Write);
        file.StoreVar(scores);
        file.Close();
        GetNode<Level>("Level").QueueFree();
        Canvas.Show();
    }
}
