using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterPlantScript : MonoBehaviour {
	#region Variables to assign via the unity inspector (SerializeFields).
	[SerializeField]
	private float towerRange = 3.0f;

	[SerializeField]
	[Range(0.0f, 1.0f)]
	private float turnSpeed = 0.5f;

	[Header("Make the firing range 1 to require the plant to be on target before shooting.")]
	[SerializeField]
	[Range(0.0f, 1.0f)]
	private float firingRange = 0.5f;

	[SerializeField]
	private float bulletSpeed = 1.0f;

	[SerializeField]
	private int bulletsPerSec = 2;

	[SerializeField]
	private GameObject bulletPrefab;

	[SerializeField]
	private Transform spriteTransform;
	#endregion

	#region Private Variable Declarations.
	private Vector3 targetPos = Vector3.zero;

	private Vector3 targetPosLastFrame = Vector3.zero;
	private bool coolingDown = false;
	#endregion

	#region Private Functions.
	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		//Find Targets.
		targetPos = FindTarget();

		//If the target is in range.
		if (IsTargetInRange() && targetPos != Vector3.zero) {
			//Rotate the sprite towards the target.
			spriteTransform.up = Vector3.Lerp(spriteTransform.up, (targetPos - this.transform.position).normalized, turnSpeed);

			float dot = Vector3.Dot((targetPos - this.transform.position).normalized.normalized, spriteTransform.up);
			if (!coolingDown && dot >= firingRange) {
				//Spawn a bullet.
				GameObject bullet = Instantiate(bulletPrefab, this.transform.position, Quaternion.identity, this.transform);

				//Make it move towards the target.
				bullet.GetComponent<Rigidbody>().velocity = spriteTransform.up * bulletSpeed;
				bullet.transform.up = spriteTransform.up;

				//Start cooldown.
				coolingDown = true;
				StartCoroutine(ShootCooldown());
			}
		}
	}

	private void OnDrawGizmos() {
		//Draw Range.
		Gizmos.color = Color.black;
		Gizmos.DrawWireSphere(this.transform.position, towerRange);
	}

	private IEnumerator ShootCooldown() {
		yield return new WaitForSeconds(1 / bulletsPerSec);
		coolingDown = false;
	}

	private Vector3 FindTarget() {
		Vector3 returnVal = Vector3.zero;
		GameObject[] ants = GameObject.FindGameObjectsWithTag("Ant");
		//Find the smallest distance.
		Vector3 towerPos = this.transform.position;
		Vector3 towerUp = this.transform.up;
		float smallestDistance = float.MaxValue;
		int ival = 0;
		for (int i = 0; i < ants.Length; i++) {
			//Get the distance between the ant and that defence point.
			float newDistance = (ants[i].transform.position - towerPos).magnitude;
			if (newDistance < smallestDistance) {
				smallestDistance = newDistance;
				ival = i;
				Rigidbody rb = ants[ival].GetComponent<Rigidbody>();
				returnVal = ants[i].transform.position;
				returnVal += rb.velocity;
			}
		}

		return returnVal;
	}

	private bool IsTargetInRange() {
		//Find the distance to the target.
		float distance = (targetPos - this.transform.position).magnitude;

		//Return whether or not it is in range.
		return distance <= towerRange;
	}
	#endregion

	#region Public Access Functions.

	#endregion
}
