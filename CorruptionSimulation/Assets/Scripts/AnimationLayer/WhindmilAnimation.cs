using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhindmilAnimation : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 50 * Time.deltaTime);
    }
}
