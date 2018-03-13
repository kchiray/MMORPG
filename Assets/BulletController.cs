using UnityEngine;

public class BulletController : MonoBehaviour
{

    void OnCollisionEnter()
    {
        Destroy(gameObject);
    }
}