using System.Text;
using System.Text.Json;

namespace Carden.Api.Helpers;

public class SnakeCaseNamingPolicy: JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        var builder = new StringBuilder();
        foreach (var c in name)
        {
            if(char.IsUpper(c) && builder.Length > 0) builder.Append('_');
            builder.Append(char.ToLower(c));
        }
        return builder.ToString();      
    }
}