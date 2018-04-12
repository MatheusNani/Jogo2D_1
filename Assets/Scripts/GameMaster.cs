using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{

	public static GameMaster gm;

	[SerializeField]
	private int maxLives = 3;

	private static int _remainingLives;
	public static int RemainingLives
	{
		get { return _remainingLives; }
	}

	public Transform playerPrefab;
	public Transform spawnPoint;
	public int SpawnDelay = 2;
	public GameObject spawnPrefab;
	public string respawnCountdownSoundName = "RespawnCountdown";
	public string spawnSoundName = "Spawn";

	public string gameOverSoundName = "GameOver";

	public CameraShake cameraShake;

	[SerializeField]
	private GameObject gameOverUI;
	
	[SerializeField]
	private GameObject upgradeMenu;

	[SerializeField]
	private WaveSpawner waveSpawner;

	//used to stop the player movement,enemy and shot functions when using the upgrade menu.
	public delegate void UpgradeManuCallBack(bool active);
	public UpgradeManuCallBack onToggleUpgradeMenu;

	[SerializeField]
	private int startingMoney;
	public static int Money;

	//cache
	private AudioManager audioManager;

	private void Awake()
	{
		if (gm == null)
		{
			gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
		}
	}

	private void Start()
	{
		if (cameraShake == null)
		{
			Debug.LogError("No camera referenced in GameMaster");
		}

		_remainingLives = maxLives;

		Money = startingMoney;

		//caching
		audioManager = AudioManager.instance;
		if (audioManager == null)
		{
			Debug.LogError("No AudioManager found in the scene");
		}

	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.U))
		{
			ToggleUpgradeMenu();
		}
	}

	
	private void ToggleUpgradeMenu()
	{
		upgradeMenu.SetActive(!upgradeMenu.activeSelf);
		waveSpawner.enabled = !upgradeMenu.activeSelf;
		onToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf);



	} 

	public void EndGame()
	{
		audioManager.PlaySound(gameOverSoundName);
		Debug.Log("Game Over");
		gameOverUI.SetActive(true);
	}

	public IEnumerator _RespawnPlayer()
	{
		// play sound respawncountdown
		audioManager.PlaySound(respawnCountdownSoundName);
		yield return new WaitForSeconds(SpawnDelay);

		// play sound spawn
		audioManager.PlaySound(spawnSoundName);
		Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);


		GameObject clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) as GameObject;
		Destroy(clone, 3f);

	}

	public static void KillPlayer(PlayerStatus player)
	{
		Destroy(player.gameObject);

		_remainingLives -= 1;
		if (_remainingLives <= 0)
		{
			gm.EndGame();
		}
		else
		{
			gm.StartCoroutine(gm._RespawnPlayer());
		}

	}

	public static void KillEnemy(Enemy enemy)
	{
		gm._killEnemy(enemy);
	}
	public void _killEnemy(Enemy _enemy)
	{
		//Let's play some sound
		audioManager.PlaySound(_enemy.deathSoundName);

		// Gain some money
		Money += _enemy.moneyDrop;
		audioManager.PlaySound("Money");

		//Add Particles
		Transform _clone = Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity) as Transform;
		Destroy(_clone.gameObject, 5f);
		// Go camerashake
		cameraShake.Shake(_enemy.shakeAmount, _enemy.shakeLength);
		Destroy(_enemy.gameObject);
	}
}
