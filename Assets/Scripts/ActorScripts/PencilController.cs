using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PencilController : MonoBehaviour
{
    //pencil moves forward slowly for half of hang time then stays still the second half before launching forward.
    //@TODO make const
    public float HANG_TIME;
    public float HANG_SPEED;

    public float speed;

    private Vector3 pos;
    private float timePassed;
    private bool playedSound;
    public GameObject glimmer;
   
    // Start is called before the first frame update
    void Start()
    {
        timePassed = 0;
        pos = transform.position;
        playedSound = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playedSound)
        {
            if (timePassed > HANG_TIME)
            {
                playedSound = true;
                AudioManager.Instance.Play("PencilWoosh");
            }
        }

        timePassed += Time.deltaTime;
        if (timePassed < HANG_TIME / 2)
            pos.x += HANG_SPEED * Time.deltaTime;
        else if (glimmer.activeSelf == false)
            glimmer.SetActive(true);
        else if (timePassed > HANG_TIME)
        {
            pos.x += speed * Time.deltaTime;
            if(glimmer.activeSelf == true)
                glimmer.SetActive(false);
        }
        transform.position = pos;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
