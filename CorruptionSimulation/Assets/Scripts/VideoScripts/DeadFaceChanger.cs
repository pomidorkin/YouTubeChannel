using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadFaceChanger : MonoBehaviour
{

    [SerializeField] Material face;
    [SerializeField] Texture deadFaceTexture;

    private void OnEnable()
    {
        face.SetTexture("_MainTex", deadFaceTexture);
    }
}
