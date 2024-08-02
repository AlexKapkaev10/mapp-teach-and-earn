namespace Project.Code.Scripts.Module.Mining
{
    public interface IMiningPresenter
    {
        void Init(IMiningView view);
        void Claim();
        void Buy();
        void Upgrade();
        void TransactionSend();
    }
    
    public class MiningPresenter : IMiningPresenter
    {
        private readonly IMiningModel _model;
        private IMiningView _view;
        
        public MiningPresenter(IMiningModel model)
        {
            _model = model;
        }

        public void Init(IMiningView view)
        {
            _view = view;
            _view.UpdateScore(_model.Score.ToString("F"));
            _model.SetInit();
        }

        public void Claim()
        {
            _model.Claim(out var value);
            _view.UpdateScore(value.ToString("F"));
        }

        public void Buy()
        {
            _model.Buy();
        }

        public void Upgrade()
        {
            _model.Upgrade();
        }

        public void TransactionSend()
        {
            _model.TransactionSend();
            _view.UpdateScore(_model.Score.ToString("F"));
        }
    }
}