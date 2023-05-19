using Godot;
using System;

public class MeelEnemy : Enemy
{
    private bool CanAttack { get; set; } = true;
    private Timer AttackCooldownTimer { get; set; }
    public override void _Ready()
    {
        base._Ready();
        AttackCooldownTimer = GetNode<Timer>("AttackCooldownTimer");
    }
    protected override Vector2 Collides(Vector2 velocity, KinematicCollision2D collision)
    {
        if (collision != null)
        {
            if (collision.Collider is Player && CanAttack)
            {
                MeelAttack((Player) collision.Collider);
                CanAttack = false;
                AttackCooldownTimer.Start();
            }
            velocity = velocity - collision.Normal;
            if (collision.Collider is PlayerBullet)
            {
                Position += collision.Normal;
            }
        }
        return velocity;
    }
    private void MeelAttack(Player player)
    {
        player.EmitSignal("Hit", new object[1] { Damage });
    }
    public void OnAttackCooldownTimerTimeout()
    {
        CanAttack = true;
    }
    
}
