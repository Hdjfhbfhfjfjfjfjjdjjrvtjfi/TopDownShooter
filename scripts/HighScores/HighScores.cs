using Godot;

public class HighScores : CanvasLayer
{
    [Signal]
    public delegate void ExitFromHighScoresScreen();
    private const string HighScoresDataPath = "res://highscores.save";
    public override void _Ready()
    {
        File file = new File();
        file.Open(HighScoresDataPath, File.ModeFlags.ReadWrite);
        int[] scores = (int[]) file.GetVar();
        file.Close();
        foreach (Score label in GetNode("ScoreLabels").GetChildren())
        {
            label.Text += scores[label.Number].ToString();
        }
    }
    public void OnExitPressed()
    {
        EmitSignal(nameof(ExitFromHighScoresScreen));
    }
}
