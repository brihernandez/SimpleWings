//
// Copyright (c) Brian Hernandez. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

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