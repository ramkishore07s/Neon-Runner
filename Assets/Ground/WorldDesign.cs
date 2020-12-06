using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldDesign : MonoBehaviour
{
    public List<GameObject> Grounds;

    private System.Random random = new System.Random();

    public GameObject SlideObstaclePrefab;
    public GameObject JumpObstaclePrefab;
    public GameObject JumpSlideObstaclePrefab;
    public GameObject CollectiblePrefab;
    public GameObject LongJumpSlideObstacle;
    public GameObject LongJumpObstacle;

    public GameObject Bus;
    public AudioClip busSound;

    public int BoardLength;
    public GameObject Player;

    public List<GameObject> gameObjects;

    public List<float> GroundUpdatePositions;

    public List<GameObject> dynamicObjects;
    public List<int> dynamicObjectVelocities;

    private int leastCount = 0;
    private int dynamicLeastCount = 0;

    private int start_length = 4;
    private float time = 0;


    private int least = 0;

    private int freeLaneCount = 0;
    private int normalCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i=1; i<5; i++) {
            UpdateThingsOnGround(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGround();
        time += Time.deltaTime * 0.01f;
        start_length += (int) time;

        for (int i=dynamicLeastCount; i<dynamicObjects.Count; i++) {
            dynamicObjects[i].transform.position += new Vector3(0, 0, dynamicObjectVelocities[i] * Time.deltaTime);
        }

    }

    void UpdateGround() {
        if (Player.transform.position.z - 50 > Grounds[least].transform.position.z) {
            Grounds[least].transform.position = new Vector3(Grounds[least].transform.position.x, Grounds[least].transform.position.y, Grounds[least].transform.position.z + 500);
            if (random.Next(0, 5) < 3) {
                if (normalCount > 5) freeLaneCount = 3;
            }
            if (freeLaneCount > 0) {
                if (freeLaneCount > 1) UpdateMovingThingsOnGround(least, Grounds[least].transform.position.z - Player.transform.position.z);
                freeLaneCount -= 1;
                normalCount = 0;
            } else {
                UpdateThingsOnGround(least);
            }
            DestroyInvalid();
            least = (least + 1) % 5;
        }
    }

    void UpdateMovingThingsOnGround(int pos, float distance) {
        Vector3 InstantiatePosition = Grounds[pos].transform.position + new Vector3(0, 0, 2 * distance);
        int freeLane = random.Next(0, 3);
        int speed = 60;

        int x1 = 0, x2 = 1;

        switch(freeLane) {
            case 0: x1 = 1; x2 = 2; break;
            case 1: x1 = 0; x2 = 2; break;
            case 2: x1 = 0; x2 = 1; break;
        }

        GameObject bus1 = Instantiate(Bus, InstantiatePosition + new Vector3(-5 + x1 * 5, 0, random.Next(0, 10)) + Bus.transform.position, Bus.transform.rotation);
        dynamicObjects.Add(bus1);
        dynamicObjectVelocities.Add(-speed - random.Next(0, 3));

        GameObject bus2 = Instantiate(Bus, InstantiatePosition + new Vector3(x2 * 5 - 5, 0, random.Next(0, 10)) + Bus.transform.position, Bus.transform.rotation);
        dynamicObjects.Add(bus2);
        dynamicObjectVelocities.Add(-speed - random.Next(0, 3));

        freeLane = random.Next(0, 3);
        switch (freeLane) {
            case 0: x1 = 1; x2 = 2; break;
            case 1: x1 = 0; x2 = 2; break;
            case 2: x1 = 0; x2 = 1; break;
        }


        GameObject bus3 = Instantiate(Bus, InstantiatePosition + new Vector3(-5 + x1 * 5, 0, random.Next(100, 110)) + Bus.transform.position, Bus.transform.rotation);
        dynamicObjects.Add(bus3);
        dynamicObjectVelocities.Add(-speed - random.Next(0, 3));

        GameObject bus4 = Instantiate(Bus, InstantiatePosition + new Vector3(x2 * 5 - 5, 0, random.Next(100, 110)) + Bus.transform.position, Bus.transform.rotation);
        dynamicObjects.Add(bus4);
        dynamicObjectVelocities.Add(-speed - random.Next(0, 3));
    }


    void UpdateThingsOnGround(int pos) {
        normalCount += 1;
        start_length += 1;
        Vector3 StartPosition = Grounds[pos].transform.position - new Vector3(0, 0, 50);
        int length = random.Next(5, System.Math.Min(BoardLength, start_length));

        GameBoard gameBoard = new GameBoard(3, length);
        int[,] board = gameBoard.GetBoard();

        for (int i = 0; i<3; i++) {
            for (int j=0; j<length; j++) {
                Vector3 currentPosition = StartPosition + new Vector3((i - 1) * 5, 0, j * (100/length));
                switch (board[i, j]) {
                    case GameBoard.FULL_OBSTACLE:
                        gameObjects.Add(Instantiate(JumpSlideObstaclePrefab, currentPosition + JumpSlideObstaclePrefab.transform.position, JumpSlideObstaclePrefab.transform.rotation));
                        break;
                    case GameBoard.COLLECTIBLE:
                        gameObjects.Add(Instantiate(CollectiblePrefab, currentPosition + CollectiblePrefab.transform.position, CollectiblePrefab.transform.rotation));
                        break;

                    case GameBoard.JUMP_OBSTACLE:
                        gameObjects.Add(Instantiate(JumpObstaclePrefab, currentPosition + JumpObstaclePrefab.transform.position, JumpObstaclePrefab.transform.rotation));
                        break;

                    case GameBoard.SLIDE_OBSTACLE:
                        gameObjects.Add(Instantiate(SlideObstaclePrefab, currentPosition + SlideObstaclePrefab.transform.position, SlideObstaclePrefab.transform.rotation));
                        break;

                    case GameBoard.LONG_FULL_OBSTACLE:
                        gameObjects.Add(Instantiate(LongJumpSlideObstacle, currentPosition + LongJumpSlideObstacle.transform.position, LongJumpSlideObstacle.transform.rotation));
                        break;

                    case GameBoard.LONG_JUMP_OBSTACLE:
                        gameObjects.Add(Instantiate(LongJumpObstacle, currentPosition + LongJumpObstacle.transform.position, LongJumpObstacle.transform.rotation));
                        break;


                }
            }
        }

    }


    void OldUpdateThingsOnGround(int pos) {
        Vector3 StartPosition = Grounds[pos].transform.position - new Vector3(0, 0, 50);
        for (int i=0; i<GroundUpdatePositions.Count; i++) {
            Vector3 leftPosition = StartPosition + new Vector3(-5, 0, GroundUpdatePositions[0]);
            Vector3 rightPosition = StartPosition + new Vector3(5, 0, GroundUpdatePositions[0]);
            Vector3 centerPosition = StartPosition + new Vector3(0, 0, GroundUpdatePositions[0]);

            gameObjects.Add(Instantiate(JumpObstaclePrefab, leftPosition + JumpObstaclePrefab.transform.position, JumpObstaclePrefab.transform.rotation));
            gameObjects.Add(Instantiate(SlideObstaclePrefab, rightPosition + SlideObstaclePrefab.transform.position, SlideObstaclePrefab.transform.rotation));
            gameObjects.Add(Instantiate(JumpSlideObstaclePrefab, centerPosition + JumpSlideObstaclePrefab.transform.position, JumpSlideObstaclePrefab.transform.rotation));


        }
    }


    void DestroyInvalid() {
        for (int i=leastCount; i<gameObjects.Count; i++) {
            if (gameObjects[i].transform.position.z < Player.transform.position.z - 100) {
                Destroy(gameObjects[i]);
                leastCount = i + 1;
            }
        }

        for (int i = dynamicLeastCount; i < dynamicObjects.Count; i++) {
            if (dynamicObjects[i].transform.position.z < Player.transform.position.z - 100) {
                Destroy(dynamicObjects[i]);
                dynamicLeastCount = i + 1;
            }
        }
    }
}
