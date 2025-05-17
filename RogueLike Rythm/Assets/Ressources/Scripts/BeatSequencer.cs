// using System.Collections.Generic;
// using UnityEngine;

// public class BeatSequencer : MonoBehaviour
// {
//     public float bpm = 120f;

//     // 4 steps par mesure, 2 mesures => 8 steps = 2 secondes à 120 BPM
//     public int stepsPerMeasure = 4;
//     public int numberOfMeasures = 2;

//     public List<InstrumentPattern> instrumentPatterns = new List<InstrumentPattern>
//     {
//         new InstrumentPattern { instrumentName = "Kick" },
//         new InstrumentPattern { instrumentName = "Snare" },
//         new InstrumentPattern { instrumentName = "HiHat" },
//         new InstrumentPattern { instrumentName = "Clap" }
//     };

//     public float GetTotalDuration()
//     {
//         int totalSteps = stepsPerMeasure * numberOfMeasures;
//         return (60f / bpm) * totalSteps;
//     }
// }
