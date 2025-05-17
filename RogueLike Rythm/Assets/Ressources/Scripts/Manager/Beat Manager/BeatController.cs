using TMPro;
using UnityEngine;

public class BeatController : MonoBehaviour
{
    public BeatManager beatManager;
    [SerializeField] TMP_Text bpmText;

    private void Start()
    {
        UpdateBPMUi();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            beatManager.Play();
            Debug.Log("Play");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            beatManager.Pause();
            Debug.Log("Pause");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            beatManager.Stop();
            Debug.Log("Stop");
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            beatManager.SetBPM(beatManager.BPM + 10f);
            UpdateBPMUi();
            Debug.Log("Increase BPM");
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            beatManager.SetBPM(beatManager.BPM - 10f);
            UpdateBPMUi();
            Debug.Log("Decrease BPM");
        }
    }

    private void UpdateBPMUi()
    {
        if (bpmText == null) return;
        bpmText.text = $"Actual BPM : {beatManager.BPM}";
    }
}
