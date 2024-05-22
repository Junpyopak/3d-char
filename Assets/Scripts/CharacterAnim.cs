using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterAnim : MonoBehaviour
{
    Animator anim;

    [SerializeField] Transform lookAtObject;
    [SerializeField, Range(0.0f, 1.0f)] float lookAtWeight;

    [SerializeField, Range(0, 1f)] float distanceToGround;//발바닥이 어케 되는지

    bool modeChange = false;
    private void OnAnimatorIK(int layerIndex)
    {
        if (lookAtObject != null)
        {
            anim.SetLookAtPosition(lookAtObject.position);
            anim.SetLookAtWeight(lookAtWeight);
        }

        anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
        anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
        anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);


        Vector3 leftfootIkPos = anim.GetIKPosition(AvatarIKGoal.LeftFoot);
        Vector3 rightfootIkPos = anim.GetIKPosition(AvatarIKGoal.RightFoot);
        if (Physics.Raycast(leftfootIkPos + Vector3.up, Vector3.down, out RaycastHit lefthit, distanceToGround + 1f, LayerMask.GetMask("Ground")))
        {
            Vector3 leftFootPos = lefthit.point;
            leftFootPos.y += distanceToGround;

            anim.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootPos);

            Vector3 vecFoward = Vector3.ProjectOnPlane(transform.forward, lefthit.normal);
            Vector3 vecUpward = lefthit.normal;

            anim.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(vecFoward, vecUpward));
        }
        if (Physics.Raycast(rightfootIkPos + Vector3.up, Vector3.down, out RaycastHit righthit, distanceToGround + 1f, LayerMask.GetMask("Ground")))
        {
            Vector3 rightFootPos = righthit.point;
            rightFootPos.y += distanceToGround;

            anim.SetIKPosition(AvatarIKGoal.RightFoot, rightFootPos);
            Vector3 vecFoward = Vector3.ProjectOnPlane(transform.forward, lefthit.normal);
            Vector3 vecUpward = lefthit.normal;
            anim.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(vecFoward, vecUpward));

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
        movingAnim();
        checkAnim();
    }

    private void movingAnim()
    {
        anim.SetFloat("SpeedVertical", Input.GetAxis("Vertical"));
        anim.SetFloat("SpeedHorizontal", Input.GetAxis("Horizontal"));
    }
    private void checkAnim()
    {
        if (modeChange == false && Input.GetKeyDown(KeyCode.LeftControl))
        {
            modeChange = true;
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                StartCoroutine(changeState(false));
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                StartCoroutine(changeState(true));
            }
        }
    }
    /// <summary>
    /// 전투모드가 되면 에니메이션의 레이어값을 변경
    /// </summary>
    /// <param name="_upper"></param>
    /// <returns></returns>
    IEnumerator changeState(bool _upper)// 코르틴
    {
        float time = 0f;
        if (_upper == true)
        {
            while (anim.GetLayerWeight(1) < 1)
            {
                time += Time.deltaTime;
                if (time > 1)
                {
                    time = 1;
                }
                anim.SetLayerWeight(1, Mathf.Lerp(0, 1, time));
                yield return null;
            }
        }
        else
        {
            while (anim.GetLayerWeight(1) > 0)
            {
                time += Time.deltaTime;
                if (time > 1)
                {
                    time = 1;
                }
                anim.SetLayerWeight(1, Mathf.Lerp(1, 0, time));
                yield return null;
            }
        }
        modeChange = false;
    }
}
