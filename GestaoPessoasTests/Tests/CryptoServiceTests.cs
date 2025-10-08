using GestaoPessoas.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoPessoasTests.Tests
{
    [TestClass]
    [DoNotParallelize]
    public class CryptoServiceTests
    {
        private TestApplicationDomain? applicationDomain;
        private ICryptoService? service;

        [TestInitialize]
        public void Initialize()
        {
            applicationDomain = new TestApplicationDomain();
            applicationDomain.Services.AddScoped<ICryptoService, CryptoService>();
            service = applicationDomain.ServiceProvider.GetRequiredService<ICryptoService>();

            var configuration = applicationDomain.ServiceProvider.GetRequiredService<IConfiguration>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            applicationDomain?.Dispose();
        }

        [TestMethod]
        public void EncryptDecryptTest()
        {
            string original = "This is a test string.";
            string? encrypted = service!.Encrypt(original);
            Assert.AreNotEqual(original, encrypted);
            string? decrypted = service.Decrypt(encrypted!);
            Assert.AreEqual(original, decrypted);
        }

        [TestMethod]
        public void EncryptFailureTest()
        {
            string emptyInput = "";
            Assert.IsNull(service!.Encrypt(emptyInput));
            string? nullInput = null;
            Assert.IsNull(service!.Encrypt(nullInput!));
        }

        [TestMethod]
        public void DecryptFailureTest()
        {
            string invalidInput = "InvalidBase64String";
            Assert.ThrowsException<InvalidOperationException>(() => service!.Decrypt(invalidInput));
            string? nullInput = null;
            Assert.IsNull(service!.Decrypt(nullInput!));
            string? emptyInput = "";
            Assert.IsNull(service!.Decrypt(emptyInput!));
        }
    }
}
