using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.CoffeeMaker
{
    public class WarmerPlate : IObserver<WarmerPlateStatus>
    {
        private readonly ICoffeeMaker hardware;

        public WarmerPlate(ICoffeeMaker hardware)
        {
            if (hardware == null)
                throw new ArgumentNullException("hardware");

            this.hardware = hardware;
        }

        public void OnNext(WarmerPlateStatus value)
        {
            if (value == WarmerPlateStatus.POT_NOT_EMPTY)
                this.hardware.SetWarmerState(WarmerState.ON);
            else
                this.hardware.SetWarmerState(WarmerState.OFF);
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }
    }
}
