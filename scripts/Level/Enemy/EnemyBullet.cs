using Godot;
using System;

public class EnemyBullet : AbstractBullet
{
    public void OnArea2DAreaEntered(Area2D area)
    {
        if (area.GetParent() is Player)
        {
            area.GetParent<Player>().EmitSignal(nameof(Player.Hit), new object[1] { Damage });
            QueueFree();
        }
    }
}
