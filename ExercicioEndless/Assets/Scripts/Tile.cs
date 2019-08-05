using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int lane;
    public float speed = 0.05f;
    Game gameRef;

    // Start is called before the first frame update
    void Start()
    {
        gameRef = GameObject.FindGameObjectWithTag("Game").GetComponent<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, -speed, 0);

        if (gameRef.tiles.Count == gameRef.maxQueue)
        {
            speed += 0.0001f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Parede"))
        {
            gameRef.respawn();
        }
    }
}
