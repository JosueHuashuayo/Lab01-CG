using System;
using UnityEngine;
using System.Collections;
using InfimaGames.LowPolyShooterPack;
using Random = UnityEngine.Random;

public class Projectile : MonoBehaviour {

	[Range(5, 100)]
	[Tooltip("After how long time should the bullet prefab be destroyed?")]
	public float destroyAfter;
	[Tooltip("If enabled the bullet destroys on impact")]
	public bool destroyOnImpact = false;
	[Tooltip("Minimum time after impact that the bullet is destroyed")]
	public float minDestroyTime;
	[Tooltip("Maximum time after impact that the bullet is destroyed")]
	public float maxDestroyTime;

	[Header("Impact Effect Prefabs")]
	public Transform [] bloodImpactPrefabs;
	public Transform [] metalImpactPrefabs;
	public Transform [] dirtImpactPrefabs;
	public Transform []	concreteImpactPrefabs;

	[Header("Custom Sounds")]
	public AudioClip collisionSound;
    private float maxVolume = 1f; // Volumen máximo del sonido

    [Range(1, 10)]
    private float maxDistance = 1f;

	private Rigidbody rb;

    private void Start ()
	{
		//Grab the game mode service, we need it to access the player character!
		var gameModeService = ServiceLocator.Current.Get<IGameModeService>();
        //Ignore the main player character's collision. A little hacky, but it should work.
        // Physics.IgnoreCollision(gameModeService.GetPlayerCharacter().GetComponent<Collider>(), GetComponent<Collider>());

        //Start destroy timer
        rb = GetComponent<Rigidbody>();

        StartCoroutine (DestroyAfter ());
	}

	//If the bullet collides with anything
	private void OnCollisionEnter (Collision collision)
	{
		//Ignore collisions with other projectiles.
		//if (collision.gameObject.GetComponent<Projectile>() != null)
		//	return;
		if (rb.velocity.magnitude > 1)
		{

			float distance = Vector3.Distance(transform.position, collision.contacts[0].point);

			// Calcula el volumen basado en la distancia usando una función logarítmica
			float distanceVolume = Mathf.Clamp01(1f - Mathf.Log10(distance) / Mathf.Log10(maxDistance));

			// Calcula el volumen basado en la velocidad del objeto
			float velocityVolume = 1f - rb.velocity.magnitude / rb.maxDepenetrationVelocity;

			// Combina los volúmenes, dando mayor peso a la distancia
			float volume = Mathf.Lerp(distanceVolume, velocityVolume, 0.5f) * maxVolume;

			// Reproduce el sonido en la posición de la colisión con el volumen calculado
			AudioSource.PlayClipAtPoint(collisionSound, collision.contacts[0].point, volume);
		}
        // //Ignore collision if bullet collides with "Player" tag
        // if (collision.gameObject.CompareTag("Player")) 
        // {
        // 	//Physics.IgnoreCollision (collision.collider);
        // 	Debug.LogWarning("Collides with player");
        // 	//Physics.IgnoreCollision(GetComponent<Collider>(), GetComponent<Collider>());
        //
        // 	//Ignore player character collision, otherwise this moves it, which is quite odd, and other weird stuff happens!
        // 	Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        //
        // 	//Return, otherwise we will destroy with this hit, which we don't want!
        // 	return;
        // }
        //
        //If destroy on impact is false, start 
        //coroutine with random destroy timer
        if (!destroyOnImpact) 
		{
			StartCoroutine (DestroyTimer ());
		}
		//Otherwise, destroy bullet on impact
		else 
		{
			Destroy (gameObject);
		}

		//If bullet collides with "Blood" tag
		if (collision.transform.tag == "Blood") 
		{
			//Instantiate random impact prefab from array
			Instantiate (bloodImpactPrefabs [Random.Range 
				(0, bloodImpactPrefabs.Length)], transform.position, 
				Quaternion.LookRotation (collision.contacts [0].normal));
			//Destroy bullet object
			Destroy(gameObject);
		}

		//If bullet collides with "Metal" tag
		if (collision.transform.tag == "Metal") 
		{
			//Instantiate random impact prefab from array
			Instantiate (metalImpactPrefabs [Random.Range 
				(0, bloodImpactPrefabs.Length)], transform.position, 
				Quaternion.LookRotation (collision.contacts [0].normal));
			//Destroy bullet object
			Destroy(gameObject);
		}

		//If bullet collides with "Dirt" tag
		if (collision.transform.tag == "Dirt") 
		{
			//Instantiate random impact prefab from array
			Instantiate (dirtImpactPrefabs [Random.Range 
				(0, bloodImpactPrefabs.Length)], transform.position, 
				Quaternion.LookRotation (collision.contacts [0].normal));
			//Destroy bullet object
			Destroy(gameObject);
		}

		//If bullet collides with "Concrete" tag
		//if (collision.transform.tag == "Concrete") 
		//{
		//	//Instantiate random impact prefab from array
		//	Instantiate (concreteImpactPrefabs [Random.Range 
		//		(0, bloodImpactPrefabs.Length)], transform.position, 
		//		Quaternion.LookRotation (collision.contacts [0].normal));
		//	//Destroy bullet object
		//	Destroy(gameObject);
		//}
		if (collision.transform.tag == "Dron")
		{
			//Instantiate random impact prefab from array
			Instantiate(concreteImpactPrefabs[Random.Range
				(0, bloodImpactPrefabs.Length)], transform.position,
				Quaternion.LookRotation(collision.contacts[0].normal));
			//Destroy bullet object
			//Destroy(gameObject);
		}

		//If bullet collides with "Target" tag
		if (collision.transform.tag == "Target") 
		{
			//Toggle "isHit" on target object
			collision.transform.gameObject.GetComponent
				<TargetScript>().isHit = true;
			//Destroy bullet object
			Destroy(gameObject);
		}
			
		//If bullet collides with "ExplosiveBarrel" tag
		if (collision.transform.tag == "ExplosiveBarrel") 
		{
			//Toggle "explode" on explosive barrel object
			collision.transform.gameObject.GetComponent
				<ExplosiveBarrelScript>().explode = true;
			//Destroy bullet object
			Destroy(gameObject);
		}

		//If bullet collides with "GasTank" tag
		if (collision.transform.tag == "GasTank") 
		{
			//Toggle "isHit" on gas tank object
			collision.transform.gameObject.GetComponent
				<GasTankScript> ().isHit = true;
			//Destroy bullet object
			Destroy(gameObject);
		}
	}

	private IEnumerator DestroyTimer () 
	{
		yield return new WaitForSeconds
			(Random.Range(minDestroyTime, maxDestroyTime));
        Instantiate(concreteImpactPrefabs[Random.Range
        		(0, bloodImpactPrefabs.Length)], transform.position, Quaternion.LookRotation(Vector3.up));
        Destroy(gameObject);
	}

	private IEnumerator DestroyAfter () 
	{
		//Wait for set amount of time
		yield return new WaitForSeconds (destroyAfter);
        Instantiate(concreteImpactPrefabs[Random.Range
                (0, bloodImpactPrefabs.Length)], transform.position, Quaternion.LookRotation(Vector3.up));
        Destroy (gameObject);
	}
}