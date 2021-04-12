using System;
using System.Linq;
using Shouldly;
using Xunit;

namespace Yay.Enumerations.UnitTests
{
    public class EnumerationTests
    {
        [Fact]
        public void ShouldGetAll()
        {
            var all = ForTestCasesEnumeration.GetAll();
            all.Count().ShouldBe(4);
        }

        [Fact]
        public void ShouldGetByDisplayName()
        {
            var all = ForTestCasesEnumeration.FromDisplayName("Normal");
            all.ShouldBe(ForTestCasesEnumeration.Normal);
        }
        
        [Fact]
        public void ShouldGetByDisplayNameOrDefault()
        {
            var all = ForTestCasesEnumeration.FromDisplayNameOrDefault("Normal");
            all.ShouldBe(ForTestCasesEnumeration.Normal);
        }
        
        [Fact]
        public void ShouldGetByValue()
        {
            var all = ForTestCasesEnumeration.FromValue(1);
            all.ShouldBe(ForTestCasesEnumeration.Normal);
        }
        
        [Fact]
        public void ShouldGetByValueOrDefault()
        {
            var all = ForTestCasesEnumeration.FromValueOrDefault(1);
            all.ShouldBe(ForTestCasesEnumeration.Normal);
        }
    }
}
