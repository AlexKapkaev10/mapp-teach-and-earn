using Project.Configs;
using UnityEngine;

namespace Project.Lottery
{
    public interface ILotteryPresenter
    {
        void SetView(ILotteryView view);
        void AddItem(string itemText);
    }
    
    public class LotteryPresenter : ILotteryPresenter
    {
        private readonly LotteryConfig _config;
        private readonly ILotteryModel _model;
        
        private ILotteryView _view;

        public LotteryPresenter(LotteryConfig config, ILotteryModel model)
        {
            _config = config;
            _model = model;
        }

        public void SetView(ILotteryView view)
        {
            _view = view;
        }

        public void AddItem(string itemText)
        {
            if (string.IsNullOrEmpty(itemText) || !_model.TryAddItem(itemText))
            {
                return;
            }

            ILotteryItem item = Object.Instantiate(_config.LotteryItemPrefab, _view.LotteryItemParent);
            item.SetHeader(itemText);
            _model.SetItem(item);
        }
    }
}