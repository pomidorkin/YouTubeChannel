using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairFall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        iTween.MoveTo(gameObject, iTween.Hash("y", 0, "time", 2, "islocal", true, "easetype", iTween.EaseType.easeOutBounce));
    }
}
