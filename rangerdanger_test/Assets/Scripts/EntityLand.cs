using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityLandType
{
    Bush,
    TallGrass,
    Tree
}
public class EntityLand : MonoBehaviour
{
    public EntityLandType entityLandType;
    private Collider[] hitColliders;
    private float timer = 1f;

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0f)
        {
            switch (entityLandType)
            {
                case EntityLandType.Tree:
                    hitColliders = Physics.OverlapSphere(transform.position + (Vector3.down * 0.625f), 0.125f, 1 << 8);
                    break;
                default:
                    hitColliders = Physics.OverlapSphere(transform.position + (Vector3.up * 0.125f), 0.0625f, 1 << 8);
                    break;
            }
            if (hitColliders.Length <= 0)
                gameObject.SetActive(false);
        }
    }
}