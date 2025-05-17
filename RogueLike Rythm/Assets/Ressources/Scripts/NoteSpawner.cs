using System;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [SerializeField] private GameObject kickNotePrefab;
    [SerializeField] private GameObject snareNotePrefab;
    [SerializeField] private GameObject hihatNotePrefab;
    [SerializeField] private GameObject clapNotePrefab;

    [Space(30)]
    [SerializeField] private Transform kickSpawn;
    [SerializeField] private Transform snareSpawn;
    [SerializeField] private Transform hihatSpawn;
    [SerializeField] private Transform clapSpawn;

    [Space(30)]
    [SerializeField] private Transform targetPoint;

    [Space(30)]
    [SerializeField] private BeatManager beatManager;

    [SerializeField] private float noteSpeed = 5f; // en unités/sec

    public static Action<Sound> onVisualNote;

    private void Awake()
    {
        float distance = Vector3.Distance(kickSpawn.position, targetPoint.position);
        float travelTime = distance / noteSpeed;

        foreach (var interval in beatManager.Intervals)
        {
            interval.SpawnLeadTime = travelTime;
        }
    }

    private void OnEnable()
    {
        onVisualNote += HandleVisualNote;
    }

    private void OnDisable()
    {
        onVisualNote -= HandleVisualNote;
    }

    private void HandleVisualNote(Sound sound)
    {
        Transform spawnPoint = sound switch
        {
            Sound.Kick => kickSpawn,
            Sound.Snare => snareSpawn,
            Sound.HiHat => hihatSpawn,
            Sound.Clap => clapSpawn,
            _ => null
        };

        GameObject notePrefab = sound switch
        {
            Sound.Kick => kickNotePrefab,
            Sound.Snare => snareNotePrefab,
            Sound.HiHat => hihatNotePrefab,
            Sound.Clap => clapNotePrefab,
            _ => null
        };

        if (spawnPoint == null || notePrefab == null) return;

        GameObject note = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity);
        NoteMover mover = note.GetComponent<NoteMover>();

        if (mover != null)
        {
            float distance = Vector3.Distance(spawnPoint.position,
                new Vector3(targetPoint.position.x, spawnPoint.position.y, spawnPoint.position.z));

            float travelTime = distance / noteSpeed;

            mover.MoveTo(targetPoint.position, travelTime);
        }
    }
}
