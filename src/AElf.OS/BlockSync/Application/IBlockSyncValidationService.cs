using System.Threading.Tasks;
using AElf.Kernel;
using AElf.OS.Network;
using AElf.Types;

namespace AElf.OS.BlockSync.Application
{
    public interface IBlockSyncValidationService
    {
        Task<bool> ValidateAnnouncementAsync(Chain chain, BlockAnnouncement blockAnnouncement, string senderPubkey);
        Task<bool> ValidateBlockAsync(Chain chain, BlockWithTransactions blockWithTransactions, string senderPubkey);
    }
}