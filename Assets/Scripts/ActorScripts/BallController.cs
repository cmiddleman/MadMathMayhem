using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private const float REVOLUTION_TIME = 1f;
    private float speed;
    private Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        speed = Values.Instance.BallSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, -360 * REVOLUTION_TIME * Time.deltaTime));
        pos.x += speed * Time.deltaTime;
        transform.position = pos;

    }

    private void OnBecameVisible()
    {
        AudioManager.Instance.Play("PaperThrow");
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
 
}
