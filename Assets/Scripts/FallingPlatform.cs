using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private float fallDelay = 1f;

    private Vector3 respawnPoint;
    public GameObject respawnPlatform;

    [SerializeField] private Rigidbody2D rb;

    void Start()
    {
        respawnPoint = transform.position;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "DestroyPlatform")
        {
            rb.bodyType = RigidbodyType2D.Static;
            transform.position = respawnPoint;
        }
    }



    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;


    }

}
