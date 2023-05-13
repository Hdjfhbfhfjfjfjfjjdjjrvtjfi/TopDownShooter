using Godot;
using System;

public abstract class Enemy : KinematicBody2D
{
    [Signal]
    public delegate void Hit(int damage);
    [Export]
    protected int Speed { get; set; }
    [Export]
    protected int Health { get; set; }
    [Export]
    protected int Damage { get; set; }
    protected Map Navigation { get; set; }
    public override void _Ready()
    {
        Navigation = GetNode<Map>("../../Map");
    }
    public override void _PhysicsProcess(float delta)
    {
        Vector2 velocity = GetVelocity(delta);
        KinematicCollision2D collision = MoveAndCollide(velocity);
        velocity = Collides(velocity, collision);
        Position += velocity;
    }

    private Vector2 GetVelocity(float delta)
    {
        Vector2[] path = Navigation.FindPath(Position, GetNode<Player>("../../Player").Position);
        Vector2 velocity = path.Length != 0 ? new Vector2(path[1][0], path[1][1]) - Position : new Vector2(0, 0);
        velocity = velocity.Normalized() * delta * Speed;
        return velocity;
    }
    protected abstract Vector2 Collides(Vector2 velocity, KinematicCollision2D collision);


    public void OnHit(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            QueueFree();
        }
    }
}
