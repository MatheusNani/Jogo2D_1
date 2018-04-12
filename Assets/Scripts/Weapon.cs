using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public float firerate = 0;
	public int damage = 10;
	public LayerMask notToHit;

	public Transform BulletTrailPrefab;
	public Transform hitPrefab;
	public float effectSpawnRate = 10f;
	public Transform muzzleFlashPrefab;

	// Handle camera shaking
	public float camShakeAmount = 0.05f;
	public float camShakeLength = 0.1f;
	private CameraShake camShake;

	public string weaponShotSound = "DefaultShot";

	private float timeToSpawnEffect;
	private float timeToFire = 0;
	private Transform firePoint;

	//caching
	AudioManager audioManager;

	// Use this for initialization
	void Awake()
	{
		firePoint = transform.Find("FirePoint");
		if (firePoint == null)
		{
			Debug.LogError("No firepoint = WHAT?");
		}
	}

	private void Start()
	{
		camShake = GameMaster.gm.GetComponent<CameraShake>();
		if (camShake == null)
		{
			Debug.LogError("No CameraShake script found on GM object.");
		}

		audioManager = AudioManager.instance;
		if(audioManager == null)
		{
			Debug.LogError("No audioManager found in scene!");
		}
	}

	// Update is called once per frame
	void Update()
	{

		if (firerate == 0)
		{
			if (Input.GetButtonDown("Fire1"))
			{
				Shoot("GunShot");				
			}
		}
		else
		{
			if (Input.GetButton("Fire1") && Time.time > timeToFire)
			{
				timeToFire = Time.time + 1 / firerate;
				Shoot("GunShot");				
			}
		}
	}

	void Shoot(string _shotAudio)
	{
		Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
		Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
		RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100, ~notToHit);

		//Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition) * 100, Color.cyan);

		if (hit.collider != null)
		{
			//Debug.DrawLine(firePointPosition, hit.point, Color.red);            
			Enemy enemy = hit.collider.GetComponent<Enemy>();
			if (enemy != null)
			{
				enemy.DamageEnemy(damage);
				//Debug.Log("We hit " + hit.collider.name + "and did " + damage + " damage");
			}
		}

		if (Time.time >= timeToSpawnEffect)
		{
			Vector3 hitPosition;
			Vector3 hitNormal;

			if (hit.collider == null)
			{
				hitPosition = (mousePosition - firePointPosition) * 30;
				hitNormal = new Vector3(9999, 9999, 9999);
			}
			else
			{
				hitPosition = hit.point;
				hitNormal = hit.normal;
			}

			Effect(hitPosition, hitNormal);
			timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
		}
		
	}

	void Effect(Vector3 hitPosition, Vector3 hitNormal)
	{
		Transform trail = Instantiate(BulletTrailPrefab, firePoint.position, firePoint.rotation) as Transform;
		LineRenderer lr = trail.GetComponent<LineRenderer>();

		if (lr != null)
		{
			lr.SetPosition(0, firePoint.position);
			lr.SetPosition(1, hitPosition);
		}

		Destroy(trail.gameObject, 0.04f);

		if (hitNormal != new Vector3(9999, 9999, 9999))
		{
			Transform hitParticle = Instantiate(hitPrefab, hitPosition, (Quaternion.FromToRotation(Vector3.right, hitNormal))) as Transform;
			Destroy(hitParticle.gameObject, 1f);
		}

		Transform clone = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
		clone.parent = firePoint;
		float size = Random.Range(0.2f, 0.4f);
		clone.localScale = new Vector3(size, size, 0);
		Destroy(clone.gameObject, 0.02f);

		//Shake the camera
		camShake.Shake(camShakeAmount, camShakeLength);

		// play shot sound
		audioManager.PlaySound(weaponShotSound);
	}
}
