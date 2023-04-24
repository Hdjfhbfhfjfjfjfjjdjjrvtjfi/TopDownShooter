using Godot;
using System;

public class MeelEnemy : KinematicBody2D, IEnemy
{
    [Signal]
    public delegate void Hit(int damage);
    [Export]
    private int Speed { get; set; }
    [Export]
    private int Health { get; set; }
    [Export]
    private int Damage { get; set; }
    private Map Navigation { get; set; }
    public override void _Ready()
    {
        Navigation = GetParent<Map>();
    }
    public override void _PhysicsProcess(float delta)
    {
        Vector2[] path = Navigation.FindPath(Position, GetNode<Player>("../Player").Position);
        Vector2 velocity = path.Length != 0 ? new Vector2(path[1][0], path[1][1]) - Position : new Vector2(0, 0);
        velocity = velocity.Normalized() * delta * Speed;
        KinematicCollision2D collision = MoveAndCollide(velocity);
        if (collision != null)
        {
            if (collision.Collider is Player)
            {
                MeelAttack((Player)collision.Collider);
                velocity = new Vector2(0, 0);
            }
            else
            {
                velocity = collision != null ? velocity - collision.Normal : velocity;
            }
        }
        Position += velocity;
    }
    public void OnHit(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            QueueFree();
        }
    }
    private void MeelAttack(Player player)
    {
        player.EmitSignal("Hit", new object[1] { Damage });
    }
}
