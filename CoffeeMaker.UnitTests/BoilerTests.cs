using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;
using Ploeh.Samples.CoffeeMaker;
using AutoFixture.Xunit2;
using Moq;

namespace Ploeh.Samples.CoffeeMaker.UnitTests
{
    public class BoilerTests
    {
        [Theory, TestConventions]
        public void SutIsObserverOfBrewButtonStatus(Boiler sut)
        {
            Assert.IsAssignableFrom<IObserver<BrewButtonStatus>>(sut);
        }

        [Theory, TestConventions]
        public void OnCompletedDoesNotThrow(Boiler sut)
        {
            sut.OnCompleted();
        }

        [Theory, TestConventions]
        public void OnErrorDoesNotThrowNotImplementedException(
            Boiler sut,
            Exception e)
        {
            try
            {
                sut.OnError(e);
            }
            catch (NotImplementedException)
            {
                Assert.True(false, "NotImplementedException thrown.");
            }
        }

        [Theory, TestConventions]
        public void OnNexNotPushedBrewButtonDoesNotThrow(Boiler sut)
        {
            sut.OnNext(BrewButtonStatus.NOT_PUSHED);
        }

        [Theory, TestConventions]
        public void SutIsObserverOfBoilerStatus(Boiler sut)
        {
            Assert.IsAssignableFrom<IObserver<BoilerStatus>>(sut);
        }

        [Theory, TestConventions]
        public void OnNextBoilerStatusEmptyDoesNotThrow(Boiler sut)
        {
            sut.OnNext(BoilerStatus.EMPTY);
        }

        [Theory, TestConventions]
        public void FillBoilerAndPushButtonStartsBoiler(
            [Frozen]Mock<ICoffeeMaker> hardwareMock,
            Boiler sut)
        {
            sut.OnNext(BoilerStatus.NOT_EMPTY);
            sut.OnNext(BrewButtonStatus.PUSHED);

            hardwareMock.Verify(hw => hw.SetBoilerState(BoilerState.ON));
        }

        [Theory, TestConventions]
        public void FillBoilerWithoutPushingButtonDoesNotTurnOnBoiler(
            [Frozen]Mock<ICoffeeMaker> hardwareMock,
            Boiler sut)
        {
            sut.OnNext(BoilerStatus.NOT_EMPTY);

            hardwareMock.Verify(
                hw => hw.SetBoilerState(BoilerState.ON),
                Times.Never());
        }

        [Theory, TestConventions]
        public void PushButtonWithUnknownBoilerStatusDoesNotStartsBoiler(
            [Frozen]Mock<ICoffeeMaker> hardwareMock,
            Boiler sut)
        {
            sut.OnNext(BrewButtonStatus.PUSHED);

            hardwareMock.Verify(
                hw => hw.SetBoilerState(BoilerState.ON),
                Times.Never());
        }

        [Theory, TestConventions]
        public void PushButtonWithEmptyBoilerDoesNotStartsBoiler(
            [Frozen]Mock<ICoffeeMaker> hardwareMock,
            Boiler sut)
        {
            sut.OnNext(BoilerStatus.EMPTY);
            sut.OnNext(BrewButtonStatus.PUSHED);

            hardwareMock.Verify(
                hw => hw.SetBoilerState(BoilerState.ON),
                Times.Never());
        }

        [Theory, TestConventions]
        public void NoButtonPushWithWaterInBoilerDoesNotStartsBoiler(
            [Frozen]Mock<ICoffeeMaker> hardwareMock,
            Boiler sut)
        {
            sut.OnNext(BoilerStatus.NOT_EMPTY);
            sut.OnNext(BrewButtonStatus.NOT_PUSHED);

            hardwareMock.Verify(
                hw => hw.SetBoilerState(BoilerState.ON),
                Times.Never());
        }

        [Theory, TestConventions]
        public void EmptyBoilerTurnsOffBoiler(
            [Frozen]Mock<ICoffeeMaker> hardwareMock,
            Boiler sut)
        {
            sut.OnNext(BoilerStatus.NOT_EMPTY);
            sut.OnNext(BrewButtonStatus.PUSHED);
            sut.OnNext(BoilerStatus.EMPTY);

            hardwareMock.Verify(hw => hw.SetBoilerState(BoilerState.OFF));
        }

        [Theory, TestConventions]
        public void NotEmptyDoesNotTurnOffBoiler(
            [Frozen]Mock<ICoffeeMaker> hardwareMock,
            Boiler sut)
        {
            sut.OnNext(BoilerStatus.NOT_EMPTY);

            hardwareMock.Verify(
                hw => hw.SetBoilerState(BoilerState.OFF),
                Times.Never());
        }
    }
}
