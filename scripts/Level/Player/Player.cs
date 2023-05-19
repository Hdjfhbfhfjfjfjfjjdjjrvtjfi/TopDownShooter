using Godot;

public class Player : KinematicBody2D
{
    [Signal]
    public delegate void Hit(int damage);
    [Signal]
    public delegate void Killed();
    [Export]
    private PackedScene Bullet { get; set;  }
    [Export]
    private float Speed { get; set; }
    [Export]
    public int Health { get; private set; }
    private State state { get; set; }
    private AnimatedSprite Animation { get; set; }
    private Vector2 OriginDirection { get; set; }
    private Vector2 Direction
    {
        get
        {  
            return Position.DirectionTo(GetViewport().GetMousePosition()).Normalized();
        }
    }
    public override void _Ready()
    {
        state = State.Stay;
        Animation = GetNode<AnimatedSprite>("AnimatedSprite");
        OriginDirection = new Vector2(1, 0).Normalized();
    }
    public override void _PhysicsProcess(float delta)
    {
        Vector2 Velocity = GetVelocity() * delta;
        if (Velocity.Length() == 0 && state == State.Move)
        {
            state = State.Stay;
            Animation.Animation = "Stay";
            Animation.Play();
        }
        if (Velocity.Length() != 0 && state == State.Stay)
        {
            state = State.Move;
            Animation.Animation = "Walk";
            Animation.Play();
        }
        Velocity = Collide(Velocity);
        switch (state)
        {
            case State.Stay:
                break;
            case State.Move:
                Rotate(Velocity);
                break;
            case State.PrepareWeapon:
            case State.RemoveWeapon:
            case State.ReadyToShoot:
            case State.Shooting:
                Rotate();
                break;
        }
        Position += Velocity;
    }
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if ((@event is InputEventMouseButton mouseInput))
        {
            if ((ButtonList)mouseInput.ButtonIndex == ButtonList.Right)
            {
                if (mouseInput.IsPressed())
                {
                    state = State.PrepareWeapon;
                    Animation.Animation = "PrepareWeapon";
                    Animation.Play();
                }
                else
                {
                    state = State.RemoveWeapon;
                    Animation.Animation = "RemoveWeapon";
                    Animation.Play();
                }
            }
            if ((ButtonList)mouseInput.ButtonIndex == ButtonList.Left)
            {
                if (state == State.ReadyToShoot)
                {
                    Shoot();
                }
            }
        }
    }
    public void OnAnimatedSpriteAnimationFinished()
    {
        switch (Animation.Animation)
        {
            case "PrepareWeapon":
                state = State.ReadyToShoot;
                Animation.Animation = "ReadyToShoot";
                break;
            case "RemoveWeapon":
                state = State.Move;
                Animation.Animation = "Walk";
                Animation.Play();
                break;
            case "Shoot":
                state = State.ReadyToShoot;
                Animation.Animation = "ReadyToShoot";
                Animation.Play();
                break;
        }
    }
    public void OnHit(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            EmitSignal("Killed");
        }
    }
    private Vector2 GetVelocity()
    {
        Vector2 Velocity = new Vector2(0, 0);
        if (Input.IsActionPressed("up"))
        {
            Velocity.y -= 1;
        }
        if (Input.IsActionPressed("down"))
        {
            Velocity.y += 1;
        }
        if (Input.IsActionPressed("right"))
        {
            Velocity.x += 1;
        }
        if (Input.IsActionPressed("left"))
        {
            Velocity.x -= 1;
        }
        return Velocity.Normalized() * Speed;
    }
    private Vector2 Collide(Vector2 Velocity)
    {
        KinematicCollision2D collision = MoveAndCollide(Velocity);
        Velocity = collision != null ? Velocity - collision.Normal : Velocity;
        return Velocity;
    }
    private void Rotate()
    {
        Rotation = OriginDirection.AngleTo(Direction);
    }
    private void Rotate(Vector2 Velocity)
    {
        Rotation = OriginDirection.AngleTo(Velocity);
    }
    private void Shoot()
    {
        state = State.Shooting;
        Animation.Animation = "Shoot";
        Animation.Play();
        PlayerBullet bulletInstance = Bullet.Instance<PlayerBullet>();
        bulletInstance.Position = Position + Direction * 8;
        bulletInstance.init(Direction);
        GetParent<Node>().AddChild(bulletInstance);
        
    }
}
