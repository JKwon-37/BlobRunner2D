using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTutorial : MonoBehaviour
{
    public GameObject textObj;
    public GameObject triggerObj;

    // Start is called before the first frame update
    void Start()
    {
        textObj.SetActive(false);   
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            textObj.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {}

     void OnTriggerExit2D(Collider2D col)
    {
        textObj.SetActive(false);
        Destroy(triggerObj);
    }
}
