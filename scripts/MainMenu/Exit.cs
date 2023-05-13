using Godot;
using System;

public class Exit : Button
{
    public void OnExitButtonDown()
    {
        GetTree().Quit();
    }
}
