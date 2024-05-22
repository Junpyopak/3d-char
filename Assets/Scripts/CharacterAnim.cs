using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnim : MonoBehaviour
{
    Animator anim;

    [SerializeField] Transform lookAtObject;
    [SerializeField, Range(0.0f, 1.0f)] float lookAtWeight; 

    private void OnAnimatorIK(int layerIndex)
    {
        if (lookAtObject != null)
        {
            anim.SetLookAtPosition(lookAtObject.position);
            anim.SetLookAtWeight(lookAtWeight);
        }
    }


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        anim.SetFloat("SpeedVertical", Input.GetAxis("Vertical"));
        anim.SetFloat("SpeedHorizontal", Input.GetAxis("Horizontal"));
    }
}
