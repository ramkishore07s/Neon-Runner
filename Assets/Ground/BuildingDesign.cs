using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDesign : MonoBehaviour
{
    public List<Texture2D> BuildingTextures;
    public List<Texture2D> HorizontalTextures;
    public List<Texture2D> VerticalTextures;

    public GameObject player;

    private System.Random random = new System.Random();

    public float min_dist, max_dist;

    public GameObject CubePrefab;
    public int offset;

    public int min_width, max_width, max_floors, min_floors, min_floor_height, max_floor_height;

    private float left_till_z, right_till_z;

    private List<GameObject> LeftGameObjects, RightGameObjects;
    private int left_least_i = 0, right_least_i = 0;

    void Start()
    {
        LeftGameObjects = new List<GameObject>();
        RightGameObjects = new List<GameObject>();


        float current_y = player.transform.position.z;
        left_till_z = -10; right_till_z = -10;

        while (left_till_z < player.transform.position.z + max_dist) {
            AddLeftBuilding();
        }

        while (right_till_z < player.transform.position.z + max_dist) {
            AddRightBuilding();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if ((left_till_z < player.transform.position.z + max_dist)) {
            RemoveUseless();
            while (left_till_z < player.transform.position.z + max_dist) {
                AddLeftBuilding();
            }
        }

        if ((right_till_z < player.transform.position.z + max_dist)) {
            while (right_till_z < player.transform.position.z + max_dist) {
                AddRightBuilding();
            }
        }
    }

    void AddLeftBuilding() {
        float width = random.Next(min_width, max_width);
        float floor_height = random.Next(min_floor_height, max_floor_height);
        float height = random.Next(min_floors, max_floors);

        float x = offset + width / 2;
        float y = (floor_height * height) / 2 - 10;
        float z = left_till_z + width / 2;

        GameObject building = Instantiate(CubePrefab, new Vector3(x, y, z), Quaternion.identity);
        building.transform.localScale = new Vector3(width, floor_height * height, width);
        building.GetComponent<MeshRenderer>().material.SetTexture("Texture2D_493F24D9", BuildingTextures[random.Next(0, BuildingTextures.Count)]);
        LeftGameObjects.Add(building);

        left_till_z += width + random.Next(0, 10);
        AddTopBanner(offset + 4, y + 16, z - width/2 + 4);
        AddFrontBanner(offset - 1, floor_height, z);
        AddFaceBanner(offset - 1, random.Next((int) floor_height, (int) (y * 2 + floor_height)) , z + width / 2 + 4);


    }

    void AddRightBuilding() {
        float width = random.Next(min_width, max_width);
        float floor_height = random.Next(min_floor_height, max_floor_height);
        float height = random.Next(min_floors, max_floors);

        float x = - offset - width / 2;
        float y = (floor_height * height) / 2 - 10;
        float z = right_till_z + width / 2;

        GameObject building = Instantiate(CubePrefab, new Vector3(x, y, z), Quaternion.identity);
        building.transform.localScale = new Vector3(width, floor_height * height, width);
        building.GetComponent<MeshRenderer>().material.SetTexture("Texture2D_493F24D9", BuildingTextures[random.Next(0, BuildingTextures.Count)]);
        RightGameObjects.Add(building);
        AddTopBanner(-offset - 4, y + 16, z - width / 2 + 4);

        AddFrontBanner(-offset + 1, floor_height, z);

        right_till_z += width + random.Next(0, 10);
        AddFaceBanner(-offset + 1, random.Next((int)floor_height, (int)(y * 2 + floor_height)), z + width / 2 + 3);

    }

    void AddTopBanner(float x, float height, float z) {
        GameObject banner = Instantiate(CubePrefab, new Vector3(x, height, z - 5), Quaternion.identity);
        banner.transform.localScale = new Vector3(-10, -10, 0.1f);
        banner.GetComponent<MeshRenderer>().material.SetTexture("Texture2D_493F24D9", HorizontalTextures[random.Next(0, HorizontalTextures.Count)]);
        LeftGameObjects.Add(banner);
    }

    void AddFaceBanner(float x, float height, float z) {
        GameObject banner = Instantiate(CubePrefab, new Vector3(x, height, z - 5.3f), Quaternion.identity);
        banner.transform.localScale = new Vector3(-3, -10, 0.1f);
        banner.GetComponent<MeshRenderer>().material.SetTexture("Texture2D_493F24D9", VerticalTextures[random.Next(0, VerticalTextures.Count)]);
        LeftGameObjects.Add(banner);
    }

    void AddFrontBanner(float x, float height, float z) {
        GameObject banner = Instantiate(CubePrefab, new Vector3(x, height, z), Quaternion.identity);
        banner.transform.localScale = new Vector3(0.1f, 10, 10);
        banner.GetComponent<MeshRenderer>().material.SetTexture("Texture2D_493F24D9", HorizontalTextures[random.Next(0, HorizontalTextures.Count)]);
        RightGameObjects.Add(banner);
    }

    void RemoveUseless() {
        for (int i=left_least_i; i<LeftGameObjects.Count; i++) {
            if (LeftGameObjects[i].transform.position.z - player.transform.position.z < min_dist) {
                Destroy(LeftGameObjects[i]);
                left_least_i = i + 1;
            }
        }

        for (int i = right_least_i; i < RightGameObjects.Count; i++) {
            if (RightGameObjects[i].transform.position.z - player.transform.position.z < min_dist) {
                Destroy(RightGameObjects[i]);
                right_least_i = i + 1;
            }
        }
    }
}
