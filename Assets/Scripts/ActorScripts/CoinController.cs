using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private float speed;
    private Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        speed = Values.Instance.BallSpeed();
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        pos.x += speed * Time.deltaTime;
        transform.position = pos;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
