using System.Text;
using RaspberryCom.Enum;

namespace RaspberryCom.Ports
{
    public abstract class ArduinoDigitalPortBase : ArduinoPortBase
    {
        protected string GenerateCommand(CommandTypeEnum commandType, DigitalPortEnum port, int value)
        {
            StringBuilder generateCommand = new StringBuilder();
            generateCommand.Append("C=");
            switch (commandType)
            {
                case CommandTypeEnum.READ:
                    generateCommand.Append("R");
                    break;
                case CommandTypeEnum.WRITE:
                    generateCommand.Append("W");
                    break;
            }


            generateCommand.Append(",");
            generateCommand.Append("P=");
            generateCommand.Append((int)port);
            generateCommand.Append(",");
            generateCommand.Append("V=");
            generateCommand.Append(value.ToString());
            generateCommand.Append("|");

            return generateCommand.ToString();
        }
     
    }
}