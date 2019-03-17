﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    public ScrollingTerrain terrain;
    public GameObject porthole1;
    public GameObject porthole2;
    public GameObject cone;
    public GameObject powerUp;
    public GameObject texture;
    //must be in percentage
    public int spawingObjectSliceChance;

    private int maxObjectsOnASlice = 7;
    private int nbrPresets = 30;
    private float widthTerrain;
    private enum Obstacle { beer, jumpObs1, jumpObs2, avoidObs, nothing, jumpBeerObs };


    Obstacle[,] array = new Obstacle[,]
    {
        {Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.jumpObs2, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing, Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.beer, Obstacle.nothing, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.beer, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.beer},
        {Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.beer, Obstacle.nothing, Obstacle.beer},
        {Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.beer, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing},
        {Obstacle.jumpObs2, Obstacle.nothing, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs},
        {Obstacle.avoidObs, Obstacle.jumpObs1,  Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.avoidObs,  Obstacle.avoidObs},
        {Obstacle.nothing, Obstacle.avoidObs,  Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs,  Obstacle.jumpBeerObs},
        {Obstacle.nothing, Obstacle.avoidObs,  Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.avoidObs,  Obstacle.jumpBeerObs},
        {Obstacle.jumpObs2, Obstacle.nothing,  Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.avoidObs, Obstacle.avoidObs,  Obstacle.jumpObs1, Obstacle.nothing, Obstacle.avoidObs, Obstacle.nothing,  Obstacle.avoidObs},
        {Obstacle.avoidObs, Obstacle.nothing,  Obstacle.nothing, Obstacle.avoidObs, Obstacle.jumpObs1, Obstacle.jumpObs2,  Obstacle.nothing},
        {Obstacle.jumpBeerObs, Obstacle.jumpObs1,  Obstacle.jumpObs2, Obstacle.avoidObs, Obstacle.avoidObs, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.avoidObs, Obstacle.nothing,  Obstacle.avoidObs, Obstacle.nothing, Obstacle.avoidObs, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.beer, Obstacle.nothing,  Obstacle.avoidObs, Obstacle.beer, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing},
        {Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing, Obstacle.nothing, Obstacle.nothing, Obstacle.nothing,  Obstacle.nothing}
    };

    void Start()
    {
        widthTerrain = texture.GetComponent<Renderer>().bounds.size.z * maxObjectsOnASlice;
    }

    void GenerateObstacle(GameObject gameobject, Vector3 offset)
    {
        GameObject instance = Instantiate(gameobject, gameObject.transform.position + offset, gameobject.transform.rotation);
        terrain.AttachProp(instance.transform);
    }

    void ReplaceFloor(GameObject gameObject, int childIndex)
    {
        GameObject replacement = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation);
        Transform current = terrain.previousSlice.GetChild(childIndex);
        replacement.transform.position = current.position;
        replacement.transform.parent = current.parent;
        current.SetParent(null);
        Destroy(current.gameObject);
    }

    void ReplaceFloorLarge(GameObject gameObject, int childIndex)
    {
        GameObject replacement = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation);
        Transform left = terrain.previousSlice.GetChild(childIndex - 1);
        Transform right = terrain.previousSlice.GetChild(childIndex);
        replacement.transform.position = left.position + new Vector3(0.0f, 0.0f, 1.0f) * terrain.sliceLength;
        replacement.transform.parent = left.parent;
        left.SetParent(null);
        Destroy(left.gameObject);
        right.SetParent(null);
        Destroy(right.gameObject);
    }

    public void GenerateLine(Transform slice)
    {
        int isSpawingLine = Random.Range(0, 100);

        //spawn obstacles
        if (isSpawingLine <= spawingObjectSliceChance)
        {
            //getting a preset
            int typeOfTerrain = Random.Range(0, nbrPresets - 1);

            for (int i = 0; i < maxObjectsOnASlice; i++)
            {
                if (array[typeOfTerrain, i] == Obstacle.jumpObs1)
                {
                    ReplaceFloor(porthole1, i);
                }

                //else if (array[typeOfTerrain, i] == Obstacle.jumpObs2 && i % 2 == 1)
                //{
                //    ReplaceFloorLarge(porthole2, i);
                //}

                else if (array[typeOfTerrain, i] == Obstacle.avoidObs)
                {
                    GenerateObstacle(cone, slice.GetChild(i).transform.position);
                }

                else if (array[typeOfTerrain, i] == Obstacle.beer)
                {
                    GenerateObstacle(powerUp, slice.GetChild(i).transform.position);
                }

                else if (array[typeOfTerrain, i] == Obstacle.jumpBeerObs)
                {
                    GameObject instance2 = Instantiate(powerUp, slice.GetChild(i).transform.position + Vector3.up * 2, Quaternion.identity);
                    terrain.AttachProp(instance2.transform);
                }
            }
        }
    }
}
