using System;

namespace dzh_for_S
{
    class Program
    {
        static void Main(string[] args)
        {
            GameServer S = new GameServer();
            
            S.Connect(26666);
            S.Start();
        }
    }
}
