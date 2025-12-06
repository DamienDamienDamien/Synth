using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MiniSynthAvance : MonoBehaviour
{
    public enum WaveType { Sine, Square, Saw, Noise }
    public WaveType waveType = WaveType.Sine;

    [Range(0f, 0.5f)]
    public float gain = 0.1f;

    private double sampleRate = 48000.0;
    private double phase1 = 0;
    private double phase2 = 0;

    private float lastSample = 0f; // pour le filtre

    // LFO pour vibrato
    public float lfoFrequency = 5f;
    public float lfoAmount = 5f; // Hz

    private float frequency = 440f;
    private double dspTime = 0;

    void Update()
    {
        // AZERTY mapping : Do majeur
        if (Input.GetKeyDown(KeyCode.A)) { frequency = NoteToFreq(48); Debug.Log("Do (C3)"); }
        if (Input.GetKeyDown(KeyCode.Z)) { frequency = NoteToFreq(50); Debug.Log("Ré (D3)"); }
        if (Input.GetKeyDown(KeyCode.E)) { frequency = NoteToFreq(52); Debug.Log("Mi (E3)"); }
        if (Input.GetKeyDown(KeyCode.R)) { frequency = NoteToFreq(53); Debug.Log("Fa (F3)"); }
        if (Input.GetKeyDown(KeyCode.T)) { frequency = NoteToFreq(55); Debug.Log("Sol (G3)"); }
        if (Input.GetKeyDown(KeyCode.Y)) { frequency = NoteToFreq(57); Debug.Log("La (A3)"); }
        if (Input.GetKeyDown(KeyCode.U)) { frequency = NoteToFreq(59); Debug.Log("Si (B3)"); }
        if (Input.GetKeyDown(KeyCode.I)) { frequency = NoteToFreq(60); Debug.Log("Do (C4)"); }
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            // Calculer le temps audio
            dspTime += 1.0 / sampleRate;

            // LFO vibrato
            float vibrato = Mathf.Sin((float)(2 * Mathf.PI * lfoFrequency * dspTime)) * lfoAmount;

            double freq1 = frequency + vibrato;
            double freq2 = frequency * 1.01 + vibrato; // détune

            double increment1 = freq1 * 2.0 * Mathf.PI / sampleRate;
            double increment2 = freq2 * 2.0 * Mathf.PI / sampleRate;

            phase1 += increment1;
            phase2 += increment2;

            // Génération des échantillons
            float sample1 = GenerateSample((float)phase1);
            float sample2 = GenerateSample((float)phase2);
            float sample = (sample1 + sample2) * 0.5f;

            // Filtre low-pass
            float alpha = 0.05f;
            sample = alpha * sample + (1 - alpha) * lastSample;
            lastSample = sample;

            for (int c = 0; c < channels; c++)
                data[i + c] = sample * gain;
        }
    }

    float GenerateSample(float phase)
    {
        switch (waveType)
        {
            case WaveType.Sine: return Mathf.Sin(phase);
            case WaveType.Square: return Mathf.Sign(Mathf.Sin(phase));
            case WaveType.Saw: return (phase / Mathf.PI % 2f) - 1f;
            case WaveType.Noise: return Random.Range(-1f, 1f);
        }
        return 0f;
    }

    float NoteToFreq(int midiNote)
    {
        return 440f * Mathf.Pow(2, (midiNote - 69) / 12f);
    }
}
