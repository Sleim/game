using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour {
    public float playerVelocity;
    public float boundary;
    private Vector3 playerPosition;
    public Canvas canvas;
    public Text esc;
    
    // Use this for initialization
    void Start () {
        playerPosition = gameObject.transform.position; // позиция платформы
        canvas.GetComponent<Canvas>();
        canvas.enabled = false;
        esc.GetComponent<Text>();
        esc.enabled = false;
    }
    void WinLose()
    {
        // все блоки уничтожены
        if ((GameObject.FindGameObjectsWithTag("Block")).Length == 0) //когда дошли до телепорта - новый уровень
        {
            // проверяем текущий уровень
            if (Application.loadedLevelName == "Level1")
            {
                Application.LoadLevel("Level2");
            }
            else
            {
                Application.Quit();
            }
        }

        if ((GameObject.FindGameObjectsWithTag("Player")).Length == 0) 
        {
            canvas.enabled = true;
            Time.timeScale = 0;
            esc.enabled = true;
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Application.Quit();
            }

        }

    }
    // Update is called once per frame
    void Update () {
        playerPosition.x += Input.GetAxis("Horizontal") * playerVelocity; // движение по горизонтальни
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        transform.position = playerPosition;   // новая позиция платформы
        if (playerPosition.x < -boundary)     // проверка выхода за границы
        {
            transform.position = new Vector3(-boundary, playerPosition.y, playerPosition.z);
        }
        if (playerPosition.x > boundary)
        {
            transform.position = new Vector3(boundary, playerPosition.y, playerPosition.z);
        }
        WinLose();
    }
}
