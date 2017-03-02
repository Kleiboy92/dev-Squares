using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace dev_Squares.Backend
{
    public class ParsingInfo
    {
        public readonly int Row;
        public readonly string Message;
        [JsonConverter(typeof(StringEnumConverter))]
        public readonly ParserType Type;
        public ParsingInfo(int row, string message, ParserType type)
        {
            this.Message = message;
            this.Row = row;
            this.Type = type;
        }
    }

    public enum ParserType
    {
        danger,
        success
    }
}