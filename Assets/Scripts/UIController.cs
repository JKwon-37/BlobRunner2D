using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;

public class UIController : MonoBehaviour
{
    //showing leaderboard things
    public PlayFabManager playfabManager;

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
        if(GameObject.Find("stats") != null){
            playfabManager = GameObject.Find("stats").GetComponent<PlayFabManager>();
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
        GetLeaderboard();
        //errorText.style.display = DisplayStyle.Flex;
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "PlatformScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (var item in result.Leaderboard)
        {
            Debug.Log($"{item.PlayFabId} {item.StatValue}");
        }
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error while getting stats");
        Debug.Log(error.GenerateErrorReport());
    }
}
