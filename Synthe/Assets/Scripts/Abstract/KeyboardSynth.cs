using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardSynth : BaseSound
{
    void Start()
    {
        Init();
        SetWaveType(Oscillator.WaveType.Sine);
    }

    protected override void Update()
    {
        bool pressed = false;

        if (Input.GetKey(KeyCode.A)) { pressed = true; SetFrequency(NoteToFreq(48)); }
        if (Input.GetKey(KeyCode.Z)) { pressed = true; SetFrequency(NoteToFreq(50)); }
        if (Input.GetKey(KeyCode.E)) { pressed = true; SetFrequency(NoteToFreq(52)); }
        if (Input.GetKey(KeyCode.R)) { pressed = true; SetFrequency(NoteToFreq(53)); }
        if (Input.GetKey(KeyCode.T)) { pressed = true; SetFrequency(NoteToFreq(55)); }
        if (Input.GetKey(KeyCode.Y)) { pressed = true; SetFrequency(NoteToFreq(57)); }
        if (Input.GetKey(KeyCode.U)) { pressed = true; SetFrequency(NoteToFreq(59)); }
        if (Input.GetKey(KeyCode.I)) { pressed = true; SetFrequency(NoteToFreq(60)); }

        isPlaying = pressed;
    }
}
