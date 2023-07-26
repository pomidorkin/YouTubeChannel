using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPeer : MonoBehaviour
{
    //[SerializeField] AudioSource audioSource;
    AudioSource audioSource;
    [SerializeField] AudioManager audioManager;
    public static float[] samples = new float[512]; // 20k samples will be squized into an array of 512
    public static float[] frequencyBands = new float[8];
    public static float[] bandBuffer = new float[8];
    float[] bufferDecrease = new float[8];
    // Start is called before the first frame update
    void Start()
    {
        audioSource = audioManager.GetAudioSource();
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
    }

    private void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    private void BandBuffer()
    {
        for (int i = 0; i < 8; i++)
        {
            if (frequencyBands[i] > bandBuffer[i])
            {
                bandBuffer[i] = frequencyBands[i];
                bufferDecrease[i] = 0.005f;
            }
            if (frequencyBands[i] < bandBuffer[i])
            {
                bandBuffer[i] -= bufferDecrease[i];
                bufferDecrease[i] *= 1.2f;
            }
        }
    }

    private void MakeFrequencyBands()
    {
        // 22050Hz / 512 = 43Hz per sample
        /*
          20 - 60 Hz
          60 - 250 Hz
          250 - 500 Hz
          500 - 2000 Hz
          2000 - 4000 Hz
          4000 - 6000 Hz
          6000 - 20000 Hz
        https://www.youtube.com/watch?v=mHk3ZiKNH48&list=PL3POsQzaCw53p2tA6AWf7_AWgplskR0Vo&index=5&ab_channel=PeerPlay
        */

        int counter = 0;

        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if (i == 7)
            {
                sampleCount += 2;
            }
            for (int ii = 0; ii < sampleCount; ii++)
            {
                average += samples[counter] * (counter + 1);
                counter++;
            }

            average /= counter;
            frequencyBands[i] = average * 10;
        }

    }
}