using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SimpleWing))]
[CanEditMultipleObjects]
public class SimpleWingEditor : Editor
{
	private bool showDebug = false;

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		SimpleWing wing = (SimpleWing)target;

		EditorGUILayout.Space();
		showDebug = EditorGUILayout.ToggleLeft("Show debug values", showDebug);

		if (showDebug)
		{
			//EditorGUILayout.BeginHorizontal();

			//EditorGUILayout.BeginVertical(GUILayout.MaxWidth(50)); // Labels
			//EditorGUILayout.LabelField("Wing Area: ");
			//EditorGUILayout.LabelField("Angle of Attack: ");
			//EditorGUILayout.LabelField("Lift Coefficent: ");
			//EditorGUILayout.LabelField("Lift Force: ");
			//EditorGUILayout.LabelField("Drag Coefficient: ");
			//EditorGUILayout.LabelField("Drag Force: ");
			//EditorGUILayout.EndVertical(); // End Labels

			//EditorGUILayout.BeginVertical(); // Values
			//EditorGUILayout.LabelField(wing.WingArea.ToString("0.00"));
			//EditorGUILayout.LabelField(wing.AngleOfAttack.ToString("0.00"));
			//EditorGUILayout.LabelField(wing.LiftCoefficient.ToString("0.00"));
			//EditorGUILayout.LabelField(wing.LiftForce.ToString("0.00"));
			//EditorGUILayout.LabelField(wing.DragCoefficient.ToString("0.00"));
			//EditorGUILayout.LabelField(wing.DragForce.ToString("0.00"));
			//EditorGUILayout.EndVertical(); // End Values

			//EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();
			EditorGUILayout.LabelField(string.Format("Wing Area: {0:0.00}", wing.WingArea));
			EditorGUILayout.LabelField(string.Format("Angle of Attack: {0:0.00}", wing.AngleOfAttack));

			EditorGUILayout.Space();
			EditorGUILayout.LabelField(string.Format("Lift Coefficient: {0:0.00}", wing.LiftCoefficient));
			EditorGUILayout.LabelField(string.Format("Lift Force: {0:0.00}", wing.LiftForce));

			EditorGUILayout.Space();
			EditorGUILayout.LabelField(string.Format("Drag Coefficient: {0:0.00}", wing.DragCoefficient));
			EditorGUILayout.LabelField(string.Format("Drag Force: {0:0.00}", wing.DragForce));

			if (Application.isPlaying)
			{
				Repaint();
			}
		}
	}
}

