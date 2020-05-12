using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{

    const int threadGroupSize = 1024;

    public BoidSettings settings;
    //public ComputeShader compute;
    private Boid[] boids;

    [SerializeField] private Transform _target;


    void Start()
    {
        boids = FindObjectsOfType<Boid>();

        foreach (Boid boid in boids)
        {
            boid.Initialize(settings, _target);
        }
    }


    void Update()
    {
        if (boids != null)
        {
            int boidCount = boids.Length;
            var boidData = new BoidData[boidCount];

            for (int i = 0; i < boids.Length; i++)
            {
                boidData[i].position = boids[i].position;
                boidData[i].direction = boids[i].forward;
            }

            for (int boidIndex = 0; boidIndex < boids.Length; boidIndex++)
            {
                for (int otherBoids = 0; otherBoids < boids.Length; otherBoids++)
                {
                    if (boidIndex != otherBoids)
                    {
                        BoidData otherBoid = boidData[otherBoids];
                        Vector3 offset = otherBoid.position - boidData[boidIndex].position;
                        float srqDst = offset.x * offset.x + offset.y * offset.y + offset.z * offset.z;

                        if(srqDst < settings.perceptionRadius * settings.perceptionRadius)
                        {
                            boidData[boidIndex].numFlockmates += 1;
                            boidData[boidIndex].flockHeading += otherBoid.direction;
                            boidData[boidIndex].flockCentre += otherBoid.position;

                            if (srqDst < settings.avoidanceRadius * settings.avoidanceRadius)
                            {
                                boidData[boidIndex].avoidanceHeading -= offset / srqDst;
                            }
                        }
                    }
                }

                boids[boidIndex].avgFlockHeading = boidData[boidIndex].flockHeading;
                boids[boidIndex].centreOfFlockmates = boidData[boidIndex].flockCentre;
                boids[boidIndex].avgAvoidanceHeading = boidData[boidIndex].avoidanceHeading;
                boids[boidIndex].numPerceivedFlockmates = boidData[boidIndex].numFlockmates;

                boids[boidIndex].UpdateBoid();

            }


            //for (int i = 0; i < boids.Length; i++)
            //{
            //    boids[i].avgFlockHeading = boidData[i].flockHeading;
            //    boids[i].centreOfFlockmates = boidData[i].flockCentre;
            //    boids[i].avgAvoidanceHeading = boidData[i].avoidanceHeading;
            //    boids[i].numPerceivedFlockmates = boidData[i].numFlockmates;

            //    boids[i].UpdateBoid();
            //}
        }
    }
}





































//var boidBuffer = new ComputeBuffer(boidCount, BoidData.Size);
//boidBuffer.SetData(boidData);

//compute.SetBuffer(0, "boids", boidBuffer);
//compute.SetInt("numBoids", boids.Length);
//compute.SetFloat("viewRadius", settings.perceptionRadius);
//compute.SetFloat("avoidRadius", settings.avoidanceRadius);

//int threadGroups = Mathf.CeilToInt(boidCount / (float)threadGroupSize);
//compute.Dispatch(0, threadGroups, 1, 1);

//boidBuffer.GetData(boidData);
//...
//...
//...
// boidBuffer.Release();