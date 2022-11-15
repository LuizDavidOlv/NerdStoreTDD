using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.WebApp.Tests.Config
{
    [CollectionDefinition(nameof(IntegrationWebTestsFixtureCollection))]
    public class IntegrationWebTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<ProgramWebTests>> { }
    
    [CollectionDefinition(nameof(IntegrationWebTestsFixtureCollection))]
    public class IntegrationApiTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<ProgramApiTests>> { }
    
    public class IntegrationTestsFixture<TProgram> : IDisposable where TProgram : class
    {
        public readonly LojaAppFactory<TProgram> Factory;
        public HttpClient Client;

        public IntegrationTestsFixture()
        {
            var clientOoptions = new WebApplicationFactoryClientOptions
            {

            };
            
            Factory = new LojaAppFactory<TProgram>();
            Client = Factory.CreateClient(clientOoptions);
        }
        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
        }
    }
}
