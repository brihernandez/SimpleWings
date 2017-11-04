using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(WingCurves))]
public class WingCurvesEditor : Editor
{
	private float neutralLift = 0.0f;
	private float maxLiftPositive = 1.1f;
	private float minLiftPositive = 0.6f;
	private float negativeAoaMult = 1.0f;
	private float flatPlateMult = 1.0f;
	private float criticalAngle = 16.0f;
	private float fullyStalledAngle = 20.0f;

	const string kNeutralLiftDesc = "Lift generated when the wing as it zero angle of attack.";
	const string kCriticalDesc = "Critical angle of attack is both the angle at which the wing starts to stall, and the angle at which it produces the most lift.";
	const string kMaxLiftPosDesc = "Lift coeffient at a positive, critical angle of attack, when the wing is generating the most lift.";
	const string kMinLiftPosDesc = "Lift coeffient when the wing is fully stalled at a positive angle of attack.";
	const string kNegAoaMultDesc = "Multiplier for lift generated before stall when at negative angles of attack.";
	const string kFlatPlateMultDesc = "Multiplier for the flat plate lift that occurs for any wing from 45 to 135 degrees of rotation.";
	const string kFullStallDesc = "Angle of attack at which the wing is fully stalled and producing the minimum lift.";

	const float kflatPlateMax = 0.9f;

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		WingCurves curve = (WingCurves)target;

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Quick generate lift curve", EditorStyles.boldLabel);

		neutralLift = EditorGUILayout.FloatField(new GUIContent("Lift at Zero AOA: ", kNeutralLiftDesc), neutralLift);
		maxLiftPositive = EditorGUILayout.FloatField(new GUIContent("Lift at Critical AOA: ", kMaxLiftPosDesc), maxLiftPositive);
		minLiftPositive = EditorGUILayout.FloatField(new GUIContent("Lift when Fully Stalled: ", kMinLiftPosDesc), minLiftPositive);

		EditorGUILayout.Space();
		criticalAngle = EditorGUILayout.Slider(new GUIContent("Critical AOA: ", kCriticalDesc), criticalAngle, 1.0f, 35.0f);
		fullyStalledAngle = EditorGUILayout.Slider(new GUIContent("Fully Stalled AOA: ", kFullStallDesc), fullyStalledAngle, 2.0f, 44.0f);

		EditorGUILayout.Space();
		flatPlateMult = EditorGUILayout.FloatField(new GUIContent("Flat Plate Lift Multiplier: ", kFlatPlateMultDesc), flatPlateMult);
		negativeAoaMult = EditorGUILayout.FloatField(new GUIContent("Negative AOA Multiplier: ", kNegAoaMultDesc), negativeAoaMult);

		// Error checking. Prevent keys from going in out of order and try to maintain a sorta normal line.
		if (fullyStalledAngle < criticalAngle)
			fullyStalledAngle = criticalAngle + 0.1f;

		if (neutralLift > maxLiftPositive)
			neutralLift = maxLiftPositive - 0.01f;

		flatPlateMult = Mathf.Clamp(flatPlateMult, 0.0f, 100.0f);

		// Build graph
		List<Keyframe> keyList = new List<Keyframe>(9)
		{
			// Wing at positive AOA.
			new Keyframe(0.0f, neutralLift),
			new Keyframe(criticalAngle, maxLiftPositive),
			new Keyframe(fullyStalledAngle, minLiftPositive),

			// Flat plate, generic across all wings.
			new Keyframe(45.0f, kflatPlateMax * flatPlateMult),
			new Keyframe(90.0f, 0.0f),
			new Keyframe(135.0f, -kflatPlateMax * flatPlateMult),

			// Wing at negative AOA.
			new Keyframe(180.0f - fullyStalledAngle, -minLiftPositive * negativeAoaMult),
			new Keyframe(180.0f - criticalAngle, -maxLiftPositive * negativeAoaMult),
			new Keyframe(180.0f, neutralLift)
		};

		bool shouldGenerate = GUILayout.Button("Generate lift curve (Will overwrite lift curve!)");
		if (shouldGenerate)
		{
			curve.SetLiftCurve(keyList.ToArray());
			Repaint();
		}
	}
}