using ChessApplication.DTO;
using ChessApplication.Interfaces.Network;
using System.Reflection;
using System.Text.Json;

namespace ChessInfrastructure.Network
{
    public class DtoResolver : IDtoResolver
    {
        private readonly Dictionary<DtoType, Type> _map;

        public DtoResolver()
        {
            _map = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t =>
                    typeof(IDtoMessage).IsAssignableFrom(t) &&
                    t.GetCustomAttribute<DtoTypeAttribute>() != null)
                .ToDictionary(
                    t => t.GetCustomAttribute<DtoTypeAttribute>()!.Type,
                    t => t
                );
        }

        public IDtoMessage Deserialize(NetworkMessage message)
        {
            if (!_map.TryGetValue(message.DtoType, out var type))
                throw new Exception($"Unknown DTO type: {message.DtoType}");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var dto = JsonSerializer.Deserialize(message.Payload, type, options);

            return (IDtoMessage)dto!;
        }
    }
}