
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	[SerializeField]
	private string mouseHouverSound = "ButtonHouver";
	[SerializeField]
	private string buttonPress = "ButtonPress";

	AudioManager audioManager;

	private void Start()
	{
		audioManager = AudioManager.instance;
		if(audioManager == null)
		{
			Debug.LogError("No audioManager Found !");
		}
	}


	public void StartGame()
    {
		audioManager.PlaySound(buttonPress);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
		audioManager.PlaySound(buttonPress);
		Debug.Log("WE QUIT THE GAME.");
        Application.Quit();
    }

	public void OnMouseOver()
	{
		audioManager.PlaySound(mouseHouverSound);
	}

}
