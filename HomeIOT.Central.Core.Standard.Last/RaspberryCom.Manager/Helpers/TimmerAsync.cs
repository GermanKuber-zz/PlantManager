using System;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace RaspberryCom.Helpers
{
    public static class TimmerAsync
    {
        public delegate bool TimerAsyncFuction();
        public static void Start(TimerAsyncFuction execute,int timeDelay)
        {
            var condiccion = true;
            Task.Factory.StartNew(async () =>
            {
                while (condiccion)
                {
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Low,
                    () =>
                    {
                        condiccion = execute();
                    });
                    await Task.Delay(timeDelay);
                }
            });
        }
    }
}
