public class Vector3
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }

    public Vector3() { }

    public Vector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static float Distance(Vector3 a, Vector3 b)
    {
        return (float)Math.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) + (a.z - b.z) * (a.z - b.z));
    }
}