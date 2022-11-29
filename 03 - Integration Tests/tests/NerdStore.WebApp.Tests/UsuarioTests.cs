using Microsoft.VisualStudio.TestPlatform.TestHost;
using NerdStore.WebApp.Tests.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.WebApp.Tests
{

    [Collection(nameof(IntegrationWebTestsFixtureCollection))]
    public class UsuarioTests
    {
        private readonly IntegrationTestsFixture<Program> testsFixture;

        public UsuarioTests(IntegrationTestsFixture<Program> testsFixture)
        {
            this.testsFixture = testsFixture;

        }
        
        [Fact(DisplayName ="Realizar cadastro com sucesso")]
        [Trait("Usuário","Integração Web - Usuário")]
        public async Task Usuario_RealizarCadastro_DeveExecutarComSucesso()
        {
            //arrange
            var initialResponse = await this.testsFixture.Client.GetAsync("/Identity/Account/Register");
            initialResponse.EnsureSuccessStatusCode();

            var email = "teste@teste.com";
            
            var formData = new Dictionary<string,string>
            {
                {"Input.Email",email},
                {"Input.Password","Teste@123"},
                {"Input.ConfirmPassword","Teste@123"}
            };
        

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Identity/Account/Register")
            {
                Content = new FormUrlEncodedContent(formData)
            };

            //act
            var postResponse = await this.testsFixture.Client.SendAsync(postRequest);

            //assert
            postResponse.EnsureSuccessStatusCode();
        }
    }
}
