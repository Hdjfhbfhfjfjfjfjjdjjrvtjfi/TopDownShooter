using Godot;
using System;

public class Level : Node2D
{
    [Signal]
    public delegate void PlayerKilled(int CountOfWaves);
    public void OnPlayerKilled()
    {
        EmitSignal("PlayerKilled", new object[1] { GetNode<Label>("Label").CountOfWaves });
    }
}
