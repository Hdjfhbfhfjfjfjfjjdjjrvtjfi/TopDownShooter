using Godot;
using System;

public class PistolShootingEnemy : Enemy
{
    [Export]
    private PackedScene Bullet { get; set; }
    [Export]
    private int ShootingDistance { get; set; }
    private RayCast2D Ray { get; set; }
    private float ShootCooldown { get; set; }
    public override void _Ready()
    {
        base._Ready();
        Ray = GetNode<RayCast2D>("Ray");
        Ray.CastTo = new Vector2(0, 0);
    }

    protected override Vector2 Collides(Vector2 velocity, KinematicCollision2D collision)
    {
        if (ShootCooldown <= 0)
        {
            Shoot();
            ShootCooldown = 4f;
        }
        if (collision != null)
        {
            if (collision.Collider is PlayerBullet)
            {
                Position += collision.Normal;
            }
        }
        Ray.CastTo = Position.DirectionTo(GetNode<Player>("../../Player").Position) * Position.DistanceTo(GetNode<Player>("../../Player").Position);
        if (Ray.CastTo.Length() <= ShootingDistance)
        {
            Position -= velocity;
            velocity = new Vector2(0, 0);
        }
        Ray.CastTo = Vector2.Zero;
        ShootCooldown -= GetPhysicsProcessDeltaTime();
        return velocity;
    }
    private void Shoot()
    {
        EnemyBullet bulletInstance = Bullet.Instance<EnemyBullet>();
        bulletInstance.Position = Position;
        bulletInstance.init(Position.DirectionTo(GetNode<Player>("../../Player").Position));
        GetParent<Node>().AddChild(bulletInstance);
    }
}