using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimalAreaType
{
    Land,
    Air,
    Water
}
public class Animal : MonoBehaviour
{
    public AnimalAreaType areaType;
    public AnimalType animalType;
    private Collider[] hitColliders;
    private float timer = 1f;
    private List<Poacher> poachers = new List<Poacher>();
    private List<Ranger> rangers = new List<Ranger>();
    private Coroutine CHunted;
    private float releaseTimer;
    private AnimalRelease animalRelease;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        releaseTimer = Random.Range(4f, 6f);
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0f)
        {
            switch (areaType)
            {
                case AnimalAreaType.Land:
                    hitColliders = Physics.OverlapSphere(transform.position, 0.125f, 1 << 8);
                    if (hitColliders.Length <= 0)
                        transform.position = new Vector3(Random.Range(-5.5f, 5.5f), 0f, Random.Range(-5.5f, 5.5f));
                    break;
                case AnimalAreaType.Water:
                    hitColliders = Physics.OverlapSphere(transform.position, 0.125f, 1 << 8);
                    if (hitColliders.Length > 0)
                        transform.position = new Vector3(Random.Range(-5.5f, 5.5f), 0f, Random.Range(-5.5f, 5.5f));
                    break;
                case AnimalAreaType.Air:
                    hitColliders = Physics.OverlapSphere(transform.position, 0.125f, 1 << 8);
                    if (hitColliders.Length > 0)
                        transform.position = new Vector3(Random.Range(-5.5f, 5.5f), 0f, Random.Range(-5.5f, 5.5f));
                    break;
                default:
                    break;
            }
        }

        releaseTimer -= Time.deltaTime;
        if (!animalRelease && releaseTimer <= 0)
        {
            animalRelease = GameManager.Instance.CreateAnimalRelease();
            animalRelease.SetAnimal(this);
            animalRelease.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up);
        }
        if (animalRelease)
            animalRelease.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up);
    }

    public void Hunted()
    {
        if (gameObject.activeSelf)
            CHunted = StartCoroutine(IEHunted());
    }
    private IEnumerator IEHunted()
    {
        yield return new WaitForSeconds(2f);
        foreach (Poacher p in poachers)
            p.ClearTarget();
        foreach (Ranger r in rangers)
            r.ClearTarget();
        GameManager.Instance.RemoveAnimal(this);
        if (CHunted != null)
            StopCoroutine(CHunted);
        if (animalRelease)
            animalRelease.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    public void StopHunt()
    {
        if (CHunted != null)
            StopCoroutine(CHunted);
    }
    public void SetPoacher(Poacher p)
    {
        poachers.Add(p);
    }
    public void SetRanger(Ranger r)
    {
        rangers.Add(r);
    }
    public void Release()
    {
        foreach (Poacher p in poachers)
            p.ClearTarget();
        foreach (Ranger r in rangers)
            r.ClearTarget();
        GameManager.Instance.RemoveAnimal(this);
        GameManager.Instance.ReleaseAnimal();
        GameManager.Instance.OpenProfile(animalType);
        if (CHunted != null)
            StopCoroutine(CHunted);
        if (animalRelease)
            animalRelease.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}