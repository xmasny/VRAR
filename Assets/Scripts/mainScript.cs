using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainScript : MonoBehaviour
{
    public Material highlightMaterial;
    public Material oldMaterial;

    // vychodzie pozicie sachovych figurok
    private int[,] occupArray = new int[,] { { 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1 } };

    private bool selected = false;
    private int lastSelX;
    private int lastSelY;
    private string lastFieldName;
    private Transform lastField;

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
                    MeshRenderer meshRenderer = hit.transform.GetComponent<MeshRenderer>();
                    meshRenderer.material = highlightMaterial;

                    lastSelX = int.Parse(hit.transform.name) % 10;
                    lastSelY = int.Parse(hit.transform.name) / 10;
                    lastFieldName = GetWholeFieldName(hit.transform.name);
                    lastField = hit.transform;

                    selected = true;
                }
                else
                {
                    MeshRenderer meshRenderer = lastField.GetComponent<MeshRenderer>();
                    meshRenderer.material = oldMaterial;

                    selected = false;
                }

            }
        }
    }

    string GetWholeFieldName(string name)
    {
        if (lastSelY == 0) name.Insert(0, "0");
        return name;
    }


}
