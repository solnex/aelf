using AElf.Types;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace AElf.Kernel.Blockchain.Infrastructure
{
    public interface IStaticChainInformationProvider
    {
        int ChainId { get; }
        Address ZeroSmartContractAddress { get; }

        Address GetSystemContractAddressInGenesisBlock(long i);
        
        Address GetZeroSmartContractAddress(int chainId);
    }

    public class StaticChainInformationProvider : IStaticChainInformationProvider, ISingletonDependency
    {
        public int ChainId { get; }
        public Address ZeroSmartContractAddress { get; }

        public Address GetSystemContractAddressInGenesisBlock(long i)
        {
            return BuildContractAddress(ChainId, i);
        }

        public Address GetZeroSmartContractAddress(int chainId)
        {
            return BuildContractAddress(chainId, 0);
        }

        public StaticChainInformationProvider(IOptionsSnapshot<ChainOptions> chainOptions)
        {
            ChainId = chainOptions.Value.ChainId;
            ZeroSmartContractAddress = BuildContractAddress(ChainId, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chainId"></param>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public static Address BuildContractAddress(Hash chainId, long serialNumber)
        {
            var hash = Hash.FromTwoHashes(chainId, Hash.FromRawBytes(serialNumber.ToBytes()));
            return Address.FromBytes(hash.ToByteArray());
        }

        public static Address BuildContractAddress(int chainId, long serialNumber)
        {
            return BuildContractAddress(chainId.ToHash(), serialNumber);
        }
    }
}