using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHelper : MonoBehaviour
{
    public Animator animator;
      public void TriggerAnimationEvent()
      {
        //animator.SetBool("Item Use", false);
    }

     public void OnEndUse()
     {
        //animator.SetBool("Item Use", false);
     }
}
