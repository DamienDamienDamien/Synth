using Unity.VisualScripting;
using UnityEngine;

public class SynthClass : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float sine_x(float amp, float freq, float time)
    {
        return (int)(Mathf.Round(amp * Mathf.Sin(2 * Mathf.PI * freq * time)));

    }
}
