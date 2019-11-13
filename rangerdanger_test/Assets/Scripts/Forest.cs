using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabTree1, prefabTallGrass;
    [SerializeField]
    private List<GameObject> prefabBushes;
    //private bool locked = false;

    private void Start()
    {
        for (int i = 0; i < Random.Range(1, 2) * 4; i++)
        {
            float xyScale = Random.Range(0.75f, 1f), range = 2f;
            Vector3 scale = new Vector3(xyScale, xyScale, xyScale);
            if (xyScale < 1f)
                range -= xyScale;
            GameObject tree = Instantiate(prefabTree1, transform.position + new Vector3(Random.Range(-range, range), xyScale, Random.Range(-range, range)), Quaternion.Euler(-90f, Random.Range(0f, 360f), Random.Range(0f, 360f)), transform);
            tree.transform.localScale = new Vector3(tree.transform.localScale.x * scale.x, tree.transform.localScale.y * scale.y, tree.transform.localScale.z * scale.z);
        }
        for (int i = 0; i < Random.Range(0, 2) * 2; i++)
        {
            float xyScale = Random.Range(0.25f, 1f), range = 2f;
            Vector3 scale = new Vector3(xyScale, xyScale, xyScale);
            if (xyScale < 1f)
                range -= xyScale;
            GameObject bush = Instantiate(prefabBushes[0], transform.position + new Vector3(Random.Range(-range, range), 0f, Random.Range(-range, range)), Quaternion.Euler(-90f, Random.Range(0f, 360f), Random.Range(0f, 360f)), transform);
            bush.transform.localScale = new Vector3(bush.transform.localScale.x * scale.x, bush.transform.localScale.y * scale.y, bush.transform.localScale.z * scale.z);
        }
        for (int i = 0; i < Random.Range(0, 2) * 2; i++)
        {
            float xyScale = Random.Range(0.25f, 1f), range = 2f;
            Vector3 scale = new Vector3(xyScale, xyScale, xyScale);
            if (xyScale < 1f)
                range -= xyScale;
            GameObject bush = Instantiate(prefabBushes[1], transform.position + new Vector3(Random.Range(-range, range), xyScale * 0.5f, Random.Range(-range, range)), Quaternion.Euler(-90f, Random.Range(0f, 360f), Random.Range(0f, 360f)), transform);
            bush.transform.localScale = new Vector3(bush.transform.localScale.x * scale.x, bush.transform.localScale.y * scale.y, bush.transform.localScale.z * scale.z);
        }
        for (int i = 0; i < Random.Range(2, 4) * 8; i++)
        {
            float xyScale = Random.Range(1f, 1f), range = 2f;
            Vector3 scale = new Vector3(xyScale, xyScale, Random.Range(xyScale * 0.25f, xyScale));
            if (xyScale < 1f)
                range -= xyScale;
            GameObject tallGrass = Instantiate(prefabTallGrass, transform.position + new Vector3(Random.Range(-range, range), 0f, Random.Range(-range, range)), Quaternion.Euler(-90f, Random.Range(0f, 360f), Random.Range(0f, 360f)), transform);
            tallGrass.transform.localScale = new Vector3(tallGrass.transform.localScale.x * scale.x, tallGrass.transform.localScale.y * scale.y, tallGrass.transform.localScale.z * scale.z);
        }
    }
}