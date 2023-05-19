using Godot;
using System;

public class AbstractBullet : KinematicBody2D
{
    [Export]
    public int Speed { get; set;  }
    [Export]
    public int Damage { get; set;  }
    protected Vector2 Direction { get; set; }
    public virtual void init(Vector2 direction)
    {
        Direction = direction.Normalized();
        Rotation = (new Vector2(1, 0)).AngleTo(Direction);
    }
    public override void _PhysicsProcess(float delta)
    {
        Move(delta);
    }

    protected void Move(float delta)
    {
        Vector2 velocity = Direction * Speed * delta;
        KinematicCollision2D collision = MoveAndCollide(velocity);
        if (collision != null)
        {
            QueueFree();
        }
        else
        {
            Position += velocity;
        }
    }
    
}
