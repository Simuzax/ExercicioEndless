using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class Tile : MonoBehaviour
{
    //Timer
    [SerializeField]
    float timerMax;
    float timer;

    public int lane;
    
    Game gameRef;

    public double score;
    public float speed = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;

        gameRef = GameObject.FindGameObjectWithTag("Game").GetComponent<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, -speed, 0);

        if (Time.time >= timer + timerMax)
        {
            timer = Time.time;

            gameRef.Score++;
        }

        if (score % 5 == 0)
        {
            gameRef.Speed += 0.00001f;
        }
        
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null && hit.collider.transform == transform && isLowestTile() && GetComponent<SpriteRenderer>().isVisible)
            {
                gameRef.addToPool(this);
            }
        }
#endif

#if UNITY_ANDROID
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                if (GetComponent<BoxCollider2D>().bounds.Contains(touch.position) && isLowestTile() && GetComponent<SpriteRenderer>().isVisible)
                {
                    gameRef.addToPool(this);
                }
            }
        }
#endif
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Entry"))
        {
            gameRef.spawn();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Parede"))
        {
            gameRef.addToPool(this);
        }
    }

    bool isLowestTile()
    {
        //Achar por menor Y e que esteja Visivel
        GameObject[] gameList = System.Array.FindAll(GameObject.FindGameObjectsWithTag("Tile").OrderBy(t => t.transform.position.y).ToArray(), t => t.GetComponent<SpriteRenderer>().isVisible); ;

        return gameList.First() == this.gameObject;
    }
}
