using Godot;
using System;

public class Label : Godot.Label
{
    public int CountOfWaves { get; private set; } = -1;
    public void UpdateCountOfWaves()
    {
        CountOfWaves++;
        Text = "Count of waves: " + CountOfWaves.ToString();
    }
}
