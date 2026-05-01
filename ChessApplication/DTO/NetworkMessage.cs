using System.Text.Json;

namespace ChessApplication.DTO
{
    public class NetworkMessage
    {
        public DtoType DtoType { get; set; }
        public JsonElement Payload { get; set; }
    }
}
