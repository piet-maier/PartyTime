using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopUpHandler : MonoBehaviour
{
    public void Start()
    {
        Destroy(gameObject, 2f);
    }
}
