using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RangerState
{
    Walking,
    Running,
    Interacting
}
public class Ranger : MonoBehaviour
{
    private Transform target;
    private RangerState state;
    [SerializeField]
    private Animator animator;
    private bool isApprehending;

    private void Update()
    {
        if (target)
        {
            if (target.GetComponent<Poacher>())
            {
                if (Vector3.Distance(transform.position, target.position) > 0.5f)
                {
                    if (state != RangerState.Running)
                    {
                        state = RangerState.Running;
                        animator.SetTrigger("RUNNING");
                    }
                    transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(target.position.x, 0, target.position.z), Time.deltaTime * 2f);
                    transform.rotation = Quaternion.LookRotation((new Vector3(target.position.x, 0, target.position.z) - new Vector3(transform.position.x, 0, transform.position.z)).normalized, Vector3.up);
                }
                else
                {
                    if (state != RangerState.Interacting)
                    {
                        state = RangerState.Interacting;
                        animator.SetTrigger("INTERACTING");
                        target.GetComponent<Poacher>().Apprehended();
                    }
                }
            }
            else if (target.GetComponent<Animal>())
            {
                if (Vector3.Distance(transform.position, target.position) > 0.5f)
                {
                    if (state != RangerState.Walking)
                    {
                        state = RangerState.Walking;
                        animator.SetTrigger("WALKING");
                    }
                    transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(target.position.x, 0, target.position.z), Time.deltaTime * 0.5f);
                    transform.rotation = Quaternion.LookRotation((new Vector3(target.position.x, 0, target.position.z) - new Vector3(transform.position.x, 0, transform.position.z)).normalized, Vector3.up);
                }
                else
                {
                    if (state != RangerState.Interacting)
                    {
                        state = RangerState.Interacting;
                        animator.SetTrigger("INTERACTING");
                    }
                }
            }
        }
        else if (!isApprehending)
        {
            target = GameManager.Instance.GetClosestAnimal(transform.position);
            state = RangerState.Walking;
            animator.SetTrigger("WALKING");
            if (target)
                target.GetComponent<Animal>().SetRanger(this);
        }
    }
    public void SetTarget(Poacher p)
    {
        if (!isApprehending)
        {
            target = p.transform;
            isApprehending = true;
        }
    }
    public void ClearTarget()
    {
        target = null;
        isApprehending = false;
    }
    public bool IsApprehending()
    {
        return isApprehending;
    }
}