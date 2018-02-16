using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.CoffeeMaker
{
    public class Boiler : IObserver<BrewButtonStatus>, IObserver<BoilerStatus>
    {
        private readonly ICoffeeMaker hardware;
        private bool hasWater;

        public Boiler(ICoffeeMaker hardware)
        {
            if (hardware == null)
                throw new ArgumentNullException("hardware");

            this.hardware = hardware;
        }

        public void OnNext(BrewButtonStatus value)
        {
            if (this.hasWater && value == BrewButtonStatus.PUSHED)                
                this.hardware.SetBoilerState(BoilerState.ON);
        }

        public void OnNext(BoilerStatus value)
        {
            this.hasWater = value == BoilerStatus.NOT_EMPTY;
            if (!this.hasWater)
                this.hardware.SetBoilerState(BoilerState.OFF);
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }
    }
}
