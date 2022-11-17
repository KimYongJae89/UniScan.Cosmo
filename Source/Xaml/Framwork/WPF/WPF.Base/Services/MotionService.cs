using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using UniEye.Base;
using WPF.Base.Helpers;

namespace WPF.Base.Services
{
    public static class MotionService
    {
        public static event EventHandler PositionChanged;

        static IEnumerable<AxisHandler> _handlers;
        public static IEnumerable<AxisHandler> Handlers { get => _handlers; }

        static DispatcherTimer _timer = new DispatcherTimer();

        static AxisPosition _axisPosition = new AxisPosition();
        public static AxisPosition AxisPosition => _axisPosition;

        static CancellationTokenSource cts = new CancellationTokenSource();

        public static void Initialize()
        {
            Application.Current.Dispatcher.Invoke(() => Application.Current.Exit += Current_Exit);

            _handlers = SystemManager.Instance().DeviceBox.AxisConfiguration.Where(f => f.HandlerType == AxisHandlerType.RobotStage);
            
            foreach (var handler in _handlers)
            {
                handler.TurnOnServo(true);
                _axisPosition = handler.GetActualPos();
            }

            CheckPosition();
        }

        private static void Current_Exit(object sender, ExitEventArgs e)
        {
            cts.Cancel();
        }

        private static void CheckPosition()
        {
            Task.Factory.StartNew(() =>
            {
                while (cts.IsCancellationRequested == false)
                {
                    bool changed = false;
                    foreach (var handler in _handlers)
                    {
                        var axisPosition = handler.GetActualPos();

                        for (int i = 0; i < axisPosition.Position.Count(); i++)
                        {
                            if (_axisPosition.Position[i] != axisPosition.Position[i])
                            {
                                _axisPosition.Position[i] = axisPosition.Position[i];
                                changed = true;
                            }
                        }
                    }

                    if (changed && PositionChanged != null)
                        PositionChanged(null, null);

                    Task.Delay(500);
                }
            }, TaskCreationOptions.LongRunning);
        }

        public static async Task<bool> WaitMoveDone(AxisHandler axisHandler, CancellationToken token)
        {
            return await Task.Run(async() =>
            {
                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        axisHandler.StopMove();
                        return false;
                    }

                    if (axisHandler.IsPositiveOn().Any(on => on) || axisHandler.IsNegativeOn().Any(on => on))
                    {
                        axisHandler.StopMove();
                        await Homming(token);
                        return false;
                    }

                    if (axisHandler.IsMoveOn().All(moveOn => moveOn == false))
                        return true;

                    await Task.Delay(100);
                }

                
            });
        }

        public static async Task<bool> WaitMoveDone(CancellationToken token)
        {
            return await Task.Run(async () =>
            {
                bool needWait = true;

                while (needWait)
                {
                    if (token.IsCancellationRequested)
                    {
                        foreach (var axisHandler in _handlers)
                            axisHandler.StopMove();

                        return false;
                    }

                    needWait = false;
                    foreach (var axisHandler in _handlers)
                    {
                        if (axisHandler.IsPositiveOn().Any(on => on) || axisHandler.IsNegativeOn().Any(on => on))
                        {
                            axisHandler.StopMove();
                            await Homming(token);
                            return false;
                        }

                        if (axisHandler.IsMoveOn().Any(moveOn => moveOn == true))
                            needWait = true;
                    }

                    await Task.Delay(100);
                }

                return true;

            });
        }

        public static async Task<bool> Homming(CancellationToken token)
        {
            return await Task.Run(async () =>
            {
                //bool needHomming = false;
                foreach (var axisHandler in _handlers)
                {
                    axisHandler.StartMultipleHomeMove(null);
                }

                await Task.Delay(1000);

                return await WaitMoveDone(token);
            });
        }
    }
}
