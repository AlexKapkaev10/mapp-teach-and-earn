using System;
using System.Collections;
using Project.Scripts.Audio;
using Project.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Loader
{
    public interface ILoaderView
    {
        event Action LoadComplete;
        void StartSlider();
    }
    
    public class LoaderView : View, ILoaderView
    {
        public event Action LoadComplete;
        
        [SerializeField] private Slider _slider;
        [SerializeField] private float _loadSimulationSpeed = 0.002f;


        public void StartSlider()
        {
            StartCoroutine(LoadSimulationAsync());
        }

        private IEnumerator LoadSimulationAsync()
        {
            while (_slider.value < 1)
            {
                _slider.value += _loadSimulationSpeed * Time.unscaledTime;
                yield return null;
            }
            LoadComplete?.Invoke();
            Destroy(gameObject);
        }
    }
}