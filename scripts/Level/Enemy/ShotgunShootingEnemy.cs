using Godot;
using System;

public class ShotgunShootingEnemy : Enemy
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
        int[] angles = new int[5] { -30, -15, 0, 15, 30 };
        foreach (int angle in angles)
        {
            EnemyBullet bulletInstance = Bullet.Instance<EnemyBullet>();
            Vector2 direction = Position.DirectionTo(GetNode<Player>("../../Player").Position);
            direction = direction.Rotated((float)(Math.PI / 180) * angle);
            bulletInstance.Position = Position;
            bulletInstance.init(direction);
            GetParent<Node>().GetParent<Node>().AddChild(bulletInstance);


        }
    }
}
