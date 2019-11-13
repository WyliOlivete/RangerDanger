using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoacherState
{
    Running,
    Hitting
}
public class Poacher : MonoBehaviour
{
    private Transform target;
    private PoacherState state;
    [SerializeField]
    private Animator animator;
    private Ranger ranger;
    private PoacherAlert poacherAlert;
    private bool isApprehended;
    private Coroutine CApprehended;

    private void Start()
    {
        poacherAlert = GameManager.Instance.CreatePoacherAlert();
        poacherAlert.SetPoacher(this);
        poacherAlert.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up);
    }
    private void Update()
    {
        if (target)
        {
            if (Vector3.Distance(transform.position, target.position) > 0.5f)
            {
                if (state != PoacherState.Running)
                {
                    state = PoacherState.Running;
                    animator.SetTrigger("RUNNING");
                }
                transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(target.position.x, 0, target.position.z), Time.deltaTime);
                transform.rotation = Quaternion.LookRotation((new Vector3(target.position.x, 0, target.position.z) - new Vector3(transform.position.x, 0, transform.position.z)).normalized, Vector3.up);
            }
            else
            {
                if (state != PoacherState.Hitting)
                {
                    state = PoacherState.Hitting;
                    animator.SetTrigger("HITTING");
                    target.GetComponent<Animal>().Hunted();
                }
            }
        }
        else if (!isApprehended)
        {
            target = GameManager.Instance.GetClosestAnimal(transform.position);
            state = PoacherState.Running;
            animator.SetTrigger("RUNNING");
            if (target)
                target.GetComponent<Animal>().SetPoacher(this);
        }
        if (poacherAlert)
            poacherAlert.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up);
    }
    public void ClearTarget()
    {
        target = null;
    }
    public void Alerted()
    {
        if (!ranger)
        {
            Transform t = GameManager.Instance.GetClosestRanger(transform.position);
            if (t)
            {
                ranger = t.GetComponent<Ranger>();
                ranger.SetTarget(this);
            }
        }
        else
            ranger.SetTarget(this);
    }
    public void Apprehended()
    {
        isApprehended = true;
        poacherAlert.gameObject.SetActive(false);
        if (gameObject.activeSelf)
            CApprehended = StartCoroutine(IEApprehended());
    }
    private IEnumerator IEApprehended()
    {
        if (target)
        {
            target.GetComponent<Animal>().StopHunt();
            target = null;
        }
        yield return new WaitForSeconds(2f);
        ranger.ClearTarget();
        GameManager.Instance.RemovePoacher(this);
        StopCoroutine(CApprehended);
        gameObject.SetActive(false);
    }
    public void SetRanger(Ranger r)
    {
        ranger = r;
    }
}