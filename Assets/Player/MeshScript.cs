using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshScript : MonoBehaviour
{
    public PlayerScript player;
    private IPlayerCollision playerCollision;
    // Start is called before the first frame update
    void Start()
    {
        playerCollision = (IPlayerCollision)player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.tag == "JumpObstacle")
        {
            Debug.Log(collider.gameObject.tag);
            playerCollision.OnJumpObstacleCollision(false);
        }
        if (collider.gameObject.tag == "SlideObstacle") {
            Debug.Log(collider.gameObject.tag);
            playerCollision.OnSlideObstacleCollision(false);
        }
        if (collider.gameObject.tag == "JumpSlideObstacle") {
            Debug.Log(collider.gameObject.tag);
            playerCollision.OnFullObstacleCollision(false);
        }
        if (collider.gameObject.tag == "collectible") {
            Debug.Log(collider.gameObject.tag);
            playerCollision.OnCollectibleCollision();
            collider.gameObject.transform.localScale = new Vector3(0, 0, 0);
        }

    }

    void OnTriggerExit(Collider collider) {

        if (collider.gameObject.tag == "JumpObstacle") {
            Debug.Log(collider.gameObject.tag);
            playerCollision.OnJumpObstacleCollision(true);
        }
        if (collider.gameObject.tag == "SlideObstacle") {
            Debug.Log(collider.gameObject.tag);
            playerCollision.OnSlideObstacleCollision(true);
        }
        if (collider.gameObject.tag == "JumpSlideObstacle") {
            Debug.Log(collider.gameObject.tag);
            playerCollision.OnFullObstacleCollision(true);
        }
        if (collider.gameObject.tag == "collectible") {
            Debug.Log(collider.gameObject.tag);
            playerCollision.OnCollectibleCollision();
            collider.gameObject.transform.localScale = new Vector3(0, 0, 0);
        }

    }
}
