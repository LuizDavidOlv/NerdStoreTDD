using Microsoft.VisualStudio.TestPlatform.TestHost;
using NerdStore.WebApp.Tests.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.WebApp.Tests
{
    public class UsuarioTests
    {
        private readonly IntegrationTestsFixture<Program> testsFixture;

        public UsuarioTests(IntegrationTestsFixture<Program> testsFixture)
        {
            this.testsFixture = testsFixture;
        }
    }
}
