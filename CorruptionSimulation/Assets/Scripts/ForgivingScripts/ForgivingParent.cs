using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForgivingParent : MonoBehaviour
{
    [SerializeField] ForgivingController forgivingController;
    private Dictionary<ForgivingParent, bool> memorizedSlimes; // false == no help
    public bool infected = false;
    [SerializeField] private int enegry = 6;
    [SerializeField] int maxEnergy = 6;
    [SerializeField] int energyToReproduce = 3;
    public int infectionChance = 10;
    [SerializeField] bool isEvil = false;
    [SerializeField] bool isEgoist = false;
    public bool stepFinished = false;
    public bool isBeingAskedForHelp = false;

    private ForgivingParent slimeHelper;

    NavMeshAgent agent;
    public bool positionReached = false;

    private void OnEnable()
    {
        forgivingController = FindObjectOfType<ForgivingController>();
        forgivingController.allSlimes.Add(this);
        memorizedSlimes = new Dictionary<ForgivingParent, bool>();
        agent = GetComponent<NavMeshAgent>();
        enegry = maxEnergy;

        forgivingController.ResetCommand += ResetSlime;
        forgivingController.SlimeDied += CheckIfMemorizedDied;
        forgivingController.NextStepCommand += ProcessNextStep;
        forgivingController.ReproduceCommand += Reproduce;

    }

    private void OnDisable()
    {
        forgivingController.ResetCommand -= ResetSlime;
        forgivingController.SlimeDied -= CheckIfMemorizedDied;
        forgivingController.NextStepCommand -= ProcessNextStep;
        forgivingController.ReproduceCommand -= Reproduce;
    }

    private void Reproduce()
    {
        /*if ((enegry - 2) > 0 && !infected)
        {
            if (!isEgoist && !isEvil)
            {
                forgivingController.SpawnNewSlime(1);
            }
            else if (!isEgoist && isEvil)
            {
                forgivingController.SpawnNewSlime(2);
            }
            else if(isEgoist && !isEvil)
            {
                forgivingController.SpawnNewSlime(3);
            }
        }*/
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
                        //animator.SetTrigger("StopJumping");
                        

                        AskForHelp(slimeHelper);
                    }
                }
            }
        }
    }

    private void ResetSlime()
    {
        if (enegry < maxEnergy)
        {
            enegry++;
        }
        isBeingAskedForHelp = false;
        positionReached = false;
        stepFinished = false;
    }

    private void CheckIfMemorizedDied(object source, ForgivingController.SlimeDieEventArgs args)
    {
        if (memorizedSlimes.ContainsKey(args.Slime))
        {
            memorizedSlimes.Remove(args.Slime);
        }
    }

    private void ProcessNextStep()
    {
        //stepFinished = false;
        if (!infected)
        {
            int rnd = UnityEngine.Random.Range(1, 101);
            if (rnd <= infectionChance)
            {
                infected = true;
                Debug.Log("I AM INFECTED!" + gameObject.name + "Infection random number = " + rnd);
                //AskForHelp();
            }
            stepFinished = true;
            positionReached = true;
            forgivingController.CheckIfAllReachedPosition();
            forgivingController.CheckIfAllProcessedStep();
        } else if (infected)
        {
            if (forgivingController.allSlimes.Count > 1)
            {
                if (isEvil)
                {
                    slimeHelper = forgivingController.AskRandomSlime();
                    if (memorizedSlimes.ContainsKey(slimeHelper))
                    {
                        bool isGood;
                        memorizedSlimes.TryGetValue(slimeHelper, out isGood);
                        while (memorizedSlimes.ContainsKey(slimeHelper) && !isGood)
                        {
                            slimeHelper = forgivingController.AskRandomSlime();
                            memorizedSlimes.TryGetValue(slimeHelper, out isGood);
                        }
                    }
                    agent.SetDestination(slimeHelper.transform.position);
                }
                else
                {
                    slimeHelper = forgivingController.AskRandomSlime();
                    agent.SetDestination(slimeHelper.transform.position);
                }
                //AskForHelp();
            }
        }
    }

    private void AskForHelp(ForgivingParent slimeHelper)
    {
        Debug.Log("Asking for help!" + gameObject.name);
        DecreaseEnergy();
        //ForgivingParent slimeHelper = forgivingController.AskRandomSlime();
        bool helpReply = slimeHelper.HelpReply(this);
        if (helpReply)
        {
            infected = false;
            //enegry = maxEnergy; // “ут нужно определить и высчитать, что происходит тогда, когда мы лечим. ¬с€ энерги€ возвращаетс€, нисколько не возвращаетс€ или чуть-чуть возвращаетс€
            enegry += 2;
            Debug.Log("Help Received! " + gameObject.name + ", The helper is " + slimeHelper.name);
            if (isEvil && !memorizedSlimes.ContainsKey(slimeHelper))
            {
                memorizedSlimes.Add(slimeHelper, true);
            }
        }
        if (!helpReply && isEvil && !memorizedSlimes.ContainsKey(slimeHelper))
        {
            memorizedSlimes.Add(slimeHelper, false);
        }
        stepFinished = true;
        positionReached = true;
        forgivingController.CheckIfAllReachedPosition();
        forgivingController.CheckIfAllProcessedStep();
    }

    public bool HelpReply(ForgivingParent helplessSlime)
    {
        isBeingAskedForHelp = true;
        if (isEvil)
        {
            if (memorizedSlimes.ContainsKey(helplessSlime))
            {
                // If val == true, it means that the slime has helped in the past
                bool val;
                memorizedSlimes.TryGetValue(helplessSlime, out val);
                if (val)
                {
                    Debug.Log("I am zlopamyatniy and I am helping! " + gameObject.name);
                    DecreaseEnergy();
                    return true;
                }
                else
                {
                    Debug.Log("I am zlopamyatniy and I am NOT helping! " + gameObject.name);
                    return false;
                }
            }
            else
            {
                Debug.Log("I am zlopamyatniy and I am helping! " + gameObject.name);
                DecreaseEnergy();
                return true;
            }
        }
        else if (isEgoist)
        {
            Debug.Log("I am an egoist and I am NOT helping! " + gameObject.name); // “ут баг какой-то: MissingReferenceException:
                                                                                  // The object of type 'ForgivingParent' has
                                                                                  // been destroyed but you are still trying to access it.
                                                                                  // Your script should either check if it is null or you
                                                                                  // should not destroy the object.
            return false;
        }
        else
        {
            Debug.Log("I am a good guy and I am helping! " + gameObject.name); // And here
            DecreaseEnergy();
            return true;
        }
    }

    private void DecreaseEnergy()
    {
        if ((enegry - energyToReproduce) > 0)
        {
            enegry -= energyToReproduce;
        }
        else
        {
            forgivingController.SlimeDiedCommand(this);
            Destroy(gameObject);
        }
    }
}
