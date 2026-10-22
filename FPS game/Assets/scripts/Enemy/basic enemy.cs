using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class basicenemy : Enemy
{
    Animator myAnim;

    void Start()
    {
        myAnim = GetComponent<Animator>();
    }


    void Update()
    {
        if (agent.velocity != Vector3.zero)
        {
            myAnim.SetBool("isWalking", true);
        }
        else
            myAnim.SetBool("isWalking", false);
    }
}
