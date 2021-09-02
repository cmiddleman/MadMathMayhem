using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaneTextController : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 pos;
    private Vector3 scale;
    private float animTime;
    public float scaleRate;
    public Vector3 targetPos;
    private float speed;
    private float timeSinceStart;
    private Text text;

    private bool runAnim;
    
    private RectTransform rect;
    // Start is called before the first frame update
    void Start()
    {
        rect = gameObject.GetComponent<RectTransform>();
        text = gameObject.GetComponent<Text>();
        startPos = rect.position;
        pos = startPos;
        scale = rect.localScale;
        runAnim = false;
        timeSinceStart = 0f;
        animTime = Values.MATH_TEXT_ANIM_TIME;

        float distanceToTarget = Vector3.Distance(startPos, targetPos);
        speed = distanceToTarget / animTime;

       //@TODO make targetPos legit, dunno why issues with center in recttransform.
        Debug.Log(targetPos);
      //  targetPos = Camera.main.ScreenToWorldPoint(targetPos);
       // Debug.Log(targetPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (runAnim)
        {
       
            timeSinceStart += Time.deltaTime;
            if(timeSinceStart > animTime)
            {
                 Reset();
            }

            Move();
        }
    }

    public void StartAnim()
    {
        runAnim = true;
      
    }

    private void Move()
    {
        pos = Vector3.MoveTowards(pos, targetPos, speed * Time.deltaTime);
        rect.position = pos;

        scale += scaleRate * Time.deltaTime * new Vector3(1f, 1f, 0f);
        rect.localScale = scale;

     //   text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Min(1, 2 - 2*timeSinceStart / animTime));
    }

    private void Reset()
    {
        timeSinceStart = 0;
        runAnim = false;
        rect.position = startPos;
        pos = startPos;
        scale = new Vector3(1f, 1f, 1f);
        rect.localScale = scale;
        text.text = "";
    }
}
