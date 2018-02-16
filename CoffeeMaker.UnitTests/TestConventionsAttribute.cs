using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace Ploeh.Samples.CoffeeMaker.UnitTests
{
    public class TestConventionsAttribute : AutoDataAttribute
    {
        public TestConventionsAttribute()
            : base(new Fixture().Customize(new AutoMoqCustomization()))
        {
        }
    }
}
