// using UnityEngine;
// using UnityEditor;

// [CustomEditor(typeof(BeatManager))]
// public class BeatManagerEditor : Editor
// {
//     private Vector2 scroll;
//     private float stepHeight = 30f;
//     private float stepWidth = 25f;

//     public override void OnInspectorGUI()
//     {
//         serializedObject.Update();

//         BeatManager manager = (BeatManager)target;

//         // BPM et résolution
//         manager.bpm = EditorGUILayout.FloatField("BPM", manager.bpm);
//         manager.stepResolution = EditorGUILayout.FloatField("Step Resolution (beats)", manager.stepResolution);

//         if (manager.patterns.Count == 0)
//         {
//             if (GUILayout.Button("Ajouter un instrument"))
//             {
//                 manager.patterns.Add(new InstrumentPattern { instrumentName = "Instrument" + (manager.patterns.Count + 1) });
//             }
//             serializedObject.ApplyModifiedProperties();
//             return;
//         }

//         float totalDuration = manager.GetTotalDuration();
//         int totalSteps = Mathf.CeilToInt(totalDuration / manager.stepResolution);

//         scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(300));

//         for (int i = 0; i < manager.patterns.Count; i++)
//         {
//             var pattern = manager.patterns[i];
//             EditorGUILayout.BeginVertical("box");
//             EditorGUILayout.LabelField(pattern.instrumentName, EditorStyles.boldLabel);

//             Rect rowRect = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth, stepHeight);

//             for (int s = 0; s < totalSteps; s++)
//             {
//                 float stepTime = s * manager.stepResolution;
//                 Rect cellRect = new Rect(10 + s * stepWidth, rowRect.y, stepWidth, stepHeight - 5);

//                 bool stepExists = pattern.steps.Exists(step => Mathf.Abs(step.time - stepTime) < 0.0001f);

//                 if (GUI.Button(cellRect, stepExists ? "●" : ""))
//                 {
//                     if (stepExists)
//                     {
//                         // Supprimer le step
//                         pattern.steps.RemoveAll(step => Mathf.Abs(step.time - stepTime) < 0.0001f);
//                     }
//                     else
//                     {
//                         // Ajouter un step
//                         pattern.steps.Add(new StepData { time = stepTime });
//                     }
//                     EditorUtility.SetDirty(manager);
//                 }
//             }

//             EditorGUILayout.EndVertical();
//             GUILayout.Space(10);
//         }

//         EditorGUILayout.EndScrollView();

//         if (GUILayout.Button("Ajouter un instrument"))
//         {
//             manager.patterns.Add(new InstrumentPattern { instrumentName = "Instrument" + (manager.patterns.Count + 1) });
//             EditorUtility.SetDirty(manager);
//         }

//         serializedObject.ApplyModifiedProperties();
//     }
// }
