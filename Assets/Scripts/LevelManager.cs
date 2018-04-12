using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelManager : MonoBehaviour
{

    [SerializeField] private float waitToRespawn;
    public int coinsCount;
    public Text textCoins;
    public Player thePlayer;
    

    // Use this for initialization
    void Start()
    {
        thePlayer = FindObjectOfType<Player>();
        textCoins.text = "Coins: " + coinsCount;
    }

    // Utilizada para somar todas as moedas coletadas no jogo
    public void AddCoin(int coinToAdd)
    {
        coinsCount += coinToAdd;
        textCoins.text = "Coins: " + coinsCount;
    }


}
