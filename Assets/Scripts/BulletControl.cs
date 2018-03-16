using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour {

    void OnCollisionEnter()
    {
        Destroy(gameObject);
    }
}
