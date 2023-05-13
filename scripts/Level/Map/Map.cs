using Godot;
using System;

public class Map : Node2D
{
    private Navigation2D NavigationNode { get; set; }
    public override void _Ready()
    {
        NavigationNode = GetNode<Navigation2D>("EnvironmentMap/CollisionMap/Navigation");
    }
    public Vector2[] FindPath(Vector2 start, Vector2 goal)
    {
        Vector2[] Path = NavigationNode.GetSimplePath(start, goal);
        return Path;
    }
}
