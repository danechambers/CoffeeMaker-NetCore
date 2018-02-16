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
    public class WarmerPlateTests
    {
        [Theory, TestConventions]
        public void SutIsObserverOfWarmerPlateStatus(WarmerPlate sut)
        {
            Assert.IsAssignableFrom<IObserver<WarmerPlateStatus>>(sut);
        }

        [Theory, TestConventions]
        public void OnCompletedDoesNotThrow(WarmerPlate sut)
        {
            sut.OnCompleted();
        }

        [Theory, TestConventions]
        public void OnErrorDoesNotThrowNotImplementedException(
            WarmerPlate sut,
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
        public void OnNextWarmerEmptyDoesNotThrow(WarmerPlate sut)
        {
            sut.OnNext(WarmerPlateStatus.WARMER_EMPTY);
        }

        [Theory, TestConventions]
        public void OnNexPotNotEmptyTurnsOnWarmer(
            [Frozen]Mock<ICoffeeMaker> hardwareMock,
            WarmerPlate sut)
        {
            sut.OnNext(WarmerPlateStatus.POT_NOT_EMPTY);

            hardwareMock.Verify(hw => hw.SetWarmerState(WarmerState.ON));
            hardwareMock.Verify(
                hw => hw.SetWarmerState(WarmerState.OFF),
                Times.Never());
        }

        [Theory, TestConventions]
        public void OnNextPotEmptyTurnsOffWarmer(
            [Frozen]Mock<ICoffeeMaker> hardwareMock,
            WarmerPlate sut)
        {
            sut.OnNext(WarmerPlateStatus.POT_EMPTY);

            hardwareMock.Verify(hw => hw.SetWarmerState(WarmerState.OFF));
            hardwareMock.Verify(
                hw => hw.SetWarmerState(WarmerState.ON),
                Times.Never());
        }

        [Theory, TestConventions]
        public void OnNextWarmerEmptyTurnsOffWarmer(
            [Frozen]Mock<ICoffeeMaker> hardwareMock,
            WarmerPlate sut)
        {
            sut.OnNext(WarmerPlateStatus.WARMER_EMPTY);

            hardwareMock.Verify(hw => hw.SetWarmerState(WarmerState.OFF));
            hardwareMock.Verify(
                hw => hw.SetWarmerState(WarmerState.ON),
                Times.Never());
        }
    }
}
