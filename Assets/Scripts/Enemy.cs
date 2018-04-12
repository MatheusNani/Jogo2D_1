using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class Enemy : MonoBehaviour {

    [System.Serializable]
    public class EnemyStats
    {
        public int maxHealth = 100;
        //  public float startPercentHealth = 1f; //  set % to health Bar

        private int _currentHealth;
        public int CurrentHealth
        {
            get { return _currentHealth; }
            set { _currentHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public int damage = 40;      

        public void Init()
        {
            CurrentHealth = maxHealth; // set % to health Bar(* startPercentHealth)
        }

    }

    public EnemyStats stats = new EnemyStats();

    public Transform deathParticles;

    public float shakeAmount = 0.1f;
    public float shakeLength = 0.1f;

	public string deathSoundName = "Explosion";

	public int moneyDrop = 10;

	[Header("Option:")]
    [SerializeField]
    private StatusIndicator statusIndicator;

    private void Start()
    {       
        stats.Init();
        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.CurrentHealth, stats.maxHealth);
        }

		//subscribe the method to be call on Delegate
		GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

        if(deathParticles == null)
        {
            Debug.LogError("No death particles referenced on Enemy");
        }

    }

	void OnUpgradeMenuToggle(bool active)
	{
		GetComponent<EnemyAI>().enabled = !active;
	}

    public void DamageEnemy(int damage)
    {
        stats.CurrentHealth -= damage;
        if (stats.CurrentHealth <= 0)
        {
            GameMaster.KillEnemy(this);          
        }

        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.CurrentHealth, stats.maxHealth);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerStatus _player = collision.collider.GetComponent<PlayerStatus>();

        if(_player != null)
        {
            _player.DamagePlayer(stats.damage);
            DamageEnemy(99999);
        }
    }

	// call after destroy the Enemy to remove from de Delegate
	private void OnDestroy()
	{
		GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
	}

}
