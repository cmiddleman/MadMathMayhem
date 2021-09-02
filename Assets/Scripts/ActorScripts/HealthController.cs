using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public const float MaxAngle = 20f;
    public const float RevTime = 2f;
    private float speed;

    private float timePassed;
    private Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        speed = Values.Instance.BallSpeed();
        pos = transform.position;
        timePassed = 0f;
        
    }

    // Update is called once per frame
    void Update()
    {
        pos.x += speed * Time.deltaTime;
        transform.position = pos;

        timePassed += Time.deltaTime;
        float rotate = MaxAngle * Mathf.Sin(2 * Mathf.PI * timePassed / RevTime);
        transform.rotation = Quaternion.Euler(0, 0, rotate);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
