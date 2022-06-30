using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;

public class UIController : MonoBehaviour
{
    //showing leaderboard things

    public PlayFabManager playfabManager;

    //start game ui
    public Button registerButton;
    public Button loginButton;
    public Button resetPasswordButton;

    public Label errorText;

    //end game ui
    public Button replayButton;
    public Button leaderboardButton;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        registerButton = root.Q<Button>("RegisterButton");
        loginButton = root.Q<Button>("LoginButton");
        resetPasswordButton = root.Q<Button>("ResetPasswordButton");
        errorText = root.Q<Label>("Errors");

        if(loginButton != null){
            registerButton.clicked += RegisterButtonPressed;
            loginButton.clicked += LoginButtonPressed;
            //resetPasswordButton.clicked += ResetPasswordButtonPressed;
        }

        replayButton = root.Q<Button>("Replay");
        leaderboardButton = root.Q<Button>("Leaderboard");

        if(replayButton != null){
            replayButton.clicked += ReplayButtonPressed;
            leaderboardButton.clicked += LeaderboardButtonPressed;
        }
        if(GameObject.Find("stats") != null){
            playfabManager = GameObject.Find("stats").GetComponent<PlayFabManager>();
            playfabManager.usernameInput = root.Q<TextField>("UsernameInput");
            playfabManager.emailInput = root.Q<TextField>("EmailInput");
            playfabManager.passwordInput = root.Q<TextField>("PasswordInput");
        }
    }

    void RegisterButtonPressed()
    {
        var request = new RegisterPlayFabUserRequest();
            request.Email = playfabManager.emailInput.text;
            request.Password = playfabManager.passwordInput.text;
            request.Username = playfabManager.usernameInput.text;
            request.DisplayName = playfabManager.usernameInput.text;
        Debug.Log($"{request.Username} registered");
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
        //SceneManager.LoadScene("Level_One");
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result){
        errorText.text = "Registered and logged in, will redirect to start game";
        SceneManager.LoadScene("Level_One");
    }

        void OnLoginSuccess(LoginPlayFabUserResult result){
        errorText.text = "Registered and logged in, will redirect to start game";
        SceneManager.LoadScene("Level_One");
    void LoginButtonPressed()
    {
        var request = new LoginWithEmailAdressRequest();
            request.Email = playfabManager.emailInput.text;
            request.Password = playfabManager.passwordInput;
        PlayFabClientAPI.LoginWithEmailAdressRequest(request, OnLoginSuccess, OnError);
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
