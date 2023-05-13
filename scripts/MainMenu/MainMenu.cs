using Godot;
using System;

public class MainMenu : CanvasLayer
{
    [Signal]
    public delegate void Start();
    [Signal]
    public delegate void HighScores();
    public void OnStartPressed()
    {
        EmitSignal("Start");
    }
    public void OnHighScoresPressed()
    {
        EmitSignal("HighScores");
    }
}
