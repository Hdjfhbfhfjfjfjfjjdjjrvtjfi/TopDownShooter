using Godot;
using System;

public class EnemyBullet : AbstractBullet
{
    public override void _PhysicsProcess(float delta)
    {
        Move<Player>(delta);
    }
}
