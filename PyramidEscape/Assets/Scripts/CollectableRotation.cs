using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableRotation : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(0, 100 * Time.deltaTime, 0) ;
    }
}
