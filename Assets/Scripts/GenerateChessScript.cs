using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateChessScript : MonoBehaviour
{
    public GameObject BIELI;
    public GameObject pesiak;
    public GameObject veza;
    public GameObject kon;
    public GameObject strelec;
    public GameObject kralovna;
    public GameObject kral;

    public GameObject CIERNI;
    public GameObject Cpesiak;
    public GameObject Cveza;
    public GameObject Ckon;
    public GameObject Cstrelec;
    public GameObject Ckralovna;
    public GameObject Ckral;
    void Start()
    {
        // BIELI
        Instantiate(pesiak, new Vector3(0.7f, 0.95f, 0.4f), Quaternion.identity, BIELI.transform).name = "pesiak";
        for (int i = 0; i < 7; i++)
        {
            Instantiate(pesiak, new Vector3(1.7f + i, 0.95f, 0.4f), Quaternion.identity, BIELI.transform).name = $"pesiak ({i+1})";
        }
        Instantiate(veza, new Vector3(-0.7f, 0.95f, 0.6f), Quaternion.identity, BIELI.transform).name = "veza";
        Instantiate(veza, new Vector3(-7.7f, 0.95f, 0.6f), Quaternion.identity, BIELI.transform).name = "veza (1)";
        Instantiate(kon, new Vector3(4.3f, 0.95f, -1.8f), Quaternion.identity, BIELI.transform).name = "kon";
        Instantiate(kon, new Vector3(-0.7f, 0.95f, -1.8f), Quaternion.identity, BIELI.transform).name = "kon (1)";
        Instantiate(strelec, new Vector3(0.9f, 0.95f, -1.8f), Quaternion.identity, BIELI.transform).name = "strelec";
        Instantiate(strelec, new Vector3(-2.1f, 0.95f, -1.8f), Quaternion.identity, BIELI.transform).name = "strelec (1)";
        Instantiate(kralovna, new Vector3(1.1f, 0.95f, 0.6f), Quaternion.identity, BIELI.transform).name = "kralovna";
        Instantiate(kral, new Vector3(-1.1f, 0.95f, 0.6f), Quaternion.identity, BIELI.transform).name = "kral";

        // CIERNI
        Instantiate(Cpesiak, new Vector3(7.7f, 0.95f, 5.4f), Quaternion.identity, CIERNI.transform).name = "Cpesiak";
        for (int i = 0; i < 7; i++)
        {
            Instantiate(Cpesiak, new Vector3(6.7f - i, 0.95f, 5.4f), Quaternion.identity, CIERNI.transform).name = $"Cpesiak ({i+1})";
        }
        Instantiate(Cveza, new Vector3(-0.7f, 0.95f, 7.6f), Quaternion.identity, CIERNI.transform).name = "Cveza";
        Instantiate(Cveza, new Vector3(-7.7f, 0.95f, 7.6f), Quaternion.identity, CIERNI.transform).name = "Cveza (1)";
        Instantiate(Ckon, new Vector3(0.7f, 0.95f, 1.7f), Quaternion.identity, CIERNI.transform).name = "Ckon";
        Instantiate(Ckon, new Vector3(-4.3f, 0.95f, 1.7f), Quaternion.identity, CIERNI.transform).name = "Ckon (1)";
        Instantiate(Cstrelec, new Vector3(0.9f, 0.95f, 5.2f), Quaternion.identity, CIERNI.transform).name = "Cstrelec";
        Instantiate(Cstrelec, new Vector3(-2.1f, 0.95f, 5.2f), Quaternion.identity, CIERNI.transform).name = "Cstrelec (1)";
        Instantiate(Ckralovna, new Vector3(1.1f, 0.95f, 7.6f), Quaternion.identity, CIERNI.transform).name = "Ckralovna";
        Instantiate(Ckral, new Vector3(-1.1f, 0.95f, 7.6f), Quaternion.identity, CIERNI.transform).name = "Ckral";
    }

    void Update()
    {

    }
}
