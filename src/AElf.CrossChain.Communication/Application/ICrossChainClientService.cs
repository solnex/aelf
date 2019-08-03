using System.Threading.Tasks;

namespace AElf.CrossChain.Communication.Application
{
    public interface ICrossChainClientService
    {
        ICrossChainClient CreateClientForChainInitializationData(int chainId);
        Task CreateClientAsync(CrossChainClientDto crossChainClientDto);
        Task<ICrossChainClient> GetClientAsync(int chainId);
        Task CloseClientsAsync();
    }
}