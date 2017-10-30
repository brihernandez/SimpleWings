using UnityEngine;

[CreateAssetMenu(fileName = "New Lift Curve", menuName = "Wings/Lift Curve", order = 99)]
public class LiftCurve : ScriptableObject
{
	public AnimationCurve liftCurve;
}
