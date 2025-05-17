using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PulseToTheBeat : MonoBehaviour
{
    [SerializeField] private float sizeMultiplier = 1.25f;
    [SerializeField] private float returnSpeed = 5f;
    [SerializeField] private AudioSource sound;
    [SerializeField] private Sound type;
    public static Action<Sound> onSound;
    private Vector3 startSize;


    void OnEnable()
    {
        onSound += Pulse;
    }
    void OnDisable()
    {
        onSound -= Pulse;
    }

    void Start()
    {
        startSize = transform.localScale;
        if (sound == null) sound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, startSize, Time.deltaTime * returnSpeed);
    }

    public void Pulse(Sound typeOf)
    {
        if (typeOf != type) return;

        transform.localScale = startSize * sizeMultiplier;
        sound.Play();
    }

}


public enum Sound
{
    Kick,
    HiHat,
    Snare,
    Clap
}