using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteriodSpawner : MonoBehaviour
{

    public Asteroid asteroidPrefab;

    public float spawnRate = 2.0f;
    public int spawnAmount = 1;
    public float spawnDistance = 15.0f;

    public float trajcetoryVarience = 15.0f;

    private void Spawn()
    {
        for(int i = 0;i<this.spawnAmount;i++)
        {
            //Spawing the asteroid by calculating the direction through spawn distance which is then
            //passed to a spawn point offseting the transform.positon of the spawner object.
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * this.spawnDistance;
            Vector3 spawnPoint = this.transform.position + spawnDirection;

            //Calculating the rotation of a asteroid.
            float varience = Random.Range(-this.trajcetoryVarience, this.trajcetoryVarience);
            Quaternion rotation = Quaternion.AngleAxis(varience, Vector3.forward);

            Asteroid asteroid = Instantiate(asteroidPrefab,spawnPoint,rotation);
            asteroid.size = Random.Range(asteroid.minSize,asteroid.maxSize);
            //Setting the asteroid trajectory and passing the direction(Vector2)
            asteroid.SetTrajectory(rotation * -spawnDirection);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
