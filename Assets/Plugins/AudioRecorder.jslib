mergeInto(LibraryManager.library, {
    // Переменные для записи аудио
    audioContext: null,
    destination: null,
    mediaRecorder: null,
    recordedChunks: [],

    // Функция для начала записи
    startRecording: function() {
        console.log("Initializing recording...");

        if (!this.audioContext) {
            this.audioContext = new (window.AudioContext || window.webkitAudioContext)();
        }

        // Создаём MediaStreamDestination для перехвата воспроизводимого звука
        this.destination = this.audioContext.createMediaStreamDestination();

        // Создаём MediaRecorder для записи звука
        this.mediaRecorder = new MediaRecorder(this.destination.stream);

        // Обработка данных при наличии нового аудиоблока
        this.mediaRecorder.ondataavailable = function(event) {
            if (event.data.size > 0) {
                this.recordedChunks.push(event.data);
            }
        }.bind(this);

        this.mediaRecorder.start();
        console.log("Recording started...");
    },

    // Функция для остановки записи
    stopRecording: function() {
        if (!this.mediaRecorder) {
            console.error("MediaRecorder is not initialized.");
            return;
        }

        this.mediaRecorder.stop();

        this.mediaRecorder.onstop = function() {
            console.log("Recording stopped. Preparing data...");

            // Формируем Blob из записанных данных
            const blob = new Blob(this.recordedChunks, { type: 'audio/webm' });
            this.recordedChunks = []; // Очищаем буфер записей

            // Конвертируем Blob в ArrayBuffer
            const reader = new FileReader();
            reader.readAsArrayBuffer(blob);

            reader.onloadend = function() {
                const arrayBuffer = reader.result;

                const audioBytes = new Uint8Array(arrayBuffer);

                const jsonData = JSON.stringify({ data: Array.from(audioBytes) });
                
                window.unityInstanceRef.SendMessage(
                    'WebGLAudioRecorder', // Имя объекта в Unity
                    'OnRecordingComplete', // Имя метода
                    jsonData // JSON-данные
                );

                console.log("Data sent to Unity.");
            };
        }.bind(this);
    }
});
