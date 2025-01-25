using UnityEngine;

public class KnifeController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("SelfDestruct", 0.1f);
    }

    private void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
