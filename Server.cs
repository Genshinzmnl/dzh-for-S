using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

public class GameServer
{
    private TcpListener? _listener;
    private List<Player> _players;
    private List<Thread> _threads;
    private int _port;
    private int _maxThreads;
    private int _currentThreads;
    private string _configPath = "Config/config.json";

    public GameServer(int port)
    {
        _port = port;
        _players = new List<Player>();
        _threads = new List<Thread>();
        LoadConfig();
    }

    private void LoadConfig()
    {
        if (File.Exists(_configPath))
        {
            var config = JsonConvert.DeserializeObject<Dictionary<string, int>>(File.ReadAllText(_configPath));
            _maxThreads = config.ContainsKey("maxThreads") ? config["maxThreads"] : 4;
        }
        else
        {
            _maxThreads = 4;
            SaveConfig();
        }
    }

    private void SaveConfig()
    {
        var config = new Dictionary<string, int> { { "maxThreads", _maxThreads } };
        if (!Directory.Exists("Config"))
        {
            Directory.CreateDirectory("Config");
        }
        File.WriteAllText(_configPath, JsonConvert.SerializeObject(config));
    }

    public void Start()
    {
        _listener = new TcpListener(IPAddress.Any, _port);
        _listener.Start();
        Console.WriteLine("Server started on port " + _port);

        while (true)
        {
            if (_listener != null && _listener.Pending() && _currentThreads < _maxThreads)
            {
                var client = _listener.AcceptTcpClient();
                var thread = new Thread(HandleClient);
                thread.Start(client);
                _threads.Add(thread);
                _currentThreads++;
            }
        }
    }

    private void HandleClient(object? obj)
    {
        if (obj is TcpClient client)
        {
            var stream = client.GetStream();
            var reader = new StreamReader(stream, Encoding.UTF8);
            var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
            var player = new Player(Guid.NewGuid().ToString());

            _players.Add(player);
            Console.WriteLine("Player connected: " + player.Id);

            while (client.Connected)
            {
                try
                {
                    var data = reader.ReadLine();
                    if (data == null) break;

                    var message = JsonConvert.DeserializeObject<Dictionary<string, string>>(data) ?? new Dictionary<string, string>();
                    if (message != null)
                    {
                        ProcessMessage(player, message);
                    }
                    BroadcastGameState();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    break;
                }
            }

            _players.Remove(player);
            client.Close();
            _currentThreads--;
            Console.WriteLine("Player disconnected: " + player.Id);
        }
    }

    private void ProcessMessage(Player player, Dictionary<string, string> message)
    {
        switch (message?["type"])
        {
            case "position":
                player.Position = JsonConvert.DeserializeObject<Vector3>(message["data"]);
                break;
            case "health":
                player.Health = int.Parse(message["data"]);
                break;
            case "bullet":
                var bullet = JsonConvert.DeserializeObject<Bullet>(message["data"]);
                if (bullet != null)
                {
                    player.Bullets.Add(bullet);
                }
                break;
            case "explosion":
                var explosion = JsonConvert.DeserializeObject<Explosion>(message["data"]);
                if (explosion != null)
                {
                    explosion.ApplyDamage(_players);
                }
                break;
        }
    }
    }