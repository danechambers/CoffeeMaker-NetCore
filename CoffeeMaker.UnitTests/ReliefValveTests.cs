using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using AutoFixture.Xunit2;
using Ploeh.Samples.CoffeeMaker;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.CoffeeMaker.UnitTests
{
    public class ReliefValveTests
    {
        [Theory, TestConventions]
        public void SutIsObserverOfWarmerPlateStatus(ReliefValve sut)
        {
            Assert.IsAssignableFrom<IObserver<WarmerPlateStatus>>(sut);
        }

        [Theory, TestConventions]
        public void OnCompletedDoesNotThrow(ReliefValve sut)
        {
            sut.OnCompleted();
        }

        [Theory, TestConventions]
        public void OnErrorDoesNotThrowNotImplementedException(
            ReliefValve sut,
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
        public void OnNextWarmerEmptyDoesNotThrow(ReliefValve sut)
        {
            sut.OnNext(WarmerPlateStatus.WARMER_EMPTY);
        }

        [Theory, TestConventions]
        public void OpenValveWhenRemovingPotFromWarmerPlate(
            [Frozen]Mock<ICoffeeMaker> hardwareMock,
            ReliefValve sut)
        {
            sut.OnNext(WarmerPlateStatus.WARMER_EMPTY);

            hardwareMock.Verify(
                hw => hw.SetReliefValveState(ReliefValveState.OPEN));
        }

        [Theory, TestConventions]
        public void CloseValveWhenEmptyPotIsPresent(
            [Frozen]Mock<ICoffeeMaker> hardwareMock,
            ReliefValve sut)
        {
            sut.OnNext(WarmerPlateStatus.POT_EMPTY);

            hardwareMock.Verify(
                hw => hw.SetReliefValveState(ReliefValveState.CLOSED));
        }

        [Theory, TestConventions]
        public void CloseValveWhenPotIsPresent(
            [Frozen]Mock<ICoffeeMaker> hardwareMock,
            ReliefValve sut)
        {
            sut.OnNext(WarmerPlateStatus.POT_NOT_EMPTY);

            hardwareMock.Verify(
                hw => hw.SetReliefValveState(ReliefValveState.CLOSED));
        }
    }
}
