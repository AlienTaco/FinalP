using System.Text;
using WatsonTcp;

namespace Server
{
    public class Server : WatsonTcpServer
    {
        private readonly Dictionary<string, string> _clientIpNames = new Dictionary<string, string>();
      //  private GameState _gameState = new GameState();
        public Server(string ip, int port) : base(ip, port)
        {
            Console.WriteLine($"My address: {ip}:{port}");
            Console.Title = $"Server {ip}:{port}";   // שם חלון הקונסולה

            Events.ClientConnected += ClientConnected;
            Events.ClientDisconnected += ClientDisconnected;
            Events.MessageReceived += MessageReceived;

            Start();                                 // המשתמשים בפעולה מוכנה

           
        }

        private void ClientConnected(object sender, ConnectionEventArgs e)
        {
            Console.WriteLine($"[{e.Client.IpPort}] client connected");
        }

        private void ClientDisconnected(object sender, DisconnectionEventArgs e)
        {
            _clientIpNames.Remove(e.Client.IpPort);  // מסירים את הלקוח מהרשימה
            Console.WriteLine($"[{e.Client.IpPort}] client disconnected: {e.Reason}");
        }

        private async void MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var ipPort = e.Client.IpPort;            // הנתונים של הלקוח
            var message = Encoding.UTF8.GetString(e.Data);
            Console.WriteLine($"[{ipPort}]: {message}");
            await ProcessMessageAsync(ipPort, message);
        }
        private async Task ProcessMessageAsync(string clientIp, string message)
        {
            var sendMessage = string.Empty;
            var parts = message.Split('|');   // split received message into two parts

            switch (parts[0])
            {
                case "Register":
                    _clientIpNames[clientIp] = parts[1];   // add client to list
                    sendMessage = $"Register|[{parts[1]}] registered";   // preparing a message
                    await SendMessage(sendMessage);
                    break;

                //case "Chat":
                //    sendMessage = parts[1];   // preparing a message: message itself
                //    await SendMessage(sendMessage);
                //    break;

                //case "StartGame":
                //    await StartGame(parts[1]);
                //    break;

                //case "AddPlayer":
                //    await AddPlayer(parts[1]);
                //    break;

                //case "GameStep":
                //    await GameStep(parts[1]);
                //    break;
            }
        }
        private async Task SendMessage(string message)
        {
            // send message to all clients:
            foreach (var client in ListClients())
            {
                await SendAsync(client.Guid, $"{message}");
            }
        }
        //private async Task GameStep(string jsonGameStep)
        //{
        //    var gameStep = JsonSerializer.Deserialize<GameStep>(jsonGameStep);

        //    var player = _gameState.Players.First(p => p.ClientName == gameStep.ClientName);   // find the player
        //    player.IsActive = false;                                                           // make it inactive
        //    var otherPlayer = _gameState.Players.First(p => p.ClientName != gameStep.ClientName); // find the other player
        //    otherPlayer.IsActive = true;                                                      // make it active

        //    _gameState.Board.Cells[gameStep.FromRow * 10 + gameStep.FromColumn].CharacterName = string.Empty;
        //    _gameState.Board.Cells[gameStep.ToRow * 10 + gameStep.ToColumn].CharacterName = player.CharacterName;

        //    await UpdateGameState();
        //}

        //private async Task AddPlayer(string jsonPlayer)
        //{
        //    var player = JsonSerializer.Deserialize<Player>(jsonPlayer);   // second player
        //    _gameState.Players.Add(player);

        //    for (int i = 99, count = 0; count < 10; i--)                   // Add characters of second player
        //    {
        //        if (_gameState.Board.Cells[i].Color == "B")
        //        {
        //            _gameState.Board.Cells[i].CharacterName = player.CharacterName;
        //            count++;
        //        }
        //    }

        //    player.IsActive = false;                                       // the second player becomes inactive
        //    var firstPlayer = _gameState.Players.First(p => p.ClientName != player.ClientName);
        //    firstPlayer.IsActive = true;                                   // the first player becomes active
        //    await UpdateGameState();
        //}
        //private async Task StartGame(string jsonPlayer)
        //{
        //    var player = JsonSerializer.Deserialize<Player>(jsonPlayer);   // first player

        //    _gameState = new GameState();
        //    _gameState.Players.Add(player);

        //    for (int i = 0; i < 100; i++)                                  // build board
        //    {
        //        _gameState.Board.Cells.Add(new Cell
        //        {
        //            CharacterName = string.Empty,
        //            Color = (i % 10 + i / 10) % 2 == 0 ? "W" : "B",
        //            Index = i,
        //        });
        //    }

        //    for (int i = 0, count = 0; count < 10; i++)                    // add characters of first player
        //    {
        //        if (_gameState.Board.Cells[i].Color == "B")
        //        {
        //            _gameState.Board.Cells[i].CharacterName = player.CharacterName;
        //            count++;
        //        }
        //    }

        //    await UpdateGameState();
        //}




        // private async Task UpdateGameState()
        //{
        //    string json = JsonSerializer.Serialize(_gameState);
        //    await SendMessage($"UpdateGameState|{json}");
        //}


    }
}
