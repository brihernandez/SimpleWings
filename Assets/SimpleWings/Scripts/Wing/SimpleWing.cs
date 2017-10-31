using UnityEngine;

// TODO:
// Executes in edit mode so that you cannot enter a wing X/Y of zero. The proper way
// to do this would be with a custom inspector but those things are a huge hassle.
[ExecuteInEditMode]
public class SimpleWing : MonoBehaviour
{
	[Tooltip("Size of the wing. The bigger the wing, the more lift it provides.")]
	public Vector2 dimensions = new Vector2(5.0f, 2.0f);
	public float WingArea { get { return dimensions.x * dimensions.y; } }

	[Tooltip("When true, wing forces will be applied only at the center of mass.")]
	public bool applyForceToOrigin = false;

	[Tooltip("Lift coefficient curve.")]
	public WingCurves wing;
	[Tooltip("Force per m^2 the wing will provide when at the optimal angle of attack.")]
	public float liftPerMeterSquared = 0.5f;
	[Tooltip("The higher the value, the more drag the wing incurs.")]
	public float dragMultiplier = 0.8f;

	[Header("Read Only")]
	[SerializeField]
	private float wingArea;

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
			Debug.LogError(name + ": SimpleWing has no rigidbody on self or parent!");
		}

		if (wing == null)
		{
			Debug.LogError(name + ": SimpleWing has no defined wing curves!");
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
		if (rigid != null && wing != null)
		{
			Vector3 forceApplyPos = (applyForceToOrigin) ? rigid.transform.TransformPoint(rigid.centerOfMass) : transform.position;

			Vector3 localVelocity = transform.InverseTransformDirection(rigid.velocity);
			localVelocity.x = 0.0f;

			// Wing generates most lift when it reaches the specified angle of attack.
			angleOfAttack = Vector3.Angle(Vector3.forward, localVelocity);
			float liftCoefficient = wing.GetLiftAtAaoA(angleOfAttack);
			float dragCoefficient = wing.GetDragAtAaoA(angleOfAttack);

			// Lift = speed^2 * lift coeffient * optimalLiftForce
			liftForce = localVelocity.sqrMagnitude * liftCoefficient * liftPerMeterSquared * WingArea;

			// Vector3.Angle always returns a positive value, so add the sign back in.
			liftForce *= -Mathf.Sign(localVelocity.y);

			// Apply lift component.
			rigid.AddForceAtPosition(transform.up * liftForce, forceApplyPos, ForceMode.Force);

			// Apply a drag proportional to the angle of attack and area of the control surface visible.
			dragForce = localVelocity.sqrMagnitude * WingArea * dragCoefficient * dragMultiplier;
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