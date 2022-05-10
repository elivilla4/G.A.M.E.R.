using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterySpawner : MonoBehaviour
{
    public GameObject battery;
    public float batteryLife = 10f;

    public float spawnDelay = 2f;

    public float minX = -25f;
    public float maxX = 25f;
    public float minZ = -25f;
    public float maxZ = 25f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("SpawnBattery");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnBattery() {
        yield return new WaitForSeconds(spawnDelay);
        GameObject newBattery = Instantiate(battery, new Vector3(Random.Range(minX, maxX), 0.5f, Random.Range(minZ, maxZ)), battery.transform.rotation);
        Destroy(newBattery, batteryLife);
        StartCoroutine("SpawnBattery");
    }

}
