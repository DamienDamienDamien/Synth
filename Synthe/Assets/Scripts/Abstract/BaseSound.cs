using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public abstract class BaseSound : MonoBehaviour
{
    [Header("Base Synth Settings")]
    public Oscillator osc;
    public Oscillator.WaveType waveType = Oscillator.WaveType.Sine;
    public float amplitude = 0.5f;
    public float baseFrequency = 440f;
    public int sampleRate = 48000;
    public float FadeIN = 0.01f;  // Temps d'attaque en secondes
    public float FadeOUT = 0.05f; // Temps de relâche en secondes
    protected float currentGain = 0f; // Gain actuel pour les enveloppes d'attaque et de relâche
    public float frequency = 440f;
    private bool _isPlaying = false;

    //protected SoundGenerator generator; // L’objet qui génère réellement le son
    protected bool initialized = false;
 
    public virtual void Init()
    {
        if (initialized) return;
        //generator = gameObject.AddComponent<SoundGenerator>();
        //generator.Init(waveType, baseFrequency, amplitude);
        osc = new Oscillator(waveType, baseFrequency, amplitude);
        initialized = true;
    }

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


    public virtual void SetFrequency(float freq)
    {
        frequency = freq;
        osc.frequency = freq;
    }

    public virtual void SetAmplitude(float amp)
    {
        amplitude = amp;
        osc.amplitude = amp;
    }

    public virtual void SetWaveType(Oscillator.WaveType type)
    {
        waveType = type;
        //if (initialized) generator.SetWaveType(type);
        osc.waveType = type;
    }

    public float NoteToFreq(int midiNote)
    {
        return 440f * Mathf.Pow(2, (midiNote - 69) / 12f);
    }

    public bool isPlaying
    {
        get => initialized && _isPlaying;
        set
        {
            if (initialized)
                _isPlaying = value;
        }
    }

    protected abstract void Update();
}
