using ChessLibrary.Enums;

namespace ChessApplication.DTO.Messages
{
    [DtoType(DtoType.StartGame)]
    public class DtoStartGame : IDtoMessage
    {
        public Player StartingSide { get; set; }

        public DtoType MessageType => DtoType.StartGame;

        public DtoStartGame() { }
        public DtoStartGame(Player startingSide)
        {
            StartingSide = startingSide;
        }
    }
}