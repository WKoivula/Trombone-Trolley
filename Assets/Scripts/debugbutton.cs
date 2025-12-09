using UnityEngine;

public class debugbutton : MonoBehaviour
{
	public string message = "Button clicked";
	private CanvasGroup canvasGroup;
	private UnityEngine.UI.Button button;

	private void Awake()
	{
		// Get or add CanvasGroup for visibility control
		canvasGroup = GetComponent<CanvasGroup>();
		if (canvasGroup == null)
		{
			canvasGroup = gameObject.AddComponent<CanvasGroup>();
		}

		// Get button component
		button = GetComponent<UnityEngine.UI.Button>();
		if (button == null)
		{
			// Try to find button in children
			button = GetComponentInChildren<UnityEngine.UI.Button>();
		}
	}

	private void Start()
	{
		UpdateButtonVisibility();
	}

	private void Update()
	{
		// Update button visibility based on game state
		UpdateButtonVisibility();
	}

	private void UpdateButtonVisibility()
	{
		if (GameManager._instance == null) return;

		// Show button in Start or GameOver states, hide during Playing
		bool shouldBeVisible = GameManager._instance.currentState == GameManager.GameState.Start ||
		                       GameManager._instance.currentState == GameManager.GameState.GameOver;

		if (canvasGroup != null)
		{
			canvasGroup.alpha = shouldBeVisible ? 1f : 0f;
			canvasGroup.interactable = shouldBeVisible;
			canvasGroup.blocksRaycasts = shouldBeVisible;
		}
		else
		{
			// Fallback: use GameObject.SetActive
			gameObject.SetActive(shouldBeVisible);
		}
	}

	public void OnClick()
	{
		Debug.Log(message);

		if (GameManager._instance == null)
		{
			Debug.LogWarning("GameManager instance not found!");
			return;
		}

		// Start or restart the game
		if (GameManager._instance.currentState == GameManager.GameState.Start ||
		    GameManager._instance.currentState == GameManager.GameState.GameOver)
		{
			GameManager._instance.StartGame();
		}
	}
}

