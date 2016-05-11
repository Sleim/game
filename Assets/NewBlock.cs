using UnityEngine;
using System.Collections;

public class NewBlock : MonoBehaviour
{
    public int hitsToKill;
    public int points;
    private int numberOfHits;
    // Use this for initialization
    void Start()
    {
        numberOfHits = 0; //количество ударов

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            numberOfHits++;

            if (numberOfHits == hitsToKill)
            {
                // уничтожаем объект
                Destroy(this.gameObject);
            }
        }

        else if (collision.gameObject.tag == "Player")
        {
            numberOfHits++;

            if (numberOfHits == hitsToKill)
            {
                // уничтожаем объект
                Destroy(this.gameObject);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}

