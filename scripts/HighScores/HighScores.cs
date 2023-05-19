using Godot;
using System.ComponentModel;
using System.Linq;

public class HighScores : CanvasLayer
{
    [Signal]
    public delegate void ExitFromHighScoresScreen();
    public void init(int[] data)
    {
        int[] scores = new int[data.Length];
        for (int i = 0; i < scores.Length; i++)
        {
            scores[i] = data[i];
        }

        foreach ( Score label in GetNode("ScoreLabels").GetChildren())
        {
            label.Text += scores[label.Number].ToString();
        }
    }
    public void OnExitPressed()
    {
        EmitSignal(nameof(ExitFromHighScoresScreen));
    }
}
