using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class TimerController : MonoBehaviour
{
    public static TimerController instance;
    
    public Label timeCounter;
        
    private TimeSpan timePlaying;
    private bool timerGoing;
    public float elapsedTime;

    private void Awake()
    {
        instance = this;
        VisualElement root =GameObject.FindWithTag("Timer").GetComponent<UIDocument>().rootVisualElement;
        timeCounter = root.Q<Label>("TimerText");
    }
    // Start is called before the first frame update
    private void Start()
    {
        timeCounter.text = "game-time";
        timerGoing = false;
        if(timerGoing == false && timeCounter.text == "game-time")
        {
            SceneManager.LoadScene("Level_One");
            BeginTimer();
        }
    }

    public void BeginTimer()
    {
        timerGoing = true;
        elapsedTime = 0f;
        StartCoroutine(UpdateTimer());
    }

    public void EndTimer()
    {
        Debug.Log("EndTimer Called prematurely");
        timerGoing = false;
    }

    private IEnumerator UpdateTimer()
    {
        while(timerGoing)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            string timePlayingStr = "Time: " + timePlaying.ToString("mm':'ss'.'ff");
            timeCounter.text = timePlayingStr;
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        timePlaying = TimeSpan.FromSeconds(elapsedTime);
        string timePlayingStr = "Time: " + timePlaying.ToString("mm':'ss'.'ff");
        timeCounter.text = timePlayingStr;
    }
}
