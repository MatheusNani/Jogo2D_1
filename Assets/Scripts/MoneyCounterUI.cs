using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Text))]
public class MoneyCounterUI : MonoBehaviour
{

	[SerializeField]
	private Text MoneyText;

	// Use this for initialization
	void Awake()
	{

		MoneyText = GetComponent<Text>();

	}

	// Update is called once per frame
	void Update()
	{
		MoneyText.text = "MONEY: " + GameMaster.Money.ToString();

	}
}
