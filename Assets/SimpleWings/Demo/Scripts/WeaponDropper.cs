//
// Copyright (c) Brian Hernandez. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

using UnityEngine;

/// <summary>
/// Drops a weapon. This class is very hastily written for the sake of the demo.
/// </summary>
public class WeaponDropper : MonoBehaviour
{
	public Weapon weaponPrefab;

	public float reloadTime = 4.0f;

	Weapon wep;
	private float cooldown = 0.0f;

	private void Update()
	{
		cooldown -= Time.deltaTime;

		if (cooldown <= 0.0f && wep == null && weaponPrefab != null)
		{
			SpawnWeapon();
		}		
	}

	public void Fire(Vector3 velocity)
	{
		if (wep != null && Input.GetButtonDown("Fire3"))
		{
			cooldown = reloadTime;
			wep.transform.SetParent(null);
			wep.Fire(velocity + (-transform.up * 5.0f));
			wep = null;
		}
	}

	private void SpawnWeapon()
	{
		wep = Instantiate(weaponPrefab, transform);
		wep.transform.localPosition = Vector3.zero;
		wep.transform.localRotation = Quaternion.identity;
	}

}