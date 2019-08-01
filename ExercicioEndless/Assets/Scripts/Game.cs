using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public GameObject fundo;
    public GameObject tile;

    /* Array spawns[5]
    lane = random(0, 4);
    position = spawns[lane].position;
    */

    [SerializeField]
    int maxQueue;

    Queue<Tile> tiles = new Queue<Tile>();

    [SerializeField]
    Vector2 distanceBetweenTiles;

    [SerializeField]
    Vector2 firstPosition;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(fundo);
    }

    // Start is called before the first frame update
    void Start()
    {
        distanceBetweenTiles.x += tile.GetComponent<SpriteRenderer>().bounds.size.x;

        for (int i = 0; i < maxQueue; i++)
        {
            Tile t = Instantiate(tile, Vector2.zero, Quaternion.identity).GetComponent<Tile>();

            Vector2 position = firstPosition;

            t.lane = Random.Range(0, 4);

            position.x += t.lane * distanceBetweenTiles.x;

            position.y += i * distanceBetweenTiles.y;

            t.transform.position = position;

            tiles.Enqueue(t);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void loadGame()
    {
        SceneManager.LoadScene("Jogo");
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void respawn()
    {
        //Dequeue, Enqueue
    }
}
