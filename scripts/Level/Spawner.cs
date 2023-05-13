using Godot;
using System;

public class Spawner : Node
{
    [Signal]
    public delegate void UpdateCountOfWaves();
    [Export]
    private PackedScene MeelEnemy { get; set; }
    [Export]
    private PackedScene PistolShootingEnemy { get; set; }
    [Export]
    private PackedScene ShotgunShootingEnemy { get; set; }
    private Node Enemies { get; set; }
    public override void _Ready()
    {
        Enemies = GetNode<Node>("../Enemies");
    }
    public override void _Process(float delta)
    {
        if (Enemies.GetChildCount() == 0)
        {
            EmitSignal(nameof(UpdateCountOfWaves));
            SpawnWave();
        }
    }
    private void SpawnWave()
    {
        foreach (Position2D position in GetChildren())
        {
            uint i = GD.Randi() % 3;
            Enemy enemyInstance = new MeelEnemy();
            switch (i)
            {
                case 0:
                    enemyInstance = MeelEnemy.Instance<MeelEnemy>();
                    break;
                case 1:
                    enemyInstance = PistolShootingEnemy.Instance<PistolShootingEnemy>();
                    break;
                case 2:
                    enemyInstance = ShotgunShootingEnemy.Instance<ShotgunShootingEnemy>();
                    break;
            }
            enemyInstance.Position = position.Position;
            Enemies.AddChild(enemyInstance);
        }
    }
}
