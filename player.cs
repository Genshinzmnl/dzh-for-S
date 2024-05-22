public class Player
{
    public string Id { get; set; }
    public Vector3 Position { get; set; }
    public int Health { get; set; }
    public List<Bullet> Bullets { get; set; }

    public Player(string id)
    {
        Id = id;
        Position = new Vector3();
        Health = 100;
        Bullets = new List<Bullet>();
    }
}