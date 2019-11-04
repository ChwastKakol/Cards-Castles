using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool playerOneTurn = true;
    public float cameraLerpTime = .4f;

    public GameObject player1;
    public GameObject player2;

    PlayerController player1Controller;
    PlayerController player2Controller;

    private Transform player1Camera;
    private Transform player2Camera;

    Camera camera;

    private void Start()
    {
        camera = Camera.main;
        player1Controller = player1.GetComponent<PlayerController>();
        player2Controller = player2.GetComponent<PlayerController>();

        player1Camera = player1Controller.GetCameraTransform();
        player2Camera = player2Controller.GetCameraTransform();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
            SwapTurn();

        if (playerOneTurn)
        {
            player1Controller.Turn();
        }
        else
        {
            player2Controller.Turn();
        }
    }

    public void SwapTurn()    
    {
        playerOneTurn = !playerOneTurn;
        if (playerOneTurn)
        {
            StartCoroutine(MoveCamera(player2Camera));
        }
        else
        {
            StartCoroutine(MoveCamera(player1Camera));
        }
    }

    IEnumerator MoveCamera(Transform destination)
    {
        float time = 0;
        while(time < cameraLerpTime)
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, destination.position, time / cameraLerpTime);
            camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, destination.rotation, time / cameraLerpTime);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
