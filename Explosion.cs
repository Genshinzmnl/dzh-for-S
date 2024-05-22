public class Explosion
{
    public Vector3 Position { get; set; }
    public float Radius { get; set; }
    public int Damage { get; set; }

    public Explosion(Vector3 position, float radius, int damage)
    {
        Position = position;
        Radius = radius;
        Damage = damage;
    }

    public void ApplyDamage(List<Player> players)
    {
        foreach (var player in players)
        {
            if (Vector3.Distance(Position, player.Position) <= Radius)
            {
                player.Health -= Damage;
            }
        }
    }
}