namespace ChessApplication.DTO
{
    public class LobbyParams
    {
        public bool IsHost { get; private set; }
        public string IpAdress { get; private set; }

        public LobbyParams(bool isHost, string ipAdress = null)
        {
            IsHost = isHost;
            IpAdress = ipAdress;
        }
    }
}
