using UnityEngine.UI;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour {

	[SerializeField]
	private Text healthText;

	[SerializeField]
	private Text speedText;

	[SerializeField]
	private float healthMultiplier = 1.3f;
	[SerializeField]
	private float speedMultiplier = 1.3f;

	[SerializeField]
	private int upgradeCost = 50;

	private PlayerStatus playerStats;

	private void Awake()
	{
		playerStats = PlayerStatus.instance;
	}

	private void OnEnable()
	{
		UpdateValues();
	}

	void UpdateValues()
	{
		healthText.text = "HEALTH: " + playerStats.maxHealth.ToString();
		speedText.text = "SPEED: " + playerStats.moveSpeed.ToString();
	}

	public void UpgradeHealth()
	{
		if(GameMaster.Money < upgradeCost)
		{
			AudioManager.instance.PlaySound("NoMoney");
			return;
		}
		playerStats.maxHealth = (int)(playerStats.maxHealth * healthMultiplier);

		GameMaster.Money -= upgradeCost;
		AudioManager.instance.PlaySound("Money");

		UpdateValues();
	}

	public void UpgradeSpeed()
	{
		if (GameMaster.Money < upgradeCost)
		{
			AudioManager.instance.PlaySound("NoMoney");
			return;
		}
		playerStats.moveSpeed = Mathf.Round(playerStats.moveSpeed * speedMultiplier);
		AudioManager.instance.PlaySound("Money");
		GameMaster.Money -= upgradeCost;
		UpdateValues();

	}
}
