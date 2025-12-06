using UnityEngine;

// Ajoute automatiquement un AudioSource si il n'y en a pas
[RequireComponent(typeof(AudioSource))]

public class SoundGenerator1 : MonoBehaviour
{
    private Oscillator osc; // L'oscillateur qui génère les échantillons audio
    private float amplitude;
    public float frequency;
    public bool isPlaying = false;
    private float currentGain = 0f;
    public float FadeIN = 0.01f;
    public float FadeOUT = 0.05f;
    private double sampleRate;

    public float drumStartFreq = 2653;
    public float drumEndFreq = 50f;
    public float drumExp = -6f;

    // Méthode d'initialisation de l'oscillateur
    public void Init(Oscillator.WaveType type = Oscillator.WaveType.Sine, float freq = 261.62f, float amp = 0.5F)
    {
      
        osc = new Oscillator(type, freq, amp);
        sampleRate = AudioSettings.outputSampleRate;
    }

    // Méthode pour générer des données audio
    private void OnAudioFilterRead(float[] data, int channels)
    {
        float attackIncrement = FadeIN > 0 ? 1f / (float)(FadeIN * sampleRate) : 1f;
        float releaseIncrement = FadeOUT > 0 ? 1f / (float)(FadeOUT * sampleRate) : 1f;

        // boucle qui calcule la valeur de chaque échantillon
        for (int i = 0; i < data.Length; i += channels)
        {
            if (isPlaying)
                currentGain = Mathf.Min(currentGain + attackIncrement, 1f);
            else
                currentGain = Mathf.Max(currentGain - releaseIncrement, 0f);

            // Récupère la valeur du sample courant depuis l'oscillateur
            float sample = osc.GetSample() * currentGain;

            // Remplit tous les canaux audio (stéréo, etc.) avec la même valeur
            for (int c = 0; c < channels; c++)
            {
                data[i + c] = sample;
            }
        }
    }

    public void SetFrequency(float freq)
    {
        frequency = freq;
        if (osc != null)
            osc.frequency = freq;
    }

    public void SetAmplitude(float amp)
    {
        amplitude = amp;
        if (osc != null)
            osc.amplitude = amp;
    }

    public void SetWaveType(Oscillator.WaveType type)
    {
        if (osc != null)
            osc.waveType = type;
    }

    public float NoteToFreq(int midiNote)
    {
        return 440f * Mathf.Pow(2, (midiNote - 69) / 12f);
    }

    private void Update()
    {
        if (osc.waveType == Oscillator.WaveType.Drum)
        {
            osc.drumSettings.startFreq = drumStartFreq;
            osc.drumSettings.endFreq = drumEndFreq;
            osc.drumSettings.exp = drumExp;
        }
    }
}
