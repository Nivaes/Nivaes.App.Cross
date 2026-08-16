using System;
using System.Collections.Generic;
using System.Text;

namespace Nivaes.App.Cross.UnitTest
{
    public sealed class TestPlatformApplication : IPlatformApplication
    {
        public IServiceProvider ServiceProvider { get; }

        public ICrossApplication Application => throw new NotImplementedException();

        public TestPlatformApplication(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
    }
}
