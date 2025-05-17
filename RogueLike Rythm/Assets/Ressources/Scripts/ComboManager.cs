using TMPro;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    [SerializeField] private TMP_Text comboText;

    private int currentCombo = 0;

    public void RegisterHit(NoteRating rating)
    {
        if (rating == NoteRating.Miss)
        {
            ResetCombo();
        }
        else
        {
            IncreaseCombo();
        }
    }

    private void IncreaseCombo()
    {
        currentCombo++;
        UpdateComboText();
    }

    private void ResetCombo()
    {
        currentCombo = 0;
        UpdateComboText();
    }

    private void UpdateComboText()
    {
        comboText.text = $"X {currentCombo}";
    }
}
