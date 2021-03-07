using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PowderBox.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPeer : MonoBehaviour
    {
        [SerializeField] public TrackProfile trackProfile;

        [SerializeField] float bandBoost = 10f;
        [SerializeField, Range(1, 100)] float _bufferDecreaseStartingSpeed = 5f;

        public float[] audioRanged = new float[8];
        public float[] audioRangedBuffer = new float[8];
        public float[] freqBandHighest; // rather readonly to compare against audio profile
        public float audioProfile;
        public float amplitudeRanged;
        public float amplitudeBufferRanged;

        AudioSource _audioSource;

        float[] _samples = new float[512];
        float[] _freqBands = new float[8];
        float[] _bandBuffers = new float[8];
        float[] _bufferDecreaseSpeed = new float[8];
        float _maxAmplitude;

        //todo: split entire class into track profile recorder and player
        //? or implement strategy if you would still like to be able to do live stuff
        //* for live it would be nice to have some commands; like issue printable words with 
        //* animation

        void Start()
        {
            _audioSource = GetComponent<AudioSource>();

            freqBandHighest = new float[_freqBands.Length];

            InitAudioProfile();
        }

        private void InitAudioProfile()
        {
            // it only solves the problem of init values for bands
            // amplitudes could use a similar solution
            //? but how would you do it live
            for (int i = 0; i < freqBandHighest.Length; i++)
            {
                freqBandHighest[i] = audioProfile;
            }
        }

        void Update()
        {
            // if (_audioSource.isPlaying)
            // {
            GetSpectrumAudioSource();
            MakeFrequencyBands();
            BandBuffer();
            CreateRangedBuffers();
            GetAmplitude();
            // }
        }

        private void GetAmplitude()
        {
            float currentAmplitude = 0;
            float currentAmplitudeBuffer = 0;

            for (int i = 0; i < _freqBands.Length; i++)
            {
                currentAmplitude += _freqBands[i];
                currentAmplitudeBuffer += _bandBuffers[i];
            }

            if (currentAmplitudeBuffer > _maxAmplitude)
            {
                _maxAmplitude = currentAmplitudeBuffer;
            }

            amplitudeRanged = currentAmplitude / _maxAmplitude;
            amplitudeBufferRanged = currentAmplitudeBuffer / _maxAmplitude;
        }

        private void CreateRangedBuffers()
        {
            for (int i = 0; i < _freqBands.Length; i++)
            {
                if (_freqBands[i] > freqBandHighest[i])
                {
                    freqBandHighest[i] = _freqBands[i];
                }

                audioRanged[i] = _freqBands[i] / freqBandHighest[i];
                audioRangedBuffer[i] = _bandBuffers[i] / freqBandHighest[i];
            }
        }

        private void BandBuffer()
        {
            for (int i = 0; i < _bandBuffers.Length; i++)
            {
                if (_bandBuffers[i] < _freqBands[i])
                {
                    _bandBuffers[i] = _freqBands[i];
                    _bufferDecreaseSpeed[i] = _bufferDecreaseStartingSpeed / 10000f;
                }
                else
                {
                    _bandBuffers[i] -= _bufferDecreaseSpeed[i];
                    _bufferDecreaseSpeed[i] *= 1.2f;
                }
            }
        }

        void GetSpectrumAudioSource()
        {
            // Read more about FFTWindows, experiment
            _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
        }

        void MakeFrequencyBands()
        {
            int sampleIndex = 0;

            for (int i = 0; i < 8; i++)
            {

                int sampleCountForCurrentBand = (int)Mathf.Pow(2, i + 1);

                if (i == 7)
                {
                    sampleCountForCurrentBand += 2;
                }

                float average = 0;

                for (int j = 0; j < sampleCountForCurrentBand; j++)
                {
                    average += _samples[sampleIndex] * (sampleIndex + 1); // why? probably to boost highs visibility
                    sampleIndex++;
                }

                average /= sampleCountForCurrentBand;
                _freqBands[i] = average * bandBoost;
            }
        }
    }

}
