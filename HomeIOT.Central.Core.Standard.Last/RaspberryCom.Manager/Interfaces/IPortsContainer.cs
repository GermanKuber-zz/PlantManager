using RaspberryCom.Ports;

namespace RaspberryCom.Interfaces
{
    public interface IPortsContainer<out TPortAnalog, out TPortDigital> where TPortAnalog : IArduinoPortListAnalog 
                                                                        where TPortDigital : IArduinoPortListDigital
    {
        TPortAnalog Analog { get; }
        TPortDigital Digital { get; }

        void ReadDataPortArduino(ArduinoDataRead data);
        void NotificateAllPortRead(PortsMega2560Container.NotifyReadGeneralPort notifiFunctionGeneralPort);

        void CancelNotificateAllPortRead(PortsMega2560Container.NotifyReadGeneralPort notifiFunctionGeneralPort);

    }
}