using UnityEngine;

public class LookAt : MonoBehaviour
{
	public Transform lookAt;

	private void Update()
	{
		if (lookAt != null)
		{
			transform.LookAt(lookAt);
		}
	}
}