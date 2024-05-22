public class Bullet
{
    public Vector3 Position { get; set; }
    public int Damage { get; set; }

    public Bullet(Vector3 position, int damage)
    {
        Position = position;
        Damage = damage;
    }
}