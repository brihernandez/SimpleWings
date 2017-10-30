using UnityEngine;

// TODO:
// Executes in edit mode so that you cannot enter a wing X/Y of zero. The proper way
// to do this would be with a custom inspector but those things are a huge hassle.
[ExecuteInEditMode]
public class SimpleWing : MonoBehaviour
{
	[Tooltip("Size of the wing. The bigger the wing, the more lift it provides.")]
	public Vector2 dimensions = Vector2.one;
	public float WingArea { get { return dimensions.x * dimensions.y; } }

	[Tooltip("When true, wing forces will be applied only at the center of mass.")]
	public bool applyForceToOrigin = false;

	[Tooltip("Angle of attack at which wing generates the most lift.")]
	public float criticalAngleOfAttack = 18.0f;
	[Tooltip("Force per m^2 the wing will provide when at the optimal angle of attack.")]
	public float liftPerMeterSquared = 1.0f;
	[Tooltip("Lift that comes \"for free\" at zero angle of attack. Zero is recommended for control surfaces such as rudders and elevators.")]
	public float neutralLiftCoefficient = 0.6f;
	[Tooltip("How much drag the wing puts out.")]
	public float dragCoeff = 0.0f;

	[Header("Read Only")]
	[SerializeField]
	private float wingArea;

	const float FORCE_MULT = 0.001f;

	private Rigidbody rigid;

	private float liftForce;
	private float dragForce;
	private float angleOfAttack;

	public float AngleOfAttack
	{
		get
		{
			Vector3 localVelocity = transform.InverseTransformDirection(rigid.velocity);
			return angleOfAttack * ((localVelocity.y > 0.0f) ? -1.0f : 1.0f);
		}
	}

	public Rigidbody Rigidbody
	{
		set { rigid = value; }
	}

	private void Awake()
	{
		// I don't especially like doing this, but there are many cases where wings might not
		// have the rigidbody on themselves (e.g. they are on a child gameobject of a plane).
		rigid = GetComponentInParent<Rigidbody>();
	}

	private void Start()
	{
		if (rigid == null)
		{
			Debug.LogWarning(name + ": SimpleWing has no rigidbody on self or parent!");
		}
	}

	private void Update()
	{
		// Prevent division by zero.
		if (dimensions.x <= 0.0f)
			dimensions.x = 0.01f;
		if (dimensions.y <= 0.0f)
			dimensions.y = 0.01f;

		// TODO: Assigned only for the debug stuff. A custom inspector should be used for this.
		wingArea = WingArea;

		// DEBUG
		if (rigid != null)
		{
			Debug.DrawRay(transform.position, transform.up * liftForce * 0.001f, Color.blue);
			Debug.DrawRay(transform.position, -rigid.velocity.normalized * dragForce * 0.001f, Color.red);
		}
	}

	private void FixedUpdate()
	{
		if (rigid != null)
		{
			Vector3 forceApplyPos = (applyForceToOrigin) ? rigid.transform.position : transform.position;

			Vector3 localVelocity = transform.InverseTransformDirection(rigid.velocity);
			localVelocity.x = 0.0f;

			// Wing generates most lift when it reaches the specified angle of attack.
			angleOfAttack = Vector3.Angle(Vector3.forward, localVelocity);

			// Angle always returns a positive value, so add the sign back in.
			float AoaComponent = Mathf.InverseLerp(0.0f, criticalAngleOfAttack, angleOfAttack);
			AoaComponent *= (localVelocity.y > 0.0f) ? -1.0f : 1.0f;

			// Lift comes from speed^2 * angle of attack * optimalLiftForce.
			liftForce = localVelocity.z * localVelocity.z * AoaComponent * liftPerMeterSquared * WingArea * FORCE_MULT;
			liftForce += neutralLiftCoefficient * liftPerMeterSquared;

			// Apply lift component.
			rigid.AddForceAtPosition(transform.up * liftForce, forceApplyPos, ForceMode.Force);

			// Apply a drag proportional to the angle of attack and area of the control surface visible.
			dragForce = rigid.velocity.sqrMagnitude * WingArea * angleOfAttack * Mathf.Sin(angleOfAttack * Mathf.Deg2Rad) * dragCoeff;
			rigid.AddForceAtPosition(-rigid.velocity.normalized * dragForce, forceApplyPos, ForceMode.Force);
		}
	}

	private void OnDrawGizmos()
	{
		Matrix4x4 oldMatrix = Gizmos.matrix;

		Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
		Gizmos.DrawWireCube(Vector3.zero, new Vector3(dimensions.x, 0.0f, dimensions.y));

		Gizmos.matrix = oldMatrix;
	}
}