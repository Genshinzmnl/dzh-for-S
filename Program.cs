using System;

namespace dzh_for_S
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server(26666);
            server.Start();
        }
    }
}
