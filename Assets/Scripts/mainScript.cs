using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScript : MonoBehaviour
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
    public Material highlightMaterial;

    public Material highlightMaterial2;

    public Material highlightMaterial3;
    public Material defaultMaterial;
    public Material blackMaterial;
    public Material whiteMaterial;

    public GameObject turnField;

    public float speed;

    // vychodzie pozicie sachovych figurok - cisla reprezentuju typ, poradie a tim
    // prva cislica je typ, druha je iteracia a tretia je tim
    // 1 - pesiak
    // 2 - veza
    // 3 - kon
    // 4 - strelec
    // 5 - kralovna
    // 6 - kral
    private int[,] occupArray = new int[8,8];

    private bool selected = false;
    private int lastSelX;
    private int lastSelY;
    private string lastFieldName;
    private Transform lastField;
    private int newSelX;
    private int newSelY;
    private string newFieldName;
    private Transform newField;
    private List<string> possibleNextFieldsFree;
    private List<string> possibleNextFieldsOccupiedByEnemy;
    private int turn = 0;

    private int selectedFigure;
    private string selectedFigureName;

    private int figureToDelete;
    private string figureToDeleteName;
    private string path = @"Assets\SavedGame\savedGame.txt";
    private Vector3 destination;

    void Start()
    {
        bool isContinue = PlayerPrefs.GetInt("CONTINUE") == 1;

        if (isContinue)
        {
            turn = PlayerPrefs.GetInt("TURN");

            string[] lines = System.IO.File.ReadAllLines(path);
            int i = 0;
            foreach (string line in lines)
            {
                string[] cols = line.Split(", ");
                int j = 0;
                foreach (string col in cols)
                {
                    occupArray[i,j] = int.Parse(col);
                    j++;
                }
                i++;
            }
        }
        else
        {
            occupArray = new int[,] {
                { 210, 310, 410, 600, 500, 400, 300, 200 },
                { 100, 110, 120, 130, 140, 150, 160, 170 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 171, 161, 151, 141, 131, 121, 111, 101 },
                { 211, 311, 411, 601, 501, 401, 301, 201 }
            };
        }
        MeshRenderer mr = turnField.transform.GetComponent<MeshRenderer>();
        mr.material = turn == 0 ? whiteMaterial : blackMaterial;

        // BIELI
        if (isInOccup(100)) Instantiate(pesiak, new Vector3(0.7f, 0.95f, -0.6f), Quaternion.identity, BIELI.transform).name = "pesiak";
        for (int i = 0; i < 7; i++)
        {
            if (isInOccup(100 + (i+1) * 10)) Instantiate(pesiak, new Vector3(0.7f, 0.95f, -0.6f), Quaternion.identity, BIELI.transform).name = $"pesiak ({i+1})";
        }
        if (isInOccup(200)) Instantiate(veza, new Vector3(-7.7f, 0.95f, 0.6f), Quaternion.identity, BIELI.transform).name = "veza";
        if (isInOccup(210)) Instantiate(veza, new Vector3(-7.7f, 0.95f, 0.6f), Quaternion.identity, BIELI.transform).name = "veza (1)";
        if (isInOccup(300)) Instantiate(kon, new Vector3(-1.7f, 0.95f, -1.8f), Quaternion.identity, BIELI.transform).name = "kon";
        if (isInOccup(310)) Instantiate(kon, new Vector3(-1.7f, 0.95f, -1.8f), Quaternion.identity, BIELI.transform).name = "kon (1)";
        if (isInOccup(400)) Instantiate(strelec, new Vector3(-4.1f, 0.95f, -1.8f), Quaternion.identity, BIELI.transform).name = "strelec";
        if (isInOccup(410)) Instantiate(strelec, new Vector3(-4.1f, 0.95f, -1.8f), Quaternion.identity, BIELI.transform).name = "strelec (1)";
        if (isInOccup(500)) Instantiate(kralovna, new Vector3(-2.9f, 0.95f, 0.6f), Quaternion.identity, BIELI.transform).name = "kralovna";
        if (isInOccup(600)) Instantiate(kral, new Vector3(-4.1f, 0.95f, 0.6f), Quaternion.identity, BIELI.transform).name = "kral";

        // CIERNI
        if (isInOccup(101)) Instantiate(Cpesiak, new Vector3(0.7f, 0.95f, -0.6f), Quaternion.identity, CIERNI.transform).name = "Cpesiak";
        for (int i = 0; i < 7; i++)
        {
            if (isInOccup(101 + (i+1) * 10)) Instantiate(Cpesiak, new Vector3(0.7f, 0.95f, -0.6f), Quaternion.identity, CIERNI.transform).name = $"Cpesiak ({i+1})";
        }
        if (isInOccup(201)) Instantiate(Cveza, new Vector3(-7.7f, 0.95f, 0.6f), Quaternion.identity, CIERNI.transform).name = "Cveza";
        if (isInOccup(211)) Instantiate(Cveza, new Vector3(-7.7f, 0.95f, 0.6f), Quaternion.identity, CIERNI.transform).name = "Cveza (1)";
        if (isInOccup(301)) Instantiate(Ckon, new Vector3(-5.2f, 0.95f, -5.2f), Quaternion.identity, CIERNI.transform).name = "Ckon";
        if (isInOccup(311)) Instantiate(Ckon, new Vector3(-5.2f, 0.95f, -5.2f), Quaternion.identity, CIERNI.transform).name = "Ckon (1)";
        if (isInOccup(401)) Instantiate(Cstrelec, new Vector3(-4.1f, 0.95f, -1.8f), Quaternion.identity, CIERNI.transform).name = "Cstrelec";
        if (isInOccup(411)) Instantiate(Cstrelec, new Vector3(-4.1f, 0.95f, -1.8f), Quaternion.identity, CIERNI.transform).name = "Cstrelec (1)";
        if (isInOccup(501)) Instantiate(Ckralovna, new Vector3(-2.9f, 0.95f, 0.6f), Quaternion.identity, CIERNI.transform).name = "Ckralovna";
        if (isInOccup(601)) Instantiate(Ckral, new Vector3(-4.1f, 0.95f, 0.6f), Quaternion.identity, CIERNI.transform).name = "Ckral";

        moveFiguresToCorrectPositions();
    }

    bool isInOccup(int val)
    {
        bool isIn = false;
        for (int k = 0; k < 8; k++)
            for (int l = 0; l < 8; l++)
                if (occupArray[k, l] == val) isIn = true;
        return isIn;

    }

    void Update()
    {
        if (selected == false && selectedFigureName != null && destination != Vector3.zero && destination != GameObject.Find(selectedFigureName).transform.localPosition) {
            IncrementAnimationPosition();
        }
        if( Input.GetMouseButtonDown(0) )
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if( Physics.Raycast(ray.origin, ray.direction, out hit) )
            {
                if (selected == true)
                {
                    newSelX = int.Parse(hit.transform.name) % 10;
                    newSelY = int.Parse(hit.transform.name) / 10;
                    newFieldName = GetWholeFieldName(hit.transform.name);
                    newField = hit.transform;


                    if (possibleNextFieldsFree.Contains(newFieldName))
                    {
                        // zmena poradia, ide super
                        ChangeTurn();

                        // presun figurky
                        Vector3 moveVector = CalculateMoveVector();
                        destination = GameObject.Find(selectedFigureName).transform.localPosition + moveVector;

                        occupArray[lastSelY,lastSelX] = 0;
                        occupArray[newSelY,newSelX] = selectedFigure;
                        UnFocus();
                        return;
                    }
                    if (possibleNextFieldsOccupiedByEnemy.Contains(newFieldName))
                    {
                        // zmena poradia, ide super
                        ChangeTurn();

                        // vymazanie starej figurky
                        figureToDelete = occupArray[newSelY,newSelX];
                        figureToDeleteName = GetFigTeam(figureToDelete) + GetFigType(figureToDelete) + GetFigIteration(figureToDelete);
                        Destroy(GameObject.Find(figureToDeleteName));

                        // presun novej figurky
                        Vector3 moveVector = CalculateMoveVector();
                        destination = GameObject.Find(selectedFigureName).transform.localPosition + moveVector;

                        occupArray[lastSelY,lastSelX] = 0;
                        occupArray[newSelY,newSelX] = selectedFigure;
                        UnFocus();
                        if (figureToDelete == 600 || figureToDelete == 601) EndGame();
                        return;
                    }
                    destination = Vector3.zero;

                    UnFocus();
                }

                lastSelX = int.Parse(hit.transform.name) % 10;
                lastSelY = int.Parse(hit.transform.name) / 10;
                lastFieldName = GetWholeFieldName(hit.transform.name);
                lastField = hit.transform;

                selectedFigure = occupArray[lastSelY,lastSelX];

                // iba ked je na rade
                if (!IsFieldFree(lastSelY, lastSelX) && GetFigTeamNumber(selectedFigure) == turn)
                {
                    MeshRenderer meshRenderer = hit.transform.GetComponent<MeshRenderer>();
                    meshRenderer.material = highlightMaterial;

                    selected = true;

                    if ( selectedFigure / 100 == 0 ) return;
                    selectedFigureName = GetFigTeam(selectedFigure) + GetFigType(selectedFigure) + GetFigIteration(selectedFigure);

                    // calculate possible next moves
                    (possibleNextFieldsFree, possibleNextFieldsOccupiedByEnemy) = FindPossibleNextFields(selectedFigure % 2);
                    // highlight free fields
                    possibleNextFieldsFree.ForEach(possibleFieldName => {
                        MeshRenderer mr = GameObject.Find(possibleFieldName).transform.GetComponent<MeshRenderer>();
                        mr.material = highlightMaterial2;
                    });
                    // highlight enemy-occupied fields
                    possibleNextFieldsOccupiedByEnemy.ForEach(possibleFieldName => {
                        MeshRenderer mr = GameObject.Find(possibleFieldName).transform.GetComponent<MeshRenderer>();
                        mr.material = highlightMaterial3;
                    });
                }

            } else if (lastField) UnFocus();
        }
    }

    void moveFiguresToCorrectPositions()
    {
        for (int k = 0; k < 8; k++)
            for (int l = 0; l < 8; l++)
            {
                if (occupArray[k, l] == 0) continue;
                else
                {
                    string figNameFull = GetFigTeam(occupArray[k, l]) + GetFigType(occupArray[k, l]) + GetFigIteration(occupArray[k, l]);
                    GameObject.Find(figNameFull).transform.Translate(new Vector3(l, 0, k));
                }
            }
    }

    void UnFocus()
    {
        MeshRenderer oldMeshRenderer = lastField.GetComponent<MeshRenderer>();
        oldMeshRenderer.material = defaultMaterial;
        selected = false;
        possibleNextFieldsFree.ForEach(possibleFieldName => {
            MeshRenderer mr = GameObject.Find(possibleFieldName).transform.GetComponent<MeshRenderer>();
            mr.material = defaultMaterial;
        });
        possibleNextFieldsOccupiedByEnemy.ForEach(possibleFieldName => {
            MeshRenderer mr = GameObject.Find(possibleFieldName).transform.GetComponent<MeshRenderer>();
            mr.material = defaultMaterial;
        });

        newSelX = 0;
        newSelY = 0;
        newFieldName = null;
        newField = null;
        lastSelX = 0;
        lastSelY = 0;
        lastFieldName = null;
        lastField = null;
        possibleNextFieldsFree = new List<string>();
    }

    bool IsFieldFree(int y, int x)
    {
        if (y>7 || x>7 || y<0 || x<0) return false;
        return occupArray[y,x] == 0;
    }

    bool IsFieldOccupiedByEnemy(int y, int x)
    {
        if (y>7 || x>7 || y<0 || x<0) return false;
        else if (occupArray[y,x] == 0) return false;
        else return (occupArray[y,x] % 2 != GetFigTeamNumber(selectedFigure));
    }

    Vector3 CalculateMoveVector()
    {
        return new Vector3(newSelX - lastSelX, 0, newSelY - lastSelY);
    }

    void ChangeTurn()
    {
        turn = 1 - turn;
        MeshRenderer mr = turnField.transform.GetComponent<MeshRenderer>();
        mr.material = turn == 0 ? whiteMaterial : blackMaterial;
    }

    string GetFigType(int figure)
    {

        switch (figure / 100)
        {
        case 1:
            return "pesiak";
        case 2:
            return "veza";
        case 3:
            return "kon";
        case 4:
            return "strelec";
        case 5:
            return "kralovna";
        case 6:
            return "kral";
        default:
            return "pesiak";
        }
    }

    string GetFigTeam(int figure)
    {
        if (figure.ToString().EndsWith("1")) return "C"; else return "";
    }

    int GetFigTeamNumber(int figure)
    {
        if (figure.ToString().EndsWith("1")) return 1; else return 0;
    }

    string GetFigIteration(int figure)
    {
        int iter = (figure % 100) / 10;
        if ( iter != 0)
        {
            return $" ({iter})";
        } else return "";
    }

    string GetWholeFieldName(string name)
    {
        if (lastSelY == 0) name.Insert(0, "0");
        return name;
    }
    string GetWholeFieldNameFromXY(int y, int x)
    {
        return y.ToString() + x.ToString();
    }

    int mod(int x, int m) {
        return (x%m + m)%m;
    }

    void IncrementAnimationPosition()
    {
        // Calculate the next position
        float delta = speed * Time.deltaTime;
        Vector3 currentPosition = GameObject.Find(selectedFigureName).transform.localPosition;
        Vector3 nextPosition = Vector3.zero;

        if (Vector3.Distance(currentPosition, destination) > 0.1) {
            nextPosition = Vector3.Lerp (currentPosition, destination, delta);
        } else {
            nextPosition = destination;
        }
        // Move the object to the next position
        GameObject.Find(selectedFigureName).transform.localPosition = nextPosition;
    }





    public (List<string>list1, List<string> list2) FindPossibleNextFields(int team)
    {
        List<string> nextFields = new List<string>();
        List<string> nextFieldsOccupiedByEnemy = new List<string>();
        int i = 1;
        int j;
        if ( GetFigTeam(selectedFigure) == "C" ) i = -1;

        switch (selectedFigure / 100)
        {
            // pesiak
            case 1:
                if (IsFieldFree(lastSelY+1*i, lastSelX)) nextFields.Add(GetWholeFieldNameFromXY(lastSelY+1*i, lastSelX));
                if (mod(i,7) == lastSelY && IsFieldFree(lastSelY+1*i, lastSelX)) {
                    if (IsFieldFree(lastSelY+2*i, lastSelX)) nextFields.Add(GetWholeFieldNameFromXY(lastSelY+2*i, lastSelX));
                }
                // vyhadzovanie pesiakov je specialne
                if (IsFieldOccupiedByEnemy(lastSelY+1*i, lastSelX+1)) nextFieldsOccupiedByEnemy.Add(GetWholeFieldNameFromXY(lastSelY+1*i, lastSelX+1));
                if (IsFieldOccupiedByEnemy(lastSelY+1*i, lastSelX-1)) nextFieldsOccupiedByEnemy.Add(GetWholeFieldNameFromXY(lastSelY+1*i, lastSelX-1));

                break;
            // veza
            case 2:
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY+j, lastSelX))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY+j, lastSelX));
                    } else if (IsFieldOccupiedByEnemy(lastSelY+j, lastSelX)) {
                        nextFieldsOccupiedByEnemy.Add(GetWholeFieldNameFromXY(lastSelY+j, lastSelX));
                        break;
                    } else break;
                    j++;
                }
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY, lastSelX+j))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY, lastSelX+j));
                    } else if (IsFieldOccupiedByEnemy(lastSelY, lastSelX+j)) {
                        nextFieldsOccupiedByEnemy.Add(GetWholeFieldNameFromXY(lastSelY, lastSelX+j));
                        break;
                    } else break;
                    j++;
                }
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY-j, lastSelX))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY-j, lastSelX));
                    } else if (IsFieldOccupiedByEnemy(lastSelY-j, lastSelX)) {
                        nextFieldsOccupiedByEnemy.Add(GetWholeFieldNameFromXY(lastSelY-j, lastSelX));
                        break;
                    } else break;
                    j++;
                }
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY, lastSelX-j))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY, lastSelX-j));
                    } else if (IsFieldOccupiedByEnemy(lastSelY, lastSelX-j)) {
                        nextFieldsOccupiedByEnemy.Add(GetWholeFieldNameFromXY(lastSelY, lastSelX-j));
                        break;
                    } else break;
                    j++;
                }
                break;
            // kon
            case 3:
                CalculateField(lastSelY+1, lastSelX+2);
                CalculateField(lastSelY+2, lastSelX+1);
                CalculateField(lastSelY+2, lastSelX-1);
                CalculateField(lastSelY+1, lastSelX-2);
                CalculateField(lastSelY-1, lastSelX+2);
                CalculateField(lastSelY-2, lastSelX+1);
                CalculateField(lastSelY-2, lastSelX-1);
                CalculateField(lastSelY-1, lastSelX-2);
                break;
            // strelec
            case 4:
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY+j, lastSelX+j))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY+j, lastSelX+j));
                    } else if (IsFieldOccupiedByEnemy(lastSelY+j, lastSelX+j)) {
                        nextFieldsOccupiedByEnemy.Add(GetWholeFieldNameFromXY(lastSelY+j, lastSelX+j));
                        break;
                    } else break;
                    j++;
                }
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY-j, lastSelX+j))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY-j, lastSelX+j));
                    } else if (IsFieldOccupiedByEnemy(lastSelY-j, lastSelX+j)) {
                        nextFieldsOccupiedByEnemy.Add(GetWholeFieldNameFromXY(lastSelY-j, lastSelX+j));
                        break;
                    } else break;
                    j++;
                }
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY-j, lastSelX-j))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY-j, lastSelX-j));
                    } else if (IsFieldOccupiedByEnemy(lastSelY-j, lastSelX-j)) {
                        nextFieldsOccupiedByEnemy.Add(GetWholeFieldNameFromXY(lastSelY-j, lastSelX-j));
                        break;
                    } else break;
                    j++;
                }
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY+j, lastSelX-j))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY+j, lastSelX-j));
                    } else if (IsFieldOccupiedByEnemy(lastSelY+j, lastSelX-j)) {
                        nextFieldsOccupiedByEnemy.Add(GetWholeFieldNameFromXY(lastSelY+j, lastSelX-j));
                        break;
                    } else break;
                    j++;
                }
                break;
            // Kralovna
            case 5:
            j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY+j, lastSelX))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY+j, lastSelX));
                    } else if (IsFieldOccupiedByEnemy(lastSelY+j, lastSelX)) {
                        nextFieldsOccupiedByEnemy.Add(GetWholeFieldNameFromXY(lastSelY+j, lastSelX));
                        break;
                    } else break;
                    j++;
                }
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY, lastSelX+j))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY, lastSelX+j));
                    } else if (IsFieldOccupiedByEnemy(lastSelY, lastSelX+j)) {
                        nextFieldsOccupiedByEnemy.Add(GetWholeFieldNameFromXY(lastSelY, lastSelX+j));
                        break;
                    } else break;
                    j++;
                }
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY-j, lastSelX))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY-j, lastSelX));
                    } else if (IsFieldOccupiedByEnemy(lastSelY-j, lastSelX)) {
                        nextFieldsOccupiedByEnemy.Add(GetWholeFieldNameFromXY(lastSelY-j, lastSelX));
                        break;
                    } else break;
                    j++;
                }
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY, lastSelX-j))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY, lastSelX-j));
                    } else if (IsFieldOccupiedByEnemy(lastSelY, lastSelX-j)) {
                        nextFieldsOccupiedByEnemy.Add(GetWholeFieldNameFromXY(lastSelY, lastSelX-j));
                        break;
                    } else break;
                    j++;
                }
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY+j, lastSelX+j))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY+j, lastSelX+j));
                    } else if (IsFieldOccupiedByEnemy(lastSelY+j, lastSelX+j)) {
                        nextFieldsOccupiedByEnemy.Add(GetWholeFieldNameFromXY(lastSelY+j, lastSelX+j));
                        break;
                    } else break;
                    j++;
                }
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY-j, lastSelX+j))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY-j, lastSelX+j));
                    } else if (IsFieldOccupiedByEnemy(lastSelY-j, lastSelX+j)) {
                        nextFieldsOccupiedByEnemy.Add(GetWholeFieldNameFromXY(lastSelY-j, lastSelX+j));
                        break;
                    } else break;
                    j++;
                }
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY-j, lastSelX-j))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY-j, lastSelX-j));
                    } else if (IsFieldOccupiedByEnemy(lastSelY-j, lastSelX-j)) {
                        nextFieldsOccupiedByEnemy.Add(GetWholeFieldNameFromXY(lastSelY-j, lastSelX-j));
                        break;
                    } else break;
                    j++;
                }
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY+j, lastSelX-j))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY+j, lastSelX-j));
                    } else if (IsFieldOccupiedByEnemy(lastSelY+j, lastSelX-j)) {
                        nextFieldsOccupiedByEnemy.Add(GetWholeFieldNameFromXY(lastSelY+j, lastSelX-j));
                        break;
                    } else break;
                    j++;
                }
                break;
            // Kral
            case 6:
                CalculateField(lastSelY+1, lastSelX);
                CalculateField(lastSelY-1, lastSelX);
                CalculateField(lastSelY, lastSelX+1);
                CalculateField(lastSelY, lastSelX-1);
                CalculateField(lastSelY-1, lastSelX-1);
                CalculateField(lastSelY-1, lastSelX+1);
                CalculateField(lastSelY+1, lastSelX-1);
                CalculateField(lastSelY+1, lastSelX+1);
                break;
            default:
                break;
        }
        return (nextFields, nextFieldsOccupiedByEnemy);

        void CalculateField(int y, int x)
        {
            if (IsFieldFree(y, x)) nextFields.Add(GetWholeFieldNameFromXY(y, x));
            if (IsFieldOccupiedByEnemy(y, x)) nextFieldsOccupiedByEnemy.Add(GetWholeFieldNameFromXY(y, x));
        }
    }

    void EndGame()
    {
        turn = 0;
        occupArray = new int[,] {
            { 210, 310, 410, 600, 500, 400, 300, 200 },
            { 100, 110, 120, 130, 140, 150, 160, 170 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 171, 161, 151, 141, 131, 121, 111, 101 },
            { 211, 311, 411, 601, 501, 401, 301, 201 }
        };
        SceneManager.LoadScene(0);
    }

    void OnDisable()
    {
        PlayerPrefs.SetInt("TURN", turn);

        try
        {
            // Create the file, or overwrite if the file exists.
            using (FileStream fs = File.Create(path))
            {
                for (int k = 0; k < 8; k++)
                {
                    int[] row = new int[8];
                    for (int l = 0; l < 8; l++)
                    {
                        row[l] = occupArray[k, l];
                    }
                    byte[] line = new UTF8Encoding(true).GetBytes(string.Join(", ", row));
                    fs.Write(line, 0, line.Length);
                    byte[] newline = Encoding.ASCII.GetBytes(Environment.NewLine);
                    fs.Write(newline, 0, newline.Length);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}
