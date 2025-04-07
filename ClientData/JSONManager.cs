using System.Text.Json;

namespace ClientData;

public class JSONManager
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };
    
    public class StringMessage<T>
    {
        public string Type { get; set; } = "";
        public T Message { get; set; } = default!;
    }
    
    public static StringMessage<JsonElement>? DeserializeRawMessage(string json)
    {
        return JsonSerializer.Deserialize<StringMessage<JsonElement>>(json, _options);
    }

    public static T? DeserializePayload<T>(JsonElement messageElement)
    {
        return messageElement.Deserialize<T>(_options);
    }
    
    public static string Serialize<T>(string type, T message)
    {
        var msg = new StringMessage<T>
        {
            Type = type,
            Message = message
        };

        return JsonSerializer.Serialize(msg, _options);
    }
}
