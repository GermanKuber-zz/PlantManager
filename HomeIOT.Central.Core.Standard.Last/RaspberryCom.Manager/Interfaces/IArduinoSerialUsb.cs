using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Business;

namespace RaspberryCom.Interfaces
{
    public interface IArduinoConnection<out TPortContainer, out TPortAnalog, out TPortDigital> where TPortAnalog : IArduinoPortListAnalog
                                                                                               where TPortDigital : IArduinoPortListDigital
                                                                                               where TPortContainer : IPortsContainer<TPortAnalog, TPortDigital>
    {

        Task WriteCommand(string command);
        Task WriteCommand(string command, string pin, string value);
        Task WriteCommand(Comando commando);
        TPortContainer Ports { get; }
        Task Connect(Device device);
        Task<List<Device>> GetDevices();
    }
}