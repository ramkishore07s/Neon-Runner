using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour, IPlayerControl, IPlayerCollision
{
    public float playerSpeed;
    private float distToGround;

    public new BoxCollider collider;

    public List<AudioClip> audioClips;
    public Animator laneAnimator;
    public Animator playerAnimator;

    public AudioSource audioSource;

    private bool update = false;
    private bool move_camera_back = false;

    public UIScript uIScript;
    private UiInterface ui;

    public new GameObject camera;



    // Start is called before the first frame update
    void Start() {
        distToGround = collider.bounds.extents.y;
        audioSource = GetComponent<AudioSource>();
        ui = (UiInterface)uIScript;
        Time.timeScale = 1;
        ui.ShowMenu();
    }

    // Update is called once per frame
    void Update() {
        if (update) {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + playerSpeed * Time.deltaTime);
            Time.timeScale += 0.005f * Time.deltaTime;
            ui.UpdateDistance(0);
        }

        if (move_camera_back) {
            camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, camera.transform.position.z -  4 * Time.deltaTime);
        }
   }

    public void StartPlayer() {
        update = true;
        playerAnimator.SetTrigger("start");
        ui.HideMenu();
    }


    void FixedUpdate() {
    }

    void IPlayerControl.onDownAction() {
        if (update) {
            playerAnimator.SetTrigger("down");
            audioSource.PlayOneShot(audioClips[1]);
        }
    }

    void IPlayerControl.onLeftAction() {
        if (update) {
            laneAnimator.SetTrigger("left");
            audioSource.PlayOneShot(audioClips[6]);
        }
    }

    void IPlayerControl.onRightAction() {
        if (update) {
            laneAnimator.SetTrigger("right");
            audioSource.PlayOneShot(audioClips[6]);
        }
    }

    void IPlayerControl.onUpAction() {
        if (update) {
            playerAnimator.SetTrigger("jump");
            audioSource.PlayOneShot(audioClips[0]);
        } 
    }

    private bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);

    }

    private void OnCollision(bool on_exit) {
        Debug.Log("Collided");
        Time.timeScale = 1;
        move_camera_back = true;
        //Time.timeScale = 0;
        update = false;
        if (on_exit) {
            playerAnimator.SetTrigger("fall_front");
        } else {
            playerAnimator.SetTrigger("fell");
        }
        audioSource.PlayOneShot(audioClips[2]);
        audioSource.PlayOneShot(audioClips[3]);
        StartCoroutine(Restart());
    }

    private IEnumerator Restart() {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
    }

    void IPlayerCollision.OnCollectibleCollision() {
        ui.OnCoinCollected();
        audioSource.PlayOneShot(audioClips[5]);
    }

    void IPlayerCollision.OnJumpObstacleCollision(bool on_exit) {
        if( !playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("jump") ) {
            //playerAnimator.SetTrigger("fall_front");
            OnCollision(on_exit);
        }
    }

    void IPlayerCollision.OnSlideObstacleCollision(bool on_exit) {
        if (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("slide")) {
            OnCollision(false);
        }
    }

    void IPlayerCollision.OnJumpOrSlideObstacleCollision(bool on_exit) {
        if (! (
            playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("slide") ||
            playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("jump")
            )) {
            OnCollision(false);
        }
    }


    void IPlayerCollision.OnFullObstacleCollision(bool on_exit) {
        OnCollision(false);
    }
}
