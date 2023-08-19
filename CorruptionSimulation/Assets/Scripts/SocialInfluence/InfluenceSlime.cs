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

    [SerializeField] bool marchingSlime = false;
    [SerializeField] bool stationarygSlime = false;
    private bool startMarching = false;
    [SerializeField] AnimationClip referenceAnim;
    private float marchingLength;
    private float marchingCounter;


    [SerializeField] public Animator animator;
    public NavMeshAgent agent;

    [SerializeField] Renderer renderer;
    [SerializeField] Material greenMat;
    [SerializeField] Material redMat;
    [SerializeField] ParticleSystem changeOpiniorParticle;

    public bool opinionReviewed = false;
    public bool positionReached = false;

    float walkRadius = 3f;

    [SerializeField] float MyLastSumOfWeights = 0;

    private void OnEnable()
    {
        influenceController.slimes.Add(this);
        influenceController.ChangeOpinionCommand += TryChangeOpinion;
        influenceController.ResetCommand += ResetSlime;
        influenceController.InteractCommand += Interact;

        if (marchingSlime || stationarygSlime)
        {
            agent.enabled = false;
        }

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

        marchingLength = referenceAnim.length;
    }

    private void OnDisable()
    {
        influenceController.ChangeOpinionCommand -= TryChangeOpinion;
        influenceController.ResetCommand -= ResetSlime;
        influenceController.InteractCommand -= Interact;
    }

    private void Update()
    {
        if (!stationarygSlime && !marchingSlime)
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

        if (startMarching)
        {
            if (marchingCounter < marchingLength)
            {
                marchingCounter += Time.deltaTime;
                transform.position += new Vector3(0, 0, 1) * Time.deltaTime;
            }
            else
            {
                animator.SetTrigger("StopJumping");
                positionReached = true;
                startMarching = false;
                marchingCounter = 0;
                influenceController.CheckIfAllReachedPosition();
            }
        }
    }

    private void Interact()
    {
        if (!marchingSlime && !stationarygSlime)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * walkRadius;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
            Vector3 finalPosition = hit.position;

            animator.SetTrigger("Jump");
            agent.SetDestination(finalPosition);
        }
        else if (marchingSlime)
        {
            animator.SetTrigger("Jump");
            startMarching = true;
        }
        else if (stationarygSlime)
        {
            positionReached = true;
            influenceController.CheckIfAllReachedPosition();
        }
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
            float opinionWeight = (maxDistance - dist);
            if (opinionWeight < 0)
            {
                opinionWeight = 0;
            }
            //sumOfWeights += (opinionWeight * (opinionWeight / influenceController.maxDistance)) * neighbors[i].opinion;
            sumOfWeights += ((opinionWeight / influenceController.maxDistance) * (opinionWeight / influenceController.maxDistance)) * neighbors[i].opinion; // Интересно при значении порога в 0.3
        }

        sumOfWeights = (sumOfWeights / influenceController.GetMaxWeight()) * 100;
        MyLastSumOfWeights = sumOfWeights;

        if ((sumOfWeights >= threshold && opinion == -1) || (sumOfWeights <= -threshold && opinion == 1))
        {
            // Opinion changed
            if (opinion == 1)
            {
                changeOpiniorParticle.gameObject.SetActive(true);
                StartCoroutine(DisableParticleEffect());
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

    private IEnumerator DisableParticleEffect()
    {
        yield return new WaitForSeconds(1);
        changeOpiniorParticle.gameObject.SetActive(false);
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
