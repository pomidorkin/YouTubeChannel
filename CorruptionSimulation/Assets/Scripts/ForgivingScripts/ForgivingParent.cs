using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgivingParent : MonoBehaviour
{
    [SerializeField] ForgivingController forgivingController;
    private Dictionary<ForgivingParent, bool> memorizedSlimes; // Нужен дикшинэри, чтобы запоминать помог ли или нет
    public bool infected = false;
    private int enegry = 6;
    [SerializeField] int maxEnergy = 6;
    public int infectionChance = 10;
    [SerializeField] bool isEvil = false;
    [SerializeField] bool isEgoist = false;
    public bool stepFinished = false;

    private void OnEnable()
    {
        forgivingController = FindObjectOfType<ForgivingController>();
        forgivingController.allSlimes.Add(this);
        memorizedSlimes = new Dictionary<ForgivingParent, bool>();
        forgivingController.ResetCommand += ResetSlime;
        forgivingController.SlimeDied += CheckIfMemorizedDied;
        forgivingController.NextStepCommand += ProcessNextStep;
    }

    private void OnDisable()
    {
        forgivingController.ResetCommand -= ResetSlime;
        forgivingController.SlimeDied -= CheckIfMemorizedDied;
        forgivingController.NextStepCommand -= ProcessNextStep;
    }

    private void ResetSlime()
    {
        if (enegry < maxEnergy)
        {
            enegry++;
        }
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
            forgivingController.CheckIfAllProcessedStep();
        } else if (infected)
        {
            if (forgivingController.allSlimes.Count > 1)
            {
                AskForHelp();
            }
        }
    }

    private void AskForHelp()
    {
        Debug.Log("Asking for help!" + gameObject.name);
        DecreaseEnergy();
        ForgivingParent slimeHelper = forgivingController.AskRandomSlime();
        bool helpReply = slimeHelper.HelpReply(this);
        // memorizedSlimes.Add(slimeHelper); // Запоминает только злопамятный
        if (helpReply)
        {
            infected = false;
            enegry = maxEnergy;
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
        forgivingController.CheckIfAllProcessedStep();
    }

    public bool HelpReply(ForgivingParent helplessSlime)
    {
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
            Debug.Log("I am an egoist and I am NOT helping! " + gameObject.name);
            return false;
        }
        else
        {
            Debug.Log("I am a good guy and I am helping! " + gameObject.name);
            DecreaseEnergy();
            return true;
        }
    }

    private void DecreaseEnergy()
    {
        if ((enegry - 2) > 0)
        {
            enegry -= 2;
        }
        else
        {
            forgivingController.SlimeDiedCommand(this);
            Destroy(gameObject);
        }
    }
}
