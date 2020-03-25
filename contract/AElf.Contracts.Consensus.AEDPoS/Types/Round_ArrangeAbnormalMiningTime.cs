using System.Linq;
using AElf.CSharp.Core;
using AElf.CSharp.Core.Extension;
using AElf.Sdk.CSharp;
using Google.Protobuf.WellKnownTypes;

// ReSharper disable once CheckNamespace
namespace AElf.Contracts.Consensus.AEDPoS
{
    public partial class Round
    {
        /// <summary>
        /// If one node produced block this round or missed his time slot,
        /// whatever how long he missed, we can give him a consensus command with new time slot
        /// to produce a block (for terminating current round and start new round).
        /// The schedule generated by this command will be cancelled
        /// if this node executed blocks from other nodes.
        /// </summary>
        /// <returns></returns>
        public Timestamp ArrangeAbnormalMiningTime(string pubkey, Timestamp currentBlockTime, bool mustExceededCurrentRound = false)
        {
            var miningInterval = GetMiningInterval();

            var minerInRound = RealTimeMinersInformation[pubkey];

            if (GetExtraBlockProducerInformation().Pubkey == pubkey && !mustExceededCurrentRound)
            {
                var distance = (GetExtraBlockMiningTime().AddMilliseconds(miningInterval) - currentBlockTime).Milliseconds();
                if (distance > 0)
                {
                    return GetExtraBlockMiningTime();
                }
            }

            var distanceToRoundStartTime = (currentBlockTime - GetRoundStartTime()).Milliseconds();
            var missedRoundsCount = distanceToRoundStartTime.Div(TotalMilliseconds(miningInterval));
            var futureRoundStartTime = CalculateFutureRoundStartTime(missedRoundsCount, miningInterval);
            return futureRoundStartTime.AddMilliseconds(minerInRound.Order.Mul(miningInterval));
        }

        public bool IsInCorrectFutureMiningSlot(string pubkey, Timestamp currentBlockTime)
        {
            var miningInterval = GetMiningInterval();

            var arrangedMiningTime =
                ArrangeAbnormalMiningTime(pubkey, currentBlockTime.AddMilliseconds(-miningInterval));

            return arrangedMiningTime <= currentBlockTime &&
                   currentBlockTime <= arrangedMiningTime.AddMilliseconds(miningInterval);
        }

        private MinerInRound GetExtraBlockProducerInformation()
        {
            return RealTimeMinersInformation.First(bp => bp.Value.IsExtraBlockProducer).Value;
        }

        /// <summary>
        /// This method for now is able to handle the situation of a miner keeping offline so many rounds,
        /// by using missedRoundsCount.
        /// </summary>
        /// <param name="miningInterval"></param>
        /// <param name="missedRoundsCount"></param>
        /// <returns></returns>
        private Timestamp CalculateFutureRoundStartTime(long missedRoundsCount = 0, int miningInterval = 0)
        {
            if (miningInterval == 0)
                miningInterval = GetMiningInterval();

            var totalMilliseconds = TotalMilliseconds(miningInterval);
            return GetRoundStartTime().AddMilliseconds(missedRoundsCount.Add(1).Mul(totalMilliseconds));
        }

        /// <summary>
        /// In current AElf Consensus design, each miner produce his block in one time slot, then the extra block producer
        /// produce a block to terminate current round and confirm the mining order of next round.
        /// So totally, the time of one round is:
        /// MiningInterval * MinersCount + MiningInterval.
        /// </summary>
        /// <param name="miningInterval"></param>
        /// <returns></returns>
        private int TotalMilliseconds(int miningInterval = 0)
        {
            if (miningInterval == 0)
            {
                miningInterval = GetMiningInterval();
            }

            return RealTimeMinersInformation.Count * miningInterval + miningInterval;
        }
    }
}