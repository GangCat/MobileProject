using DI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Background : DIMono
{
    [Inject]
    Camera mainCam;

    private void LateUpdate()
    {
        Vector3 curPos = transform.position;
        curPos.x = mainCam.transform.position.x;
        transform.position = curPos;
    }

    public float pallaSpeed;

   

}
