//
// Copyright (c) Brian Hernandez. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

using UnityEngine;

/// <summary>
/// Shoots a rigidbody forwards with an inital velocity.
/// </summary>
public class Shooter : MonoBehaviour
{
	[Tooltip("Launch speed in m/s")]
	public float launchSpeed = 0.0f;

	[Tooltip("Launches forwards on start. When false, use \"Launch\" function to launch.")]
	public bool launchOnStart = true;

	private Rigidbody rigid;

	private void Awake()
	{
		rigid = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		if (launchOnStart)
		{
			Launch();
		}
	}

	[ContextMenu("Launch!")]
	public void Launch()
	{
		if (rigid != null)
		{
			rigid.AddRelativeForce(Vector3.forward * launchSpeed, ForceMode.VelocityChange);
		}
	}

	public void Launch(float speed)
	{
		launchSpeed = speed;
		Launch();
	}
}