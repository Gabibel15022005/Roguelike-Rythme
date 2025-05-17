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

    [Space(10)]
    [SerializeField] private BeatManager beatManager;

    public static Action<Sound> onVisualNote;

    private void Awake()
    {
        float distance = Vector3.Distance(kickSpawn.position, targetPoint.position);
        float travelTime = distance / 5f; // Valeur de base pour init, corrigée à chaque visual spawn

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

        float distance = Vector3.Distance(spawnPoint.position, targetPoint.position);

        // Récupérer le SpawnLeadTime (valeur constante pour ce son)
        float leadTime = 0.5f;
        foreach (var interval in beatManager.Intervals)
        {
            if (interval.ActionToCall == sound)
            {
                leadTime = interval.SpawnLeadTime;
                break;
            }
        }

        float speed = distance / leadTime;

        GameObject note = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity);
        NoteMover mover = note.GetComponent<NoteMover>();

        if (mover != null)
        {
            mover.InitMovement(speed);
        }
    }
}
