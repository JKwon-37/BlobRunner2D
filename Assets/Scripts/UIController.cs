using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    //start game ui
    public Button startButton;
    public Button loginButton;
    public Label errorText;

    //end game ui
    public Button replayButton;
    public Button leaderboardButton;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        startButton = root.Q<Button>("StartButton");
        loginButton = root.Q<Button>("Login");
        errorText = root.Q<Label>("Errors");

        if(startButton != null){
            startButton.clicked += StartButtonPressed;
            loginButton.clicked += LoginButtonPressed;
        }

        replayButton = root.Q<Button>("Replay");
        leaderboardButton = root.Q<Button>("Leaderboard");

        if(replayButton != null){
            replayButton.clicked += ReplayButtonPressed;
            leaderboardButton.clicked += LeaderboardButtonPressed;
        }
    }

    void StartButtonPressed()
    {
        SceneManager.LoadScene("Level_One");
    }

    void LoginButtonPressed()
    {
        errorText.text = "Login button pressed";
        errorText.style.display = DisplayStyle.Flex;
    }

    void ReplayButtonPressed()
    {
        SceneManager.LoadScene("Level_One");
    }

    void LeaderboardButtonPressed()
    {
        errorText.text = "Leaderboard button pressed";
        errorText.style.display = DisplayStyle.Flex;
    }
}
