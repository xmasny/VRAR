using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePesiak : MonoBehaviour
{
    public Material highlightMaterial;
    public Material oldMaterial;

    private GameObject obj;
    private bool selected = false;

    void Start()
    {
    }
    void Update()
    {
        if( Input.GetMouseButtonDown(0) )
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if( Physics.Raycast(ray.origin, ray.direction, out hit) )
            {
                if (selected == false)
                {
                    obj = hit.transform.parent.gameObject;

                    MeshRenderer meshRenderer = obj.transform.GetChild(0).GetComponent<MeshRenderer>();
                    meshRenderer.material = highlightMaterial;

                    selected = true;
                }
                else
                {
                    obj = hit.transform.parent.gameObject;

                    MeshRenderer meshRenderer = obj.transform.GetChild(0).GetComponent<MeshRenderer>();
                    meshRenderer.material = oldMaterial;
                    Debug.Log(obj.transform.position);

                    selected = false;
                }

            }
        }
    }


}
