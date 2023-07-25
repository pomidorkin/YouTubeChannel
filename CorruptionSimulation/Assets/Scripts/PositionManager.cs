using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager : MonoBehaviour
{
    [SerializeField] GameObject forestPosition;
    [SerializeField] GameObject altruistHomePosition;
    [SerializeField] GameObject individualistHomePosition;
    [SerializeField] GameObject generalArea;
    private float walkRadius = 1f;
    [SerializeField] private float yPos = 3.09f;
    public bool walkRadiusRequired = true;
    private void OnEnable()
    {
        if (walkRadiusRequired)
        {
            walkRadius = forestPosition.transform.localScale.x;
        }
    }

    public Vector3 GetRandomForestPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += forestPosition.transform.position;
        randomDirection.y = yPos;
        return randomDirection;
    }

    public Vector3 GetRandomHomePosition(bool altruist)
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        if (altruist)
        {
            randomDirection += altruistHomePosition.transform.position;
        }
        else
        {
            randomDirection += individualistHomePosition.transform.position;
        }
        randomDirection.y = yPos;
        return randomDirection;
    }

    public Vector3 GetRandomSpawnPosition(bool altruist)
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        if (altruist)
        {
            randomDirection += altruistHomePosition.transform.position;
        }
        else
        {
            randomDirection += individualistHomePosition.transform.position;
        }
        randomDirection.y = yPos;
        return randomDirection;
    }

    public Vector3 GetRandomGeneralPositionInRange()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += generalArea.transform.position;
        //randomDirection.y = yPos;
        return randomDirection;
    }
}
