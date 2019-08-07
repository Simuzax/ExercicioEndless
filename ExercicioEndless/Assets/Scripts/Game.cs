using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class Game : MonoBehaviour
{
    //Timer
    [SerializeField]
    float timerMax;
    float timer;

    //Não Remover
    public GameObject fundo;
    public GameObject tile;

    //Spawns
    [SerializeField]
    Transform[] spawns = new Transform[4];

    //Tiles
    [SerializeField]
    public Queue<Tile> tiles = new Queue<Tile>();

    //Lanes
    [SerializeField]
    public int maxQueue;
    int maxLane = 4;

    [SerializeField]
    bool inMenu;

    //Speed
    [SerializeField]
    float speed_;
    public float Speed
    {
        get
        {
            return speed_;
        }
        set
        {
            speed_ = value;
            foreach(GameObject t in GameObject.FindGameObjectsWithTag("Tile"))
            {
                t.GetComponent<Tile>().speed = speed_;
            }
        }
    }

    //Troca de Cena e Referência de SpawnPoints
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Jogo")
        {
            inMenu = false;

            GameObject[] spawnsGO = GameObject.FindGameObjectsWithTag("spawnPoints");

            for(int i = 0; i < spawnsGO.Length;i++)
            {
                if (i >= spawns.Length)
                {
                    break;
                }   

                spawns[i] = spawnsGO[i].transform;

                Vector3 pos = spawns[i].position;
                pos.y += tile.GetComponent<SpriteRenderer>().bounds.size.y / 2;

                spawns[i].position = pos;
            }
            spawn();
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(fundo);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void spawn()
    {
        if (tiles.Count > 0)
        {
            respawn();
        }
        else
        {
            int lane = Random.Range(0, maxLane);

            if (!spawns[lane])
            {
                return;
            }

            Vector2 position = spawns[lane].position;

            Tile t = Instantiate(tile, position, Quaternion.identity).GetComponent<Tile>();

            t.lane = lane;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (inMenu == false)
        {
            if (Time.time >= timer + timerMax && tiles.Count < maxQueue)
            {
                timer = Time.time;
            }

            foreach (Tile t in tiles)
            {
                t.speed = tiles.Peek().speed;
            }
        }
    }

    //Menu Start
    public void loadGame()
    {
        SceneManager.LoadScene("Jogo");
    }

    //Menu Exit
    public void exitGame()
    {
        Application.Quit();
    }

    //Pooling dos Tiles
    public void respawn()
    {
        int lane = Random.Range(0, maxLane);
        Tile t = tiles.Dequeue();

        while (lane == t.lane)
        {
            lane = Random.Range(0, maxLane);
        }

        t.lane = lane;

        t.transform.position = spawns[t.lane].position;

        t.GetComponent<BoxCollider2D>().enabled = true;

        t.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void addToPool(Tile t)
    {
        if (tiles.Count < maxQueue)
        {
            t.GetComponent<BoxCollider2D>().enabled = false;

            t.GetComponent<SpriteRenderer>().enabled = false;

            tiles.Enqueue(t);
        }
        else
        {
            Destroy(t.gameObject);
        }
    }
}
