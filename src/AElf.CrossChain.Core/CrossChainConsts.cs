using System.Collections.Generic;

namespace AElf.CrossChain
{
    public class CrossChainConsts
    {
        public const int WaitingIntervalInMillisecond = 10;
        public const int MaximalCountForIndexingParentChainBlock = 256; // Maximal count for once indexing from parent chain.
        public const int MaximalCountForIndexingSideChainBlock = 256; // Maximal count for once indexing from side chain.
        public const int MinimalBlockInfoCacheThreshold = 4;
        public const string CrossChainIndexingMethodName = "RecordCrossChainData";
        public static readonly List<string> SymbolsOfExchangedExtraData = new List<string>{"Consensus"};
    }
}