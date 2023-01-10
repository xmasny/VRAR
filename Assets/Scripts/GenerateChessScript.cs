using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateChessScript : MonoBehaviour
{
    public GameObject BIELI;
    public GameObject pesiak;
    public GameObject pesiak1;

    private GameObject _pesiak;
    private GameObject _pesiak1;

    void Start()
    {
        _pesiak = Instantiate(pesiak, new Vector3(0.7f, 0.95f, 0.4f), Quaternion.identity, BIELI.transform);
        _pesiak.name = "pesiak";
        _pesiak1 = Instantiate(pesiak, new Vector3(1.7f, 0.95f, 0.4f), Quaternion.identity, BIELI.transform);
        _pesiak1.name = "pesiak (1)";
    }

    void Update()
    {

    }
}
