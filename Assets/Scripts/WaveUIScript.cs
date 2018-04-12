using UnityEngine.UI;
using UnityEngine;

public class WaveUIScript : MonoBehaviour {

    [SerializeField]
    private WaveSpawner spawner;

    [SerializeField]
    private Animator waveAnimator;

    [SerializeField]
    private Text waveCountdownText;

    [SerializeField]
    private Text waveCountText;

    private WaveSpawner.SpawnState previousState;

	// Use this for initialization
	void Start () {

        if (spawner == null)
        {
            Debug.LogError("No Spawner Referenced!");
            this.enabled = false;
        }
        if (waveAnimator == null)
        {
            Debug.LogError("No waveAnimator Referenced!");
            this.enabled = false;
        }
        if (waveCountdownText == null)
        {
            Debug.LogError("No waveCountdownText Referenced!");
            this.enabled = false;
        }
        if (waveCountText == null)
        {
            Debug.LogError("No waveCountText Referenced!");
            this.enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        switch (spawner.State)
        {
            case WaveSpawner.SpawnState.COUNTING:
                UpdateCountingUI();
                break;
            case WaveSpawner.SpawnState.SPAWNING:
                UpdateSpawingUI();
                break;
        }

        previousState = spawner.State;
	}

    void UpdateCountingUI()
    {
        if(previousState != WaveSpawner.SpawnState.COUNTING) {

            waveAnimator.SetBool("WaveIncoming", false);
            waveAnimator.SetBool("WaveCountdown", true);
           // Debug.Log("COUNTING");
        }

        waveCountdownText.text = ((int)spawner.WaveCountdown).ToString();
        
    }
    void UpdateSpawingUI()
    {
        if (previousState != WaveSpawner.SpawnState.SPAWNING)
        {
            waveAnimator.SetBool("WaveCountdown", false);
            waveAnimator.SetBool("WaveIncoming", true);

            waveCountText.text = spawner.NextWave.ToString();

            //Debug.Log("SPAWNING");
        }
      
    }
}
