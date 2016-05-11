using UnityEngine;
using System.Collections;

public class Ball2 : MonoBehaviour
{
    private bool ballIsActive;
    private Vector3 ballPosition;
    private Vector2 ballInitialForce;
    public GameObject playerObject;
    // Use this for initialization
    void Start()
    {
        ballInitialForce = new Vector2(0.0f, 100.0f);   //сила
        ballIsActive = false;
        ballPosition = transform.position;  //положение
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") == true)   //нажимаем на Пробел = старт
        {
            if (!ballIsActive)
            {
                GetComponent<Rigidbody2D>().isKinematic = false;
                GetComponent<Rigidbody2D>().AddForce(ballInitialForce);   //добавляем силу

                ballIsActive = !ballIsActive;
            }

        }
        if (!ballIsActive && playerObject != null)
        {

            ballPosition.x = playerObject.transform.position.x; // зададим новую позицию шарика
            ballPosition.y = playerObject.transform.position.y;
            transform.position = ballPosition;
        }
        if (ballIsActive && transform.position.y < -6) // проверка смерти
        {
            ballIsActive = !ballIsActive;
            ballPosition.x = playerObject.transform.position.x;
            ballPosition.y = -4.2f;
            transform.position = ballPosition;

            GetComponent<Rigidbody2D>().isKinematic = true;
        }

    }
}
