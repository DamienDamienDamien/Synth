using UnityEngine;

public class DrumSynth : BaseSound
{
    [Header("Drum Pitch Drop")]
    public float startFreq = 120f;
    public float endFreq = 30f;
    public float duration = 0.15f;
    public float decayExp = -6f;

    void Start()
    {
        Init();
        SetWaveType(Oscillator.WaveType.Drum);

        //generator.SetDrumSettings(startFreq, endFreq, duration, decayExp);
    }

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //generator.TriggerDrum();
            isPlaying = true;
        }
    }


}
