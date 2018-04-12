using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
	public static PlayerStatus instance;

	public int maxHealth = 100;
	private int _currentHealth;
	public int CurrentHealth
	{
		get { return _currentHealth; }
		set { _currentHealth = Mathf.Clamp(value, 0, maxHealth); }
	}

	public int fallBoundry = -20;

	public string deathSoundName = "DeathVoice";
	public string damageSoundName = "Grunt";

	private AudioManager audioManager;

	[SerializeField]
	private StatusIndicator statusIndicator;

	private PlayerStatus stats;
	public float healthRegenRate = 2f;
	public float moveSpeed = 10f;

	public void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}		
	}


	private void Start()
	{
		stats = PlayerStatus.instance;

		stats.CurrentHealth = stats.maxHealth;

		if (statusIndicator == null)
		{
			Debug.LogError("No status indicator referenced on Player");
		}
		else
		{
			statusIndicator.SetHealth(stats.CurrentHealth, stats.maxHealth);
		}

		audioManager = AudioManager.instance;
		if (audioManager == null)
		{
			Debug.LogError("No audioManager in scene. ");
		}

		InvokeRepeating("RegenHealth", 1f/stats.healthRegenRate, 1f/stats.healthRegenRate);
	}

	private void Update()
	{
		if (transform.position.y <= -20)
		{
			DamagePlayer(99999999);
		}
	}

	public void DamagePlayer(int damage)
	{
		stats.CurrentHealth -= damage;
		if (stats.CurrentHealth <= 0)
		{
			//Play death sound
			audioManager.PlaySound(deathSoundName);
			//kill player
			GameMaster.KillPlayer(this);

		}
		else
		{
			//play damage sound
			audioManager.PlaySound(damageSoundName);
		}

		if (statusIndicator != null)
		{
			statusIndicator.SetHealth(stats.CurrentHealth, stats.maxHealth);
		}

	}

	void RegenHealth()
	{		
		stats.CurrentHealth += 1;
		//update health bar
		statusIndicator.SetHealth(stats.CurrentHealth, stats.maxHealth);
	}

}
