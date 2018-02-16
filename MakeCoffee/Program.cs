using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.Samples.CoffeeMaker;

namespace Ploeh.Samples.MakeCoffee
{
    class Program
    {
        static void Main(string[] args)
        {
            var hardware = new CoffeMakerInMemory();
            hardware.SetReliefValveState(ReliefValveState.CLOSED);
            hardware.WarmerPlateStatus = WarmerPlateStatus.POT_EMPTY;

            var buttonEvents = Observable
                .Interval(TimeSpan.FromSeconds(1))
                .Select(_ => hardware.GetBrewButtonStatus())
                .Publish();
            var boilerEvents = Observable
                .Interval(TimeSpan.FromSeconds(1))
                .Select(_ => hardware.GetBoilerStatus())
                .Publish();
            var warmerEvents = Observable
                .Interval(TimeSpan.FromSeconds(1))
                .Select(_ => hardware.GetWarmerPlateStatus())
                .Publish();

            var boiler = new Boiler(hardware);
            var indicator = new Indicator(hardware);
            var valve = new ReliefValve(hardware);
            var warmer = new WarmerPlate(hardware);

            using (buttonEvents.Subscribe(boiler))
            using (buttonEvents.Subscribe(indicator))
            using (boilerEvents.Subscribe(boiler))
            using (boilerEvents.Subscribe(indicator))
            using (warmerEvents.Subscribe(valve))
            using (warmerEvents.Subscribe(warmer))
            {
                buttonEvents.Connect();
                boilerEvents.Connect();
                warmerEvents.Connect();

                while (!Exit(hardware))
                    WriteHardwareState(hardware);
            }
        }

        private static bool Exit(CoffeMakerInMemory hardware)
        {
            Console.Write("> ");
            var command = Console.ReadLine().ToUpperInvariant();
            switch (command)
            {
                case "POLL":
                    return false;
                case "FILL":
                    hardware.BoilerStatus = BoilerStatus.NOT_EMPTY;
                    return false;
                case "EMPTY":
                    hardware.BoilerStatus = BoilerStatus.EMPTY;
                    return false;
                case "PRESS":
                    hardware.BrewButtonStatus = BrewButtonStatus.PUSHED;
                    return false;
                case "DRIP":
                    hardware.WarmerPlateStatus = WarmerPlateStatus.POT_NOT_EMPTY;
                    return false;
                case "DRY":
                    hardware.WarmerPlateStatus = WarmerPlateStatus.POT_EMPTY;
                    return false;
                case "TAKE":
                    hardware.WarmerPlateStatus = WarmerPlateStatus.WARMER_EMPTY;
                    return false;
                case "EXIT":
                case "QUIT":
                    return true;
                case "HELP":
                default:
                    Console.WriteLine("Options: poll, fill, empty, press, drip, dry, take, exit, quit, help.");
                    return false;
            }
        }

        private static void WriteHardwareState(CoffeMakerInMemory hardware)
        {
            Console.WriteLine("Coffe maker status {0:T}:", DateTime.Now);

            Console.Write("Ready indicator:\t");
            Console.ForegroundColor =
                hardware.IndicatorState == IndicatorState.ON ?
                ConsoleColor.Green :
                ConsoleColor.Red;
            Console.Write(hardware.IndicatorState);
            Console.ResetColor();
            Console.WriteLine();

            Console.Write("Boiler:\t\t\t");
            Console.ForegroundColor =
                hardware.BoilerStatus == BoilerStatus.NOT_EMPTY ?
                ConsoleColor.Green :
                ConsoleColor.Red;
            Console.Write(hardware.BoilerStatus);
            Console.ResetColor();
            Console.WriteLine();

            Console.Write("Boiler:\t\t\t");
            Console.ForegroundColor =
                hardware.BoilerState == BoilerState.ON ?
                ConsoleColor.Green :
                ConsoleColor.Red;
            Console.Write(hardware.BoilerState);
            Console.ResetColor();
            Console.WriteLine();

            Console.Write("Warmer plate:\t\t");
            switch (hardware.WarmerPlateStatus)
            {
                case WarmerPlateStatus.WARMER_EMPTY:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case WarmerPlateStatus.POT_EMPTY:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case WarmerPlateStatus.POT_NOT_EMPTY:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                default:
                    break;
            }
            Console.Write(hardware.WarmerPlateStatus);
            Console.ResetColor();
            Console.WriteLine();

            Console.Write("Warmer plate:\t\t");
            Console.ForegroundColor =
                hardware.WarmerState == WarmerState.ON ?
                ConsoleColor.Green :
                ConsoleColor.Red;
            Console.Write(hardware.WarmerState);
            Console.ResetColor();
            Console.WriteLine();

            Console.Write("Relief valve:\t\t");
            Console.ForegroundColor =
                hardware.ReliefValveState == ReliefValveState.CLOSED ?
                ConsoleColor.Green :
                ConsoleColor.Red;
            Console.Write(hardware.ReliefValveState);
            Console.ResetColor();
            Console.WriteLine();

            Console.WriteLine();
        }
    }
}
