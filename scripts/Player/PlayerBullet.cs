using Godot;
using System;

public class PlayerBullet : AbstractBullet
{

    public override void _PhysicsProcess(float delta)
    {
        Move<IEnemy>(delta);
    }
}
