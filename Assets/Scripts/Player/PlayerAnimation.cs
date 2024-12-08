using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    Animation run;
    [SerializeField]
    Animation jump;
    [SerializeField]
    Animation shoot;
    [SerializeField]
    Animation idle;

    private Animator myAnimator;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

}
