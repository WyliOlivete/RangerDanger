 
using UnityEngine;
using System.Collections;
 
[ExecuteInEditMode]
public class SetDecalMater : MonoBehaviour
{
   public Transform proj;

    void Update()
    {
        SetMatr();
    }

    [ContextMenu("set matrix")]
    void SetMatr()
    {
        Matrix4x4 mat = proj.worldToLocalMatrix * transform.localToWorldMatrix;
        GetComponent<Renderer>().sharedMaterial.SetMatrix("_DecalMatr", mat);
    }
}