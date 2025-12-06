using UnityEngine;

public class Oscillator
{
    public enum WaveType { Sine, Square, Saw, Triangle, Drum}

    public WaveType waveType;
    public float amplitude;
    public float frequency;
    private double phase; // Position actuelle de l'oscillateur sur la vague de 0 à 1
    private double sampleRate; // Fréquence d'échantillonnage (nombre de samples par seconde, généralement 48000)

    public DrumSettings drumSettings = new DrumSettings();
    private float drumTime = 0f;
    private float drumDuration = 0.8f;

    public Oscillator(WaveType type, float freq, float amp)
    {
        waveType = type;
        amplitude = amp;
        frequency = freq;
        sampleRate = AudioSettings.outputSampleRate;
    }

    // Calcule la valeur de l'échantillon audio actuel
    public float GetSample()
    {
        // Calcul de l'incrémentation a chaque échantillon
        double phaseIncrement = frequency / sampleRate;
        // Calcule de la valeur de l'onde pour la phase actuelle
        float value = Waveform((float)phase);
        phase = (phase + phaseIncrement) % 1;
        return value;
    }

    private float Waveform(float phase)
    {
        switch (waveType)
        {
            case WaveType.Sine:
                return Mathf.Sin(phase * 2 * Mathf.PI) * amplitude;
            case WaveType.Square:
                return (phase < 0.5 ? 1 : -1) * amplitude;
            case WaveType.Saw:
                return (float)(2f * phase - 1f) * amplitude;
            case WaveType.Triangle:
                return (phase < 0.5f ? 4f * phase - 1f : -4f * phase + 3f) * amplitude;
            case WaveType.Drum:
                return DrumWave(phase);
            default:
                return 0f;
        }
    }

    private float DrumWave(float phase)
    {
        // Avancement dans la boucle
        drumTime += 1f / (float)sampleRate;
        if (drumTime > drumDuration) drumTime -= drumDuration;

        float t = drumTime / drumDuration; // 0 -> 1

        // Pitch descend exponentiellement (courbe modulée)
        float mod = Mathf.Pow(t, 3); // t^3 pour descente plus rapide au début
        float drumFreq = Mathf.Lerp(frequency, drumSettings.endFreq, mod);

        // Envelope amplitude
        float ampEnv = Mathf.Exp(drumSettings.exp * t);

        // Phase update
        float drumPhaseInc = drumFreq / (float)sampleRate;
        phase = (float)((phase + drumPhaseInc) % 1.0f);
        // Onde de base sinusoïdale
        return Mathf.Sin(phase * 2f * Mathf.PI) * ampEnv * amplitude;

    }


}

[System.Serializable]
public class DrumSettings
{
    public float startFreq = 400f;
    public float endFreq = 40f;
    public float exp = -6f;
}
