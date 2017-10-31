using UnityEngine;

[CreateAssetMenu(fileName = "New Wing Curves", menuName = "Wings/Wing Curves", order = 99)]
public class WingCurves : ScriptableObject
{
	[TextArea]
	public string description;

	[SerializeField]
	private AnimationCurve lift = new AnimationCurve();

	[SerializeField]
	private AnimationCurve drag = new AnimationCurve();

	/// <summary>
	/// Returns the lift coefficient at a given angle of attack.
	/// Expected range is 0 to 180.
	/// </summary>
	public float GetLiftAtAaoA(float aoa)
	{
		return lift.Evaluate(aoa);
	}

	/// <summary>
	/// Returns the drag coefficient at a given angle of attack.
	/// Expected range is 0 to 180.
	/// </summary>
	public float GetDragAtAaoA(float aoa)
	{
		return drag.Evaluate(aoa);
	}
}
