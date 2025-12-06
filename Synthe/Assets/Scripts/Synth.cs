using UnityEngine;
using UnityEngine.InputSystem;

public class Synth : MonoBehaviour
{
    public Oscillator.WaveType waveType = Oscillator.WaveType.Square;
    public SoundGenerator gen;
    public SoundGenerator genDrum;
    [Range(0f, 1f)] public float amplitude = 0.1f;
    public float frequency = 440f;
    private bool sustainNote = false;

    void Start()
    {
        GameObject synthObj = new GameObject("Synth");
        gen = synthObj.AddComponent<SoundGenerator>();
        gen.Init(Oscillator.WaveType.Square, frequency, amplitude);

        GameObject DrumObj = new GameObject("Drum");
        genDrum = DrumObj.AddComponent<SoundGenerator>();
        genDrum.Init(Oscillator.WaveType.Drum, frequency, 0.5f);

    }


    void Update()
    {
        Keyboard();
    }

    private bool PlayNote(SoundGenerator gene, int midiNote, bool sustain = false)
    {
        frequency = gene.NoteToFreq(midiNote);
        gene.SetFrequency(frequency);
        
        if (sustain)
        {
            gene.isPlaying = true;
            return false;
        }
        else
        {
            return true;
        }
    }

    void Keyboard()
    {
        bool anyKey = false;

        gen.SetAmplitude(amplitude);
        gen.SetWaveType(waveType);

        if (Input.GetKey(KeyCode.A)) anyKey |= PlayNote(gen, 48);
        if (Input.GetKey(KeyCode.Z)) anyKey |= PlayNote(gen, 50);
        if (Input.GetKey(KeyCode.E)) anyKey |= PlayNote(gen, 52);
        if (Input.GetKey(KeyCode.R)) anyKey |= PlayNote(gen, 53);
        if (Input.GetKey(KeyCode.T)) anyKey |= PlayNote(gen, 55);
        if (Input.GetKey(KeyCode.Y)) anyKey |= PlayNote(gen, 57);
        if (Input.GetKey(KeyCode.U)) anyKey |= PlayNote(gen, 59);
        if (Input.GetKey(KeyCode.I)) anyKey |= PlayNote(gen, 60);
        if (Input.GetKeyDown(KeyCode.Q)) PlayNote(genDrum, 100, sustain: true);

        gen.isPlaying = anyKey || sustainNote;
    }




}