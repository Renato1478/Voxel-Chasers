using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 20.0f);
    }
}
