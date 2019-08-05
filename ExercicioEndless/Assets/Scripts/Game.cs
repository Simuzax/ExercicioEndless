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
    Transform[] spawns = new Transform[4];

    //Tiles
    public Queue<Tile> tiles = new Queue<Tile>();

    //Lanes
    [SerializeField]
    public int maxQueue;
    int maxLane = 4;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Jogo")
        {
            GameObject[] spawnsGO = GameObject.FindGameObjectsWithTag("spawnPoints");

            for(int i = 0; i < spawnsGO.Length;i++)
            {
                if (i >= spawns.Length)
                    break;

                spawns[i] = spawnsGO[i].transform;
            }
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(fundo);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void spawn()
    {
        if (tiles.Count >= maxQueue)
        {
            return;
        }

        int lane = Random.Range(0, maxLane);

        Vector3 position = spawns[lane].position;

        Tile t = Instantiate(tile, position, Quaternion.identity).GetComponent<Tile>();

        t.lane = lane;

        tiles.Enqueue(t);
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= timer + timerMax && tiles.Count < maxQueue)
        {
            timer = Time.time;

            spawn();
        }

        foreach (Tile t in tiles)
        {
            t.speed = tiles.Peek().speed;
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
        tiles.Enqueue(t);
    }
}
