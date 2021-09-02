using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAnimatorController : MonoBehaviour
{
    public GameObject panel;
    private Animator animator;
    private const int CLIP_INDEX = 1;
  
    public void TogglePanel()
    {
        animator = panel.GetComponent<Animator>();
        if (animator != null)
        {
            bool isOpen = animator.GetBool("open");
            if (isOpen)
                StartCoroutine(SetPanelInactive());
            else
                panel.SetActive(true);

            animator.SetBool("open", !isOpen);

        }

    }


    IEnumerator SetPanelInactive()
    {
        float animTime = animator.runtimeAnimatorController.animationClips[CLIP_INDEX].length;
        yield return new WaitForSeconds(animTime);
        panel.SetActive(false);
    }
   
}
