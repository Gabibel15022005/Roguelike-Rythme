// --- BeatManager.cs ---
using System;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    [SerializeField] private float bpm = 120f;
    public float BPM => bpm;

    [SerializeField] private Intervals[] intervals;
    public Intervals[] Intervals => intervals;

    private double dspStartTime;
    private bool isPlaying = false;
    private double pauseTimeOffset = 0;

    public void Play()
    {
        dspStartTime = AudioSettings.dspTime - pauseTimeOffset;
        isPlaying = true;

        foreach (var interval in intervals)
        {
            interval.Init(bpm, dspStartTime);
        }
    }

    public void Pause()
    {
        if (!isPlaying) return;
        pauseTimeOffset = AudioSettings.dspTime - dspStartTime;
        isPlaying = false;
    }

    public void Stop()
    {
        isPlaying = false;
        pauseTimeOffset = 0;
    }

    public void SetBPM(float newBpm)
    {
        bpm = newBpm;
        if (!isPlaying) return;

        foreach (var interval in intervals)
        {
            interval.UpdateBPM(bpm, dspStartTime);
        }
    }

    void Update()
    {
        if (!isPlaying) return;

        double dspTime = AudioSettings.dspTime;
        foreach (var interval in intervals)
        {
            interval.Update(dspTime);
        }
    }
}

// --- Intervals.cs ---
[Serializable]
public class Intervals
{
    [SerializeField] private int steps = 1;
    [SerializeField, Min(1)] private int cycleLengthInMeasures = 1;
    [Range(0f, 1f)] [SerializeField] private float offset = 0f;
    [SerializeField] private Sound actionToCall;
    public Sound ActionToCall => actionToCall;


    private double intervalLength;
    private double nextAudioTriggerTime;
    private double nextVisualTriggerTime;

    public float SpawnLeadTime { get; set; } = 0.5f;

    public void Init(float bpm, double dspStartTime)
    {
        double measureDuration = 60.0 / bpm * 4;
        double cycleDuration = measureDuration * cycleLengthInMeasures;
        intervalLength = cycleDuration / steps;

        nextAudioTriggerTime = Math.Max(dspStartTime + offset * cycleDuration, AudioSettings.dspTime + 0.05);
        nextVisualTriggerTime = nextAudioTriggerTime - SpawnLeadTime;
    }

    public void UpdateBPM(float newBpm, double dspStartTime)
    {
        double measureDuration = 60.0 / newBpm * 4;
        double cycleDuration = measureDuration * cycleLengthInMeasures;
        intervalLength = cycleDuration / steps;

        double timeSinceStart = AudioSettings.dspTime - dspStartTime;
        double nextInterval = Math.Ceiling((timeSinceStart - offset * cycleDuration) / intervalLength);

        nextAudioTriggerTime = dspStartTime + offset * cycleDuration + nextInterval * intervalLength;
        nextVisualTriggerTime = nextAudioTriggerTime - SpawnLeadTime;
    }

    public void Update(double dspTime)
    {
        while (dspTime >= nextVisualTriggerTime)
        {
            NoteSpawner.onVisualNote?.Invoke(actionToCall);
            nextVisualTriggerTime += intervalLength;
        }

        while (dspTime >= nextAudioTriggerTime)
        {
            PulseToTheBeat.onSound?.Invoke(actionToCall);
            nextAudioTriggerTime += intervalLength;
        }
    }
}