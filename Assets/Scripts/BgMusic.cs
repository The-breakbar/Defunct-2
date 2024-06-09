using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgMusic : MonoBehaviour
{
    public AudioSource audioSource;
    private float[] spectrum;
    private const int spectrumSize = 64;

    [Range(0, spectrumSize - 1)]
    public int spectrumBand;

    [Range(0.0f, 2.0f)]
    public float multiplier;

    public void FixedUpdate()
    {
        spectrum = new float[spectrumSize];
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Blackman);
    }

    public float GetSpectrum()
    {
        if (spectrum == null) return 0.0f;

        return Mathf.Log(spectrum[spectrumBand] + 0.001f, 2) * -1 * multiplier;
    }
}
