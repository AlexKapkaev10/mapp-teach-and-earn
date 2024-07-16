using System;
using System.Collections.Generic;

namespace Project.Lottery
{
    public interface ILotteryModel
    {
        void SetItem(ILotteryItem lotteryItem);
        bool TryAddItem(string itemHeader);
    }
    
    public class LotteryModel : ILotteryModel
    {
        private readonly List<ILotteryItem> _lotteryItems = new();
        public void SetItem(ILotteryItem lotteryItem)
        {
            _lotteryItems.Add(lotteryItem);
        }

        public bool TryAddItem(string itemHeader)
        {
            foreach (var lotteryItem in _lotteryItems)
            {
                if (lotteryItem.GetHeader() == itemHeader)
                {
                    return false;
                }
            }

            return true;
        }
    }
}