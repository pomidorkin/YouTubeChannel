using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DemonstrationTextController : MonoBehaviour
{
    [SerializeField] Transform observableSlime;
    [SerializeField] TMP_Text text;
    private float stepTime = 0f;
    private float timeCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeCounter < stepTime)
        {
            timeCounter += Time.deltaTime;
        }
        else
        {
            CalculateInfluenceParameters();
            timeCounter = 0;
        }
    }

    private void CalculateInfluenceParameters()
    {
            float dist = Vector3.Distance(observableSlime.position, transform.position);
            float opinionWeight = (40 - dist);
            if (opinionWeight < 0)
            {
                opinionWeight = 0;
            }
        dist = (1 - (dist / 40)) * 100;
    opinionWeight = ((opinionWeight / 40) * (opinionWeight / 40)) * 100;

        text.text = "Близ: " +  dist.ToString("F1") + "\nВес: " + opinionWeight.ToString("F1");
    }
}
