using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public float speed = 0.001f;

    private BoidGroup _group;
    private Camera _cam;
    bool turning = false;


    private void Awake()
    {
        _group = GetComponentInParent<BoidGroup>();
        _cam = Camera.main;
        setRandomdirection();

        speed = Random.Range(_group.minMaxFishSpeed.x, _group.minMaxFishSpeed.y);
    }

    public Vector3 GetDirection()
    {
        return transform.forward;
    }


    private void Update()
    {
        turning = (Vector3.Distance(transform.position, Vector3.zero) >= _group.tankSize);

        if (turning)
        {
            Vector3 direction = Vector3.zero - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), _group.rotationSpeed * Time.deltaTime);
            speed = Random.Range(_group.minMaxFishSpeed.x, _group.minMaxFishSpeed.y);
        }
        else
        {
            if (Random.Range(0, 5) < 1) applyRules();
        }

       // simulateBoid();
        move();
       // outOfScreenCheck();
    }


    private void move()
    {
        transform.Translate(0, 0, Time.deltaTime * speed);
    }


    private void simulateBoid()
    {
        Vector3 boidDirection = Allign();
        if(boidDirection != Vector3.zero) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(boidDirection), _group.rotationSpeed * Time.deltaTime);
    }


    private void setRandomdirection()
    {
        transform.position = new Vector3(Random.Range(-_group.tankSize, _group.tankSize), Random.Range(-_group.tankSize, _group.tankSize), Random.Range(-_group.tankSize, _group.tankSize));
        transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
    }


    private void outOfScreenCheck()
    {
        Vector3 position = transform.position;
        float maxY = _cam.transform.position.y + _cam.orthographicSize;
        float maxX = _cam.transform.position.x + (_cam.orthographicSize * ((float)Screen.width / (float)Screen.height));
        float maxZ = 10;

        if (position.y > maxY) position.y = -maxY;
        else if (position.y < -maxY) position.y = maxY;
        else if (position.x > maxX) position.x = -maxX;
        else if (position.x < -maxX) position.x = maxX;
        else if (position.z > maxZ) position.z = 0;
        else if (position.z < 0) position.z = maxZ;

        transform.position = position;
    }


    private void applyRules()
    {
        Boid[] boids = _group.boids.ToArray();

        Vector3 groupCenter = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float totalGroupSpeed = 0.1f;

        int groupSize = 0;

        foreach (Boid otherBoid in boids)
        {
            if (otherBoid != this)
            {
                float distance = Vector3.Distance(otherBoid.transform.position, transform.position);

                if (distance <= _group.groupRange)
                {
                    if (distance < 1) vavoid = vavoid += (transform.position - otherBoid.transform.position);

                    groupCenter += otherBoid.transform.position;
                    totalGroupSpeed = totalGroupSpeed + otherBoid.speed;
                    groupSize++;
                }
            }
        }

        if (groupSize > 0)
        {
            groupCenter = groupCenter / groupSize + (_group.targetPosition - transform.position);
            speed = totalGroupSpeed / groupSize;

            Vector3 direction = (groupCenter + vavoid) - transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), _group.rotationSpeed * Time.deltaTime);
            }
        }
    }


    /// <summary>
    /// Return the steering direction
    /// </summary>
    /// <returns></returns>
    private Vector3 Allign()
    {
        Boid[] boids = _group.boids.ToArray();
        Vector3 avrgGroupDirection = Vector3.zero;
        int boidCount = 0;

        foreach (Boid other in boids)
        {
            if(other != this)
            {
                float dis = Vector3.Distance(this.transform.position, other.transform.position);
                if (dis < _group.groupRange) 
                {
                    //avrgGroupDirection += other.transform.position;
                    avrgGroupDirection += other.GetDirection();
                    boidCount++;
                }
            }
        }


        if(boidCount > 0)
        {
            avrgGroupDirection += (_group.targetPosition - transform.position);
            avrgGroupDirection = avrgGroupDirection / boidCount;
        }

        return avrgGroupDirection - transform.position;
    }
}
