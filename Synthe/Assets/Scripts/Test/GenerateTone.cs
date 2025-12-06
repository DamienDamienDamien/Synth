using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GenerateTone : MonoBehaviour
{
    public float frequency = 440f;   // fréquence du son (Hz)
    public float gain = 0.1f;        // volume
    private double phase = 0.0;
    private double increment;
    private double sampling_rate = 48000.0;

    void OnAudioFilterRead(float[] data, int channels)
    {
        increment = frequency * 2.0 * Mathf.PI / sampling_rate;

        for (int i = 0; i < data.Length; i += channels)
        {
            phase += increment;
            float sample = (float)(Mathf.Sin((float)phase) * gain);

            // Pour chaque canal (gauche/droite)
            for (int c = 0; c < channels; c++)
                data[i + c] = sample;
        }
    }
}
