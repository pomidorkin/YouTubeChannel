using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeFaceChanger : MonoBehaviour
{
    [SerializeField] Material face;
    [SerializeField] Texture calmTexture;
    [SerializeField] Texture angryTexture;
    private bool calm = true;

    private void OnEnable()
    {
        if (calm)
        {
            calm = false;
            face.SetTexture("_MainTex", angryTexture);
        }
        else
        {
            calm = true;
            face.SetTexture("_MainTex", calmTexture);
        }
    }
}
