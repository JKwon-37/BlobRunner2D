using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public Button startButton;
    public Button loginButton;
    public Label errorText;
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        startButton = root.Q<Button>("StartButton");
        loginButton = root.Q<Button>("Login");
        errorText = root.Q<Label>("Errors");

        startButton.clicked += StartButtonPressed;
        loginButton.clicked += LoginButtonPressed;
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
}
