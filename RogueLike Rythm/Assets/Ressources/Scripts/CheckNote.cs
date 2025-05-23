using UnityEngine;

public class CheckNote : MonoBehaviour
{
    [SerializeField] private Vector2 offsetEarly;
    [SerializeField] private Vector2 sizeEarly;
    [SerializeField] private Color colorEarly;

    [Space(10)]
    [SerializeField] private Vector2 offsetPerfect;
    [SerializeField] private Vector2 sizePerfect;
    [SerializeField] private Color colorPerfect;

    [Space(10)]
    [SerializeField] private Vector2 offsetLate;
    [SerializeField] private Vector2 sizeLate;
    [SerializeField] private Color colorLate;

    [Space(10)]
    [SerializeField] private LayerMask noteLayerMask;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) CheckNotes(Sound.Kick);
        if (Input.GetKeyDown(KeyCode.F)) CheckNotes(Sound.HiHat);
        if (Input.GetKeyDown(KeyCode.K)) CheckNotes(Sound.Snare);
        if (Input.GetKeyDown(KeyCode.J)) CheckNotes(Sound.Clap);
    }

    private NoteRating CheckNotes(Sound noteToCheck)
    {
        // PERFECT
        if (TryGetMostAdvancedNote(noteToCheck, offsetPerfect, sizePerfect, out Collider2D perfectNote))
        {
            Destroy(perfectNote.gameObject);
            Debug.Log("PERFECT");
            return NoteRating.Perfect;
        }

        // EARLY
        if (TryGetMostAdvancedNote(noteToCheck, offsetEarly, sizeEarly, out Collider2D earlyNote))
        {
            Destroy(earlyNote.gameObject);
            Debug.Log("EARLY");
            return NoteRating.Early;
        }

        // LATE
        if (TryGetMostAdvancedNote(noteToCheck, offsetLate, sizeLate, out Collider2D lateNote))
        {
            Destroy(lateNote.gameObject);
            Debug.Log("LATE");
            return NoteRating.Late;
        }

        Debug.Log("MISS");
        return NoteRating.Miss;
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
    }
}

public enum NoteRating
{
    Miss,
    Early,
    Perfect,
    Late
}