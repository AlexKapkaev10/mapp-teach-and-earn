using UnityEngine;

namespace Project.Scripts
{
    public class Metronome : MonoBehaviour
    {
        public AudioSource tickSound; // Звук метронома
        public int bpm = 120; // Начальный BPM
        private float beatInterval; // Интервал между ударами в секундах
        private float nextBeatTime; // Время следующего удара

        void Start()
        {
            UpdateBeatInterval();
            nextBeatTime = Time.time; // Инициализация времени следующего удара
        }

        void Update()
        {
            // Проверяем текущее время на необходимость удара
            if (Time.time >= nextBeatTime)
            {
                PlayTick();
                nextBeatTime += beatInterval; // Устанавливаем время следующего удара
            }
        }

        private void PlayTick()
        {
            // Проигрываем звук удара
            if (tickSound != null)
            {
                tickSound.Play();
            }
        }

        public void SetBPM(int newBPM)
        {
            // Обновляем BPM и пересчитываем интервал
            bpm = Mathf.Clamp(newBPM, 30, 300); // Ограничиваем BPM от 30 до 300
            UpdateBeatInterval();

            // Корректируем время следующего удара, чтобы избежать рассинхронизации
            nextBeatTime = Time.time + beatInterval;
        }

        private void UpdateBeatInterval()
        {
            // Пересчитываем интервал между ударами на основе нового BPM
            beatInterval = 60f / bpm;
        }
    }
}