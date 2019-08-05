using System.Text;
using RaspberryCom.Enum;

namespace RaspberryCom.Helpers
{
    public static class WriteCommandClass
    {
        public static string Generate(string command, string pin, string value)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("C=");
            stringBuilder.Append(command);
            stringBuilder.Append(",P=");
            stringBuilder.Append(pin);
            stringBuilder.Append(",V=");
            stringBuilder.Append(value);
            stringBuilder.Append("|");
            return stringBuilder.ToString();
            
        }
        public static string Generate(CommandTypeEnum command, string pin, string value)
        {
            var localCommand = "";
            if (command == CommandTypeEnum.READ)
            {
                localCommand = "R";
            }else if (command == CommandTypeEnum.WRITE)
            {
                localCommand = "W";
            }

            var stringBuilder = new StringBuilder();
            stringBuilder.Append("C=");
            stringBuilder.Append(localCommand);
            stringBuilder.Append(",P=");
            stringBuilder.Append(pin);
            stringBuilder.Append(",V=");
            stringBuilder.Append(value);
            stringBuilder.Append("|");
            return stringBuilder.ToString();
        }
    }
}
