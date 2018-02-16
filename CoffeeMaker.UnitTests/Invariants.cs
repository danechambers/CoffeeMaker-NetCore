using AutoFixture.Idioms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.CoffeeMaker.UnitTests
{
    public class Invariants
    {
        [Theory, TestConventions]
        public void ConstructorsHaveAppropriateGuardClauses(
            GuardClauseAssertion assertion)
        {
            var representativeType = typeof(ICoffeeMaker);
            assertion.Verify(representativeType.Assembly
                .GetExportedTypes()
                .SelectMany(t => t.GetConstructors()));
        }
    }
}
