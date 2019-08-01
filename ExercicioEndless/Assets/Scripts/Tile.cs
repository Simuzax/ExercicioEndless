using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int lane;
    float speed = 5f;
    Game gameRef;

    // Start is called before the first frame update
    void Start()
    {
        gameRef = GameObject.FindGameObjectWithTag("Game").GetComponent<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, (float)-0.05, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Parede"))
        {
            gameRef.respawn();
        }
    }
}
