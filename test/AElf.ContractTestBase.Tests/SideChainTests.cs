using System.Threading.Tasks;
using AElf.Contracts.CrossChain;
using AElf.Contracts.TestKit;
using AElf.CrossChain;
using AElf.Kernel;
using AElf.Kernel.Blockchain.Application;
using AElf.Types;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace AElf.ContractTestBase.Tests
{
    public class SideChainTests : SideChainTestBase
    {
        [Fact]
        public async Task Test()
        {
            var blockchainService = Application.ServiceProvider.GetRequiredService<IBlockchainService>();
            var chain = await blockchainService.GetChainAsync();
            var chainContext = new ChainContext
            {
                BlockHash = chain.BestChainHash,
                BlockHeight = chain.BestChainHeight
            };
            var address = await ContractAddressService.GetAddressByContractNameAsync(chainContext,CrossChainSmartContractAddressNameProvider.StringName);
            var crossChainStub = GetTester<CrossChainContractContainer.CrossChainContractStub>(address, SampleECKeyPairs.KeyPairs[0]);
            var parentChainId = await crossChainStub.GetParentChainId.CallAsync(new Empty());
            ChainHelper.ConvertChainIdToBase58(parentChainId.Value).ShouldBe("AELF");
        }
    }
}