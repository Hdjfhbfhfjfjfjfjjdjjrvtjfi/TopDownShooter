using Godot;
using System;

public class PlayerBullet : AbstractBullet
{
    public void OnArea2DAreaEntered(Area2D area)
    {
        if (area.GetParent() is Enemy)
        {
            area.GetParent<Enemy>().EmitSignal(nameof(Enemy.Hit), new object[1] { Damage });
            QueueFree();
        }
    }
}
