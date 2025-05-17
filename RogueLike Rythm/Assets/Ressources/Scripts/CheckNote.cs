using UnityEngine;

public class CheckNote : MonoBehaviour
{
    [Header("Zone des notes early")]
    [SerializeField] private Vector2 offsetEarly;
    [SerializeField] private Vector2 sizeEarly;
    [SerializeField] private Color colorEarly;

    [Space(10)]
    [Header("Zone des notes perfect")]
    [SerializeField] private Vector2 offsetPerfect;
    [SerializeField] private Vector2 sizePerfect;
    [SerializeField] private Color colorPerfect;

    [Space(10)]

    [Header("Zone des notes late")]
    [SerializeField] private Vector2 offsetLate;
    [SerializeField] private Vector2 sizeLate;
    [SerializeField] private Color colorLate;

    [Space(10)]

    [Header("Zone de suppression des notes ratées")]
    [SerializeField] private Vector2 offsetMiss;
    [SerializeField] private Vector2 sizeMiss;
    [SerializeField] private Color colorMiss = Color.gray;

    [Space(10)]

    [Header("Feedback Sprites")]
    [SerializeField] private GameObject perfectSpritePrefab;
    [SerializeField] private GameObject earlySpritePrefab;
    [SerializeField] private GameObject lateSpritePrefab;
    [SerializeField] private GameObject missSpritePrefab;

    [Space(10)]
    [SerializeField] private LayerMask noteLayerMask;

    [Space(10)]
    [SerializeField] private ComboManager comboManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) CheckNotes(Sound.Kick);
        if (Input.GetKeyDown(KeyCode.F)) CheckNotes(Sound.HiHat);
        if (Input.GetKeyDown(KeyCode.J)) CheckNotes(Sound.Snare);
        if (Input.GetKeyDown(KeyCode.K)) CheckNotes(Sound.Clap);
    }

    void LateUpdate()
    {
        CheckForMissedNotes();
    }

    private void CheckForMissedNotes()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(
            transform.position + new Vector3(offsetMiss.x, offsetMiss.y, 0),
            sizeMiss,
            0,
            noteLayerMask
        );

        foreach (Collider2D hit in hits)
        {
            Note note = hit.GetComponent<Note>();
            if (note != null)
            {
                Destroy(note.gameObject);// Si aucune note détectée = MISS
                comboManager.RegisterHit(NoteRating.Miss);
                ShowFeedback(missSpritePrefab);
                Debug.Log($"MISS by despawn: {note.soundType}");
            }
        }
    }

    private NoteRating CheckNotes(Sound noteToCheck)
    {
        if (TryGetMostAdvancedNote(noteToCheck, offsetPerfect, sizePerfect, out Collider2D perfectNote))
        {
            ShowFeedback(perfectSpritePrefab);
            Destroy(perfectNote.gameObject);
            comboManager.RegisterHit(NoteRating.Perfect);
            Debug.Log("PERFECT");
            return NoteRating.Perfect;
        }

        if (TryGetMostAdvancedNote(noteToCheck, offsetEarly, sizeEarly, out Collider2D earlyNote))
        {
            ShowFeedback(earlySpritePrefab);
            Destroy(earlyNote.gameObject);
            comboManager.RegisterHit(NoteRating.Early);
            Debug.Log("EARLY");
            return NoteRating.Early;
        }

        if (TryGetMostAdvancedNote(noteToCheck, offsetLate, sizeLate, out Collider2D lateNote))
        {
            ShowFeedback(lateSpritePrefab);
            Destroy(lateNote.gameObject);
            comboManager.RegisterHit(NoteRating.Late);
            Debug.Log("LATE");
            return NoteRating.Late;
        }

        // Si aucune note détectée = MISS
        ShowFeedback(missSpritePrefab);
        comboManager.RegisterHit(NoteRating.Miss);
        Debug.Log("MISS");
        return NoteRating.Miss;

    }

    private void ShowFeedback( GameObject prefab)
    {
        if (prefab == null) return;

        Instantiate(prefab, transform.position, Quaternion.identity);
    }

    private bool TryGetMostAdvancedNote(Sound soundToCheck, Vector2 offset, Vector2 size, out Collider2D matchedNote)
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(
            transform.position + new Vector3(offset.x, offset.y, 0),
            size,
            0,
            noteLayerMask
        );

        matchedNote = null;
        bool firstValueIsChecked = false;
        float highestX = 1;

        foreach (Collider2D hit in hits)
        {
            Note note = hit.GetComponent<Note>();

            if (note != null && note.soundType == soundToCheck)
            {
                //Debug.Log("note != null && note.soundType == soundToCheck");

                float xPos = hit.transform.position.x;

                if (!firstValueIsChecked)
                {
                    highestX = xPos;
                    firstValueIsChecked = true;
                }

                if (xPos >= highestX) // Le plus bas = le plus avancé si les notes descendent
                    {
                        highestX = xPos;
                        matchedNote = hit;
                    }
            }
        }

        return matchedNote != null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = colorEarly;
        Gizmos.DrawWireCube(transform.position + new Vector3(offsetEarly.x, offsetEarly.y, 0), sizeEarly);

        Gizmos.color = colorPerfect;
        Gizmos.DrawWireCube(transform.position + new Vector3(offsetPerfect.x, offsetPerfect.y, 0), sizePerfect);

        Gizmos.color = colorLate;
        Gizmos.DrawWireCube(transform.position + new Vector3(offsetLate.x, offsetLate.y, 0), sizeLate);

        Gizmos.color = colorMiss;
        Gizmos.DrawWireCube(transform.position + new Vector3(offsetMiss.x, offsetMiss.y, 0), sizeMiss);
    }
}

public enum NoteRating
{
    Miss,
    Early,
    Perfect,
    Late
}