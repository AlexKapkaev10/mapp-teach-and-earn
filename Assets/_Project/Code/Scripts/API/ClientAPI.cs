using System;
using System.Threading;
using System.Threading.Tasks;
using Project.Infrastructure.Extensions;
using Project.Scripts.API.Response;
using Project.Scripts.Bank;
using UnityEngine;
using UnityEngine.Networking;
using VContainer;

namespace Project.Scripts.API
{
    public interface IClientAPI : IDisposable
    {
        void RandomClaimAsync(Action<bool, float> callBack);
        void ConfirmTransaction();
    }
    
    public class ClientAPI : IClientAPI
    {
        private CancellationTokenSource _cts = new ();
        private IBank _bank;

        private const string _url = "https://jsonplaceholder.typicode.com/posts";

        [Inject]
        private void Construct(IBank bank)
        {
            _bank = bank;
        }

        public async void RandomClaimAsync(Action<bool, float> callBack)
        {
            try
            {
                var token = _cts.Token;
                var claimPoints = await ClaimPointsAsync(_url, token);
                
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
                
                _bank.SetPoints(claimPoints.Value);
                callBack?.Invoke(claimPoints.IsSuccess, claimPoints.Value);
            }
            catch (Exception e)
            {
                callBack?.Invoke(false, 0);
                this.Log(e.Message);
            }
        }

        public void ConfirmTransaction()
        {
            _bank.SetPoints(100);
        }

        public void OpenUrl(string url)
        {
            Application.OpenURL(url);
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts = null;
        }

        private async Task<ClaimResponse> ClaimPointsAsync(string url, CancellationToken token)
        {
            using UnityWebRequest request = UnityWebRequest.Get(url);

            await request.SendWebRequest();

            if (token.IsCancellationRequested)
            {
                token.ThrowIfCancellationRequested();
            }

            var isSuccess = request.result == UnityWebRequest.Result.Success;
            
            ClaimResponse claimResponse = new ClaimResponse
            {
                IsSuccess = isSuccess
            };

            if (isSuccess)
            {
                claimResponse.Value = _bank.ClaimPoints();
                this.Log(request);
            }

            return claimResponse;
        }
    }
}