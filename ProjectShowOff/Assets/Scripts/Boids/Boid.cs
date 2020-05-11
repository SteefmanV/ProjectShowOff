using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public float speed = 0.001f;

    private BoidGroup _group;
    //private Camera _cam;
    bool turning = false;


    private void Awake()
    {
        _group = GetComponentInParent<BoidGroup>();
        //_cam = Camera.main;
        setRandomdirection();

        speed = Random.Range(_group.minMaxFishSpeed.x, _group.minMaxFishSpeed.y);
    }


    private void Update()
    {
        turning = (Vector3.Distance(transform.position, Vector3.zero) >= _group.tankSize);

        if(turning)
        {
            Vector3 direction = Vector3.zero - transform.position; 
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), _group.rotationSpeed * Time.deltaTime);
            speed = Random.Range(_group.minMaxFishSpeed.x, _group.minMaxFishSpeed.y);
        }
        else
        {
            if (Random.Range(0, 5) < 1) applyRules();
        }

        move();
    }


    private void move()
    {
        transform.Translate(0, 0, Time.deltaTime * speed);
    }


    private void setRandomdirection()
    {
        float degrees = Random.Range(0, 361);
        transform.rotation = Quaternion.Euler(0, 0, degrees);
    }


    //private void outOfScreenCheck()
    //{
    //    Vector3 position = transform.position;
    //    float maxY = cam.transform.position.y + cam.orthographicSize;
    //    float maxX = cam.transform.position.x + (cam.orthographicSize * ((float)Screen.width / (float)Screen.height));
    //    float maxZ = cam.orthographicSize;

    //    if (position.y > maxY) position.y = -maxY;
    //    else if (position.y < -maxY) position.y = maxY;
    //    else if (position.x > maxX) position.x = -maxX;
    //    else if (position.x < -maxX) position.x = maxX;
    //    else if (position.z > maxZ) position.z = -maxZ;
    //    else if (position.z < -maxZ) position.z = maxZ;

    //    transform.position = position;
    //}


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

                if (distance <= _group.neighbourDistanceThrashhold)
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
}
