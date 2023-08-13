using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InfluenceSlime : MonoBehaviour
{
    [SerializeField] InfluenceController influenceController;
    public int opinion = 1;
    [SerializeField] [Range(1, 100)] float threshold = 6;
    [SerializeField] float maxDistance = 10f;
    [SerializeField] bool controlOwnThreshold = false;
    public List<InfluenceSlime> society;


    [SerializeField] public Animator animator;
    public NavMeshAgent agent;

    [SerializeField] Renderer renderer;
    [SerializeField] Material greenMat;
    [SerializeField] Material redMat;

    public bool opinionReviewed = false;
    public bool positionReached = false;

    float walkRadius = 3f;

    private void OnEnable()
    {
        influenceController.slimes.Add(this);
        influenceController.ChangeOpinionCommand += TryChangeOpinion;
        influenceController.ResetCommand += ResetSlime;
        influenceController.InteractCommand += Interact;

        if (!controlOwnThreshold)
        {
            if (influenceController.generalThreshold)
            {
                threshold = influenceController.threshold;
            }
            else
            {
                threshold = UnityEngine.Random.Range(influenceController.minThreshold, influenceController.maxThreshold + 1);
            }
        }
    }

    private void OnDisable()
    {
        influenceController.ChangeOpinionCommand -= TryChangeOpinion;
        influenceController.ResetCommand -= ResetSlime;
        influenceController.InteractCommand -= Interact;
    }

    private void Update()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    if (!positionReached)
                    {
                        animator.SetTrigger("StopJumping");
                        positionReached = true;
                        influenceController.CheckIfAllReachedPosition();
                    }
                }
            }
        }
    }

    private void Interact()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
        Vector3 finalPosition = hit.position;

        animator.SetTrigger("Jump");
        agent.SetDestination(finalPosition);
    }

    private void ResetSlime()
    {
        opinionReviewed = false;
        positionReached = false;
    }

    void linearThresholdModel(List<InfluenceSlime> neighbors, float threshold)
    {
        float sumOfWeights = 0;
        for (int i = 0; i < neighbors.Count; i++)
        {
            float dist = Vector3.Distance(neighbors[i].transform.position, transform.position);
            //Debug.Log("dist: " + dist);
            //Debug.Log("dist: " + dist + "(maxDistance - dist): " + (maxDistance - dist));
            sumOfWeights += (maxDistance - dist) * neighbors[i].opinion;
        }
        sumOfWeights = (sumOfWeights / influenceController.GetMaxWeight()) * 100;

        //Debug.Log("My threshold: " + threshold + ", sumOfWeights: " + sumOfWeights);

        //if ((sumOfWeights >= threshold && opinion == -1) || (sumOfWeights <= -threshold && opinion == 1))
        if ((sumOfWeights >= threshold && opinion == -1) || (sumOfWeights <= -threshold && opinion == 1))
        {
            // Opinion changed
            if (opinion == 1)
            {
                renderer.material = redMat;
                opinion = -1;
            }
            else
            {
                renderer.material = greenMat;
                opinion = 1;
            }
        }
    }

    private void TryChangeOpinion()
    {
        if (society.Count == 0)
        {
            PopulateSociety();
        }
        linearThresholdModel(society, threshold);
        opinionReviewed = true;
        if (opinion == 1)
        {
            influenceController.greenSlimesThreshold.Add(threshold);
        }
        else
        {
            influenceController.redSlimesThreshold.Add(threshold);
        }
        influenceController.CheckIfAllProcessedStep();
    }

    private void PopulateSociety()
    {
        foreach (InfluenceSlime slime in influenceController.slimes)
        {
            if (slime != this)
            {
                society.Add(slime);
            }
        }
    }
}
