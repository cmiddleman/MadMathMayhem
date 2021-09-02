using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    static int health;
    static float timePassed;
    public float RevTime;
    public float MaxAngle;
    // Start is called before the first frame update
    void Start()
    {
        timePassed = 0f;
        health = 3;
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime/(health +.00001f);
        float rotate = MaxAngle * Mathf.Sin(2 * Mathf.PI * timePassed / RevTime);
        transform.rotation = Quaternion.Euler(0, 0, rotate);
    }

    private void OnEnable()
    {
        health++;
    }
    private void OnDisable()
    {
        health--;
    }
}
