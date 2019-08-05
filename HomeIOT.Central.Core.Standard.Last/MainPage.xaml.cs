using Core.Business;
using Emmellsoft.IoT.Rpi.SenseHat;
using Emmellsoft.IoT.Rpi.SenseHat.Fonts.SingleColor;
using RaspberryCom.Manager;
using RaspberryCom.Ports;
using RaspberryCom.Sync;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Gpio;
using Windows.Devices.SerialCommunication;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HomeIOT.Central.Core.Standard.Last
{
    public sealed partial class MainPage : Page
    {
        #region Privados

        private const string RaspberryName = "CentralIOT";
        private IotCentral _iotConfigurations = new IotCentral();
        private DispatcherTimer _timer;
        private int contador = 0;
        private ISenseHat _senseHat;
        private ISenseHatDisplay _display;
        private string _unitText;
        private bool _timerRun;

        private const string PIN_VENTILADOR = "13";
        private const string PIN_AGUA = "12";
        private const string PIN_LUZ = "11";
        private const string PIN_IS_BUZZER_ON = "10";
        private const string PIN_BUZZER = "53";
        private const string PIN_SUELO = "A15";
        private const string PIN_SENSOR_LUZ = "A14";

        private const int LIMIT_TEMPERATURE_VENTILADOR = 38;
        private const int LIMIT_TEMPERATURE_BUZZER = 43;
        private const int LIMITE_HUMEDAD_SUELO = 400;

        #endregion

        #region ArduinoManager

        private readonly ArduinosManager _arduinosManager = new ArduinosManager();

        #endregion

        #region Constructor

        public MainPage()
        {
            this.InitializeComponent();
            ConnectSync();
            this.InicializarArduino();

        }

        private void ConnectSync()
        {
            UpdateConfiguracion();
        }

        #endregion

        private void LoadConfiguracion(IotCentral iotConfigurations)
        {
            this._iotConfigurations = iotConfigurations;
        }

        #region Subscripcion a eventos de Server




        private void EnviarComando(Comando comando)
        {
            this._arduinosManager.ArduinoMega2560.WriteCommand(comando);

        }



        private void UpdateConfiguracion()
        {


            this._arduinosManager.ArduinoMega2560.GetDevices().ContinueWith(async task =>
            {
                if (task.IsFaulted)
                {
                    throw new Exception("Error");
                }
                else
                {
                    var dispositivosConectadosARaspberry = task.Result;
                    var iotDevice = dispositivosConectadosARaspberry.FirstOrDefault(x => x.Identifier.Contains("USB#VID_23"));
                    iotDevice.Activo = true;
                    _iotConfigurations = new IotCentral();
                    _iotConfigurations.Devices.Add(iotDevice);

                    iotDevice.Puertos.Add(new Port
                    {
                        Pin = PIN_SENSOR_LUZ,
                        Accion = PortAccionEnum.READ,
                        Type = ComponentTypeEnum.Read
                    });
                    iotDevice.Puertos.Add(new Port
                    {
                        Pin = PIN_SUELO,
                        Accion = PortAccionEnum.READ,
                        Type = ComponentTypeEnum.Read
                    });
                    iotDevice.Puertos.Add(new Port
                    {
                        Pin = "A1",
                        Accion = PortAccionEnum.READ,
                        Type = ComponentTypeEnum.Read
                    });
                    if (iotDevice != null)
                    {
                        //Si el el dispositivo no esta ignorado y ademas activo
                        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                        {
                            await this._arduinosManager.ConnectAll(iotDevice);
                        });
                    }
                    this.StartTimer();


                }
            }).Wait();

        }



        #endregion

        #region Funciones Privadas


        private async Task InicializarTimer()
        {

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                                      () =>
                                      {
                                          if (this._timer == null)
                                          {
                                              this._timer = new DispatcherTimer();
                                              _timer.Interval = TimeSpan.FromSeconds(0.05);
                                              _timer.Tick += TimerOnTickAsync;
                                          }
                                      });
        }
        private double ConvertTemperatureValue(TemperatureUnit unit, double temperatureInCelcius)
        {
            switch (unit)
            {
                case TemperatureUnit.Celcius:
                    return temperatureInCelcius;

                case TemperatureUnit.Fahrenheit:
                    return temperatureInCelcius * 9 / 5 + 32;

                case TemperatureUnit.Kelvin:
                    return temperatureInCelcius + 273.15;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private string GetUnitText(TemperatureUnit unit)
        {
            switch (unit)
            {
                case TemperatureUnit.Celcius:
                    return "\u00B0C"; // Where "\u00B0" is the degree-symbol.

                case TemperatureUnit.Fahrenheit:
                    return "\u00B0F"; // Where "\u00B0" is the degree-symbol.

                case TemperatureUnit.Kelvin:
                    return "K";

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private readonly ManualResetEventSlim _waitEvent = new ManualResetEventSlim(false);
        protected void Sleep(TimeSpan duration)
        {
            _waitEvent.Wait(duration);
        }
        private void TimerOnTickAsync(object sender, object o)
        {
            this.LecturaContinua();
            if (_senseHat != null)
            {
                TemperaturaChecker();
                JostickChecker();
            }
        }

        private JostickAction Buzzer = new JostickAction();
        private JostickAction Light = new JostickAction();
        private void JostickChecker()
        {

        }
        private void TemperaturaChecker()
        {
            if (_senseHat.Joystick.Update()) // Has any of the buttons on the joystick changed?
            {
                if (_senseHat.Joystick.LeftKey == KeyState.Released)
                {
                    Buzzer.Change();
                    if (Buzzer.IsOn)
                        Task.Run(() => this._arduinosManager.ArduinoMega2560.WriteCommand("W", PIN_IS_BUZZER_ON, "0")).Wait();
                    else
                        Task.Run(() => this._arduinosManager.ArduinoMega2560.WriteCommand("W", PIN_IS_BUZZER_ON, "1")).Wait();

                }
                if (_senseHat.Joystick.DownKey == KeyState.Pressed)
                    Light.Change();
            }
            _senseHat.Sensors.HumiditySensor.Update();

            if (_senseHat.Sensors.Temperature.HasValue)
            {
                _display.Clear();
                var temperature = Math.Round(_senseHat.Sensors.Temperature.Value, 2);

                var color = Color.FromArgb(255, 255, 255, 255);
                if (temperature > LIMIT_TEMPERATURE_VENTILADOR)
                    color = Color.FromArgb(255, 30, 60, 255);

                if (temperature > LIMIT_TEMPERATURE_BUZZER)
                    color = Color.FromArgb(255, 255, 0, 0);

                new TinyFont().Write(_display, ((int)temperature).ToString(), color);
                _display.Update();

                _senseHat.Sensors.HumiditySensor.Update();

                if (_senseHat.Sensors.Temperature.HasValue)
                {
                    txtTemperatura.Text = temperature.ToString();
                    if (temperature < LIMIT_TEMPERATURE_VENTILADOR)
                    {
                        Task.Run(() => this._arduinosManager.ArduinoMega2560.WriteCommand("W", PIN_BUZZER, "1")).Wait();
                        Task.Run(() => this._arduinosManager.ArduinoMega2560.WriteCommand("W", PIN_VENTILADOR, "0")).Wait();
                        txtVentilador.Text = "Apagado";
                        txtAlarma.Text = "Apagado";
                    }
                    else
                    {
                        if (temperature > LIMIT_TEMPERATURE_BUZZER)
                        {
                            txtAlarma.Text = "Prendido";
                            Task.Run(() => this._arduinosManager.ArduinoMega2560.WriteCommand("W", PIN_BUZZER, "0")).Wait();
                        }
                        else
                        {
                            txtAlarma.Text = "Apagado";
                            Task.Run(() => this._arduinosManager.ArduinoMega2560.WriteCommand("W", PIN_BUZZER, "1")).Wait();
                        }
                        Task.Run(() => this._arduinosManager.ArduinoMega2560.WriteCommand("W", PIN_VENTILADOR, "1")).Wait();
                        txtVentilador.Text = "Prendido";


                    }
                    txtTemperatura.Text = temperature.ToString();

                }
                else
                {
                    Sleep(TimeSpan.FromSeconds(0.5));
                }
            }

        }
        private async void StartTimer()
        {

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                                  async () =>
                                  {
                                      await InicializarTimer();
                                      if (!_timer.IsEnabled)
                                      {
                                          _timer.Start();
                                          this._timerRun = true;
                                      }
                                  });

            _senseHat = await SenseHatFactory.GetSenseHat().ConfigureAwait(false);
            _senseHat.Display.Clear();
            _senseHat.Display.Update();
            _display = _senseHat.Display;
            _unitText = GetUnitText(TemperatureUnit.Celcius);

        }

        private async void StopTimer()
        {

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                                        () =>
                                        {
                                            if (_timer.IsEnabled)
                                            {
                                                _timer.Stop();
                                                this._timerRun = false;
                                            }
                                        });



        }

        private async void LecturaContinua()
        {
            if (_iotConfigurations?.Devices != null)
            {
                foreach (var devices in _iotConfigurations.Devices)
                {
                    if (devices.Activo)
                    {
                        //Si el dispositivo,  no es para solo utilizar los arduino y no cualquier otro dispositivo conectado a USB
                        if (devices.Puertos != null)
                        {
                            foreach (var puerto in devices.Puertos)
                            {

                                EjecutarComandoContraPuerto(puerto);


                            }
                        }
                    }
                }
            }
        }


        private async void EjecutarComandoContraPuerto(Port puerto)
        {
            if (puerto.Type == ComponentTypeEnum.Read)
            {

                await this._arduinosManager.ArduinoMega2560.WriteCommand("R", puerto.Pin, "");
                Debug.WriteLine(contador + " - WRITE : Puerto : " + puerto.Pin);
                ++contador;
            }
            else
            {
                //si el puerto es de componente on Off no hago nada, ya que estos los contemplo, cuando leo datos del puerto serie
                //y reaccion en cuanto a ellos.
            }

        }

        private void InicializarArduino()
        {
            this._arduinosManager.ArduinoMega2560.Ports.ActiveProcessRead(this._iotConfigurations);

            this._arduinosManager.ArduinoMega2560.Ports.NotificateAllPortRead((async data =>
            {
                await ManagerLogicAsync(data);
            }));
        }

        private async Task ManagerLogicAsync(ArduinoDataRead dataRead)
        {
            if (dataRead.Pin == PIN_SENSOR_LUZ)
            {
                txtSensorLuz.Text = dataRead.Value;
                if (int.Parse(dataRead.Value) > LIMITE_SENSOR_LUZ)
                {
                    await this._arduinosManager.ArduinoMega2560.WriteCommand("W", PIN_LUZ, "1");
                    txtLedRojo.Text = "Prendido";
                }
                else
                {
                    await this._arduinosManager.ArduinoMega2560.WriteCommand("W", PIN_LUZ, "0");
                    txtLedRojo.Text = "Apagado";
                }
            }
            else if (dataRead.Pin == PIN_SUELO)
            {
                txtHumedad.Text = dataRead.Value;
                if (int.Parse(dataRead.Value) < LIMITE_HUMEDAD_SUELO)
                {
                    await this._arduinosManager.ArduinoMega2560.WriteCommand("W", PIN_AGUA, "0");
                    txtBomba.Text = "Apagado";
                }
                else
                {
                    await this._arduinosManager.ArduinoMega2560.WriteCommand("W", PIN_AGUA, "1");
                    txtBomba.Text = "Prendido";

                }
            }
            else if (dataRead.Pin == "A1")
            {
                if (int.Parse(dataRead.Value) < 200)
                {
                    await this._arduinosManager.ArduinoMega2560.WriteCommand("W", "7", "1");
                }
                else
                {
                    await this._arduinosManager.ArduinoMega2560.WriteCommand("W", "7", "0");
                }
            }
        }

        private const int LIMITE_SENSOR_LUZ = 600;
        #endregion

    }
}
