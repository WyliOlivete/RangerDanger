using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalRelease : MonoBehaviour
{
    private Animal animal;

    public void SetAnimal(Animal a)
    {
        animal = a;
    }

    public void Release()
    {
        animal.Release();
    }
}
