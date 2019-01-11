//
// Copyright (c) Brian Hernandez. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

using UnityEngine;

public class Engine : MonoBehaviour
{
	[Range(0, 1)]
	public float throttle = 1.0f;

	[Tooltip("How much power the engine puts out.")]
	public float thrust;

	private Rigidbody rigid;

	private void Awake()
	{
		rigid = GetComponentInParent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		if (rigid != null)
		{
			rigid.AddRelativeForce(Vector3.forward * thrust * throttle, ForceMode.Force);
		}
	}

}
