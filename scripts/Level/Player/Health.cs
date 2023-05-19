using Godot;

public class Health : Label
{
    private int health { get; set; }
    public override void _Ready()
    {
        health = GetNode<Player>("../Player").Health;
        Text = health.ToString();
    }
    public void OnPlayerHit(int damage)
    {
        health -= damage;
        Text = health.ToString();
    }
}
