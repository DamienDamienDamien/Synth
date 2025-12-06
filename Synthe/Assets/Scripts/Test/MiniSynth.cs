using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MiniSynth : MonoBehaviour
{
    public enum WaveType { Sine, Square, Saw, Noise }
    public WaveType waveType = WaveType.Sine;

    public float gain = 0.1f;
    public float frequency = 440f;

    private double sampleRate = 48000.0;
    private double phase = 0;

    void Update()
    {
        // Clavier : touches type piano
        if (Input.GetKey(KeyCode.A)) frequency = NoteToFreq(48);
        if (Input.GetKey(KeyCode.Z)) frequency = NoteToFreq(50);
        if (Input.GetKey(KeyCode.E)) frequency = NoteToFreq(52); 
        if (Input.GetKey(KeyCode.R)) frequency = NoteToFreq(53); 
        if (Input.GetKey(KeyCode.T)) frequency = NoteToFreq(55); 
        if (Input.GetKey(KeyCode.Y)) frequency = NoteToFreq(57);
        if (Input.GetKey(KeyCode.U)) frequency = NoteToFreq(59); 
        if (Input.GetKey(KeyCode.I)) frequency = NoteToFreq(60);

        if (Input.GetKey(KeyCode.Q)) frequency = NoteToFreq(48);
        if (Input.GetKey(KeyCode.S)) frequency = NoteToFreq(50);
        if (Input.GetKey(KeyCode.D)) frequency = NoteToFreq(51);
        if (Input.GetKey(KeyCode.F)) frequency = NoteToFreq(53);
        if (Input.GetKey(KeyCode.G)) frequency = NoteToFreq(55);
        if (Input.GetKey(KeyCode.H)) frequency = NoteToFreq(56);
        if (Input.GetKey(KeyCode.J)) frequency = NoteToFreq(58);
        if (Input.GetKey(KeyCode.K)) frequency = NoteToFreq(60);

        if (Input.anyKeyDown)
        {
            Debug.Log("Fréquence : " + frequency);
        }
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        double increment = frequency * 2.0 * Mathf.PI / sampleRate;

        for (int i = 0; i < data.Length; i += channels)
        {
            phase += increment;

            float sample = GenerateSample((float)phase);

            for (int c = 0; c < channels; c++)
                data[i + c] = sample * gain;
        }
    }

    float GenerateSample(float phase)
    {
        switch (waveType)
        {
            case WaveType.Sine:
                return Mathf.Sin(phase);

            case WaveType.Square:
                return Mathf.Sign(Mathf.Sin(phase));

            case WaveType.Saw:
                return (phase / Mathf.PI % 2f) - 1f;

            case WaveType.Noise:
                return Random.Range(-1f, 1f);
        }
        return 0f;
    }

    float NoteToFreq(int midiNote)
    {
        return 440f * Mathf.Pow(2, (midiNote - 69) / 12f);
    }
}
