using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.Samples.CoffeeMaker;

namespace Ploeh.Samples.MakeCoffee
{
    public class CoffeMakerInMemory : ICoffeeMaker
    {
        public WarmerPlateStatus WarmerPlateStatus { get; set; }
        public WarmerPlateStatus GetWarmerPlateStatus()
        {
            return this.WarmerPlateStatus;
        }

        public BoilerStatus BoilerStatus { get; set; }
        public BoilerStatus GetBoilerStatus()
        {
            return this.BoilerStatus;
        }

        public BrewButtonStatus BrewButtonStatus { get; set; }
        public BrewButtonStatus GetBrewButtonStatus()
        {
            var returnValue = this.BrewButtonStatus;
            this.BrewButtonStatus = BrewButtonStatus.NOT_PUSHED;
            return returnValue;
        }

        public BoilerState BoilerState { get; set; }
        public void SetBoilerState(BoilerState boilerState)
        {
            this.BoilerState = boilerState;
        }

        public WarmerState WarmerState { get; set; }
        public void SetWarmerState(WarmerState warmerState)
        {
            this.WarmerState = warmerState;
        }

        public IndicatorState IndicatorState { get; set; }
        public void SetIndicatorState(IndicatorState indicatorState)
        {
            this.IndicatorState = indicatorState;
        }

        public ReliefValveState ReliefValveState { get; set; }
        public void SetReliefValveState(ReliefValveState reliefValveState)
        {
            this.ReliefValveState = reliefValveState;
        }
    }
}
