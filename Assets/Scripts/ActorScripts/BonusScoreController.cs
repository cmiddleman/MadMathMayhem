using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BonusScoreController : MonoBehaviour
{
    private float speed;

    private Vector3 pos;
    private RectTransform rt;
    private TextMeshPro tmp;
    public int bonusScore { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        speed = Values.Instance.BallSpeed();
        tmp = gameObject.GetComponent<TextMeshPro>();
        rt = gameObject.GetComponent<RectTransform>();
        pos = rt.position;

        //@TODO, tweak
        bonusScore = 10 * Random.Range(1, 6);
        tmp.text = bonusScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        pos.x += speed * Time.deltaTime;
        rt.position = pos;
       
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
