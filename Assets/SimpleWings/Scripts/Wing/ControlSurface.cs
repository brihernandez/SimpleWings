using UnityEngine;


public class ControlSurface : MonoBehaviour
{
	[Header("Deflection")]
	[Tooltip("Deflection with max positive input at max control authority")]
	public float max = 30.0f;

	[Tooltip("Speed of the control surface deflection.")]
	public float moveSpeed = 90.0f;

	private Quaternion startLocalRotation = Quaternion.identity;

	private float deflection = 0.0f;
	private float angle = 0.0f;

	public float Deflection { set { deflection = value; } }

	private void Start()
	{
		// Dirty hack so that the rotation can be reset before applying the deflection.
		startLocalRotation = transform.localRotation;
	}

	private void FixedUpdate()
	{
		// Move the control surface.
		angle = Mathf.MoveTowards(angle, deflection * max, moveSpeed * Time.deltaTime);

		// Hacky way to do this!
		transform.localRotation = startLocalRotation;
		transform.Rotate(Vector3.right, angle, Space.Self);
	}

}