using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    [SerializeField]
    private Transform mainField;
    [SerializeField]
    private GameObject prefabHill, prefabGrassHill, prefabLake, prefabForest;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }
    private void Start()
    {
        for (int i = 0; i < 16; i++)
        {
            float xyScale = Random.Range(0f, 2f), range = 7f;
            Vector3 scale = new Vector3(xyScale, xyScale, Random.Range(0f, xyScale * 0.0625f));
            if (xyScale < 1f)
                range -= xyScale;
            GameObject grassHill = Instantiate(prefabGrassHill, new Vector3(Random.Range(-range, range), 0f, Random.Range(-range, range)), Quaternion.Euler(-90f, Random.Range(0f, 360f), Random.Range(0f, 360f)), mainField);
            grassHill.transform.localScale = new Vector3(grassHill.transform.localScale.x * scale.x, grassHill.transform.localScale.y * scale.y, grassHill.transform.localScale.z * scale.z);
        }
        {
            float xyzScale = Random.Range(1f, 2f), range = 3f;
            if (xyzScale < 1f)
                range -= xyzScale;
            GameObject lake = Instantiate(prefabLake, new Vector3(Random.Range(-range, range), 0f, Random.Range(-range, range)), Quaternion.Euler(0, 0, 0), mainField);
            lake.transform.localScale = Vector3.one * xyzScale;
        }
        for (int i = 0; i < Random.Range(1, 2) * 4; i++)
        {
            float xyScale = Random.Range(0.25f, 1f), range = 7f;
            Vector3 scale = new Vector3(xyScale, xyScale, Random.Range(xyScale * 0.5f, xyScale));
            if (xyScale < 1f)
                range -= xyScale;
            GameObject hill = Instantiate(prefabHill, new Vector3(Random.Range(-range, range), 0f, Random.Range(-range, range)), Quaternion.Euler(-90f, Random.Range(0f, 360f), Random.Range(0f, 360f)), mainField);
            hill.transform.localScale = new Vector3(hill.transform.localScale.x * scale.x, hill.transform.localScale.y * scale.y, hill.transform.localScale.z * scale.z);
        }
        {
            float range = 11f;
            for (int x = 0; x < range; x++)
                for (int y = 0; y < range; y++)
                    Instantiate(prefabForest, new Vector3(x - range * 0.5f, 0f, y - range * 0.5f), Quaternion.Euler(0f, 0f, 0f), mainField);
        }
    }

    public Transform GetMainField()
    {
        return mainField;
    }
}