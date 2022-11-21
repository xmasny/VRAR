using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainScript : MonoBehaviour
{
    public Material highlightMaterial;

    public Material highlightMaterial2;
    public Material defaultMaterial;

    // vychodzie pozicie sachovych figurok - cisla reprezentuju typ, poradie a tim
    // prva cislica je typ, druha je iteracia a tretia je tim
    // 1 - pesiak
    // 2 - veza
    // 3 - kon
    // 4 - strelec
    // 5 - kralovna
    // 6 - kral
    private int[,] occupArray;

    private bool selected = false;
    private int lastSelX;
    private int lastSelY;
    private string lastFieldName;
    private Transform lastField;
    private int newSelX;
    private int newSelY;
    private string newFieldName;
    private Transform newField;
    private List<string> possibleNextFields;

    private int selectedFigure;
    private string selectedFigureName;

    void Start()
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
    void Update()
    {
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


                    if (possibleNextFields.Contains(newFieldName))
                    {
                        Vector3 moveVector = CalculateMoveVector();
                        GameObject.Find(selectedFigureName).transform.Translate(moveVector);
                        occupArray[lastSelY,lastSelX] = 0;
                        occupArray[newSelY,newSelX] = selectedFigure;
                        UnFocus();
                        return;
                    }

                    UnFocus();
                }

                MeshRenderer meshRenderer = hit.transform.GetComponent<MeshRenderer>();
                meshRenderer.material = highlightMaterial;

                lastSelX = int.Parse(hit.transform.name) % 10;
                lastSelY = int.Parse(hit.transform.name) / 10;
                lastFieldName = GetWholeFieldName(hit.transform.name);
                lastField = hit.transform;
                selected = true;

                selectedFigure = occupArray[lastSelY,lastSelX];
                if ( selectedFigure / 100 == 0 ) return;
                selectedFigureName = GetFigTeam() + GetFigType() + GetFigIteration();

                possibleNextFields = FindPossibleNextFields();
                possibleNextFields.ForEach(possibleFieldName => {
                    MeshRenderer mr = GameObject.Find(possibleFieldName).transform.GetComponent<MeshRenderer>();
                    mr.material = highlightMaterial2;
                });
            } else if (lastField) UnFocus();
        }
    }

    void UnFocus()
    {
        MeshRenderer oldMeshRenderer = lastField.GetComponent<MeshRenderer>();
        oldMeshRenderer.material = defaultMaterial;
        selected = false;
        possibleNextFields.ForEach(possibleFieldName => {
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
        possibleNextFields = new List<string>();
    }

    bool IsFieldFree(int y, int x)
    {
        if (y>7 || x>7 || y<0 || x<0) return false;
        return occupArray[y,x] == 0;
    }

    Vector3 CalculateMoveVector()
    {
        return new Vector3(newSelX - lastSelX, 0, newSelY - lastSelY);
    }

    string GetFigType()
    {

        switch (selectedFigure / 100)
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

    string GetFigTeam()
    {
        if (selectedFigure.ToString().EndsWith("1")) return "C"; else return "";
    }

    string GetFigIteration()
    {
        int iter = (selectedFigure % 100) / 10;
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






    List<string> FindPossibleNextFields()
    {
        List<string> nextFields = new List<string>();
        int i = 1;
        int j;
        if ( GetFigTeam() == "C" ) i = -1;

        switch (selectedFigure / 100)
        {
            // pesiak
            case 1:
                CalculateField(lastSelY+1*i, lastSelX);
                if (mod(i,7) == lastSelY && IsFieldFree(lastSelY+1*i, lastSelX)) {
                    CalculateField(lastSelY+2*i, lastSelX);
                }
                break;
            // veza
            case 2:
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY+j, lastSelX))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY+j, lastSelX));
                    } else break;
                    j++;
                }
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY, lastSelX+j))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY, lastSelX+j));
                    } else break;
                    j++;
                }
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY-j, lastSelX))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY-j, lastSelX));
                    } else break;
                    j++;
                }
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY, lastSelX-j))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY, lastSelX-j));
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
                    } else break;
                    j++;
                }
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY-j, lastSelX+j))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY-j, lastSelX+j));
                    } else break;
                    j++;
                }
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY-j, lastSelX-j))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY-j, lastSelX-j));
                    } else break;
                    j++;
                }
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY+j, lastSelX-j))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY+j, lastSelX-j));
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
                    } else break;
                    j++;
                }
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY, lastSelX+j))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY, lastSelX+j));
                    } else break;
                    j++;
                }
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY-j, lastSelX))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY-j, lastSelX));
                    } else break;
                    j++;
                }
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY, lastSelX-j))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY, lastSelX-j));
                    } else break;
                    j++;
                }
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY+j, lastSelX+j))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY+j, lastSelX+j));
                    } else break;
                    j++;
                }
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY-j, lastSelX+j))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY-j, lastSelX+j));
                    } else break;
                    j++;
                }
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY-j, lastSelX-j))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY-j, lastSelX-j));
                    } else break;
                    j++;
                }
                j = 1;
                while (j < 8)
                {
                    if (IsFieldFree(lastSelY+j, lastSelX-j))
                    {
                        nextFields.Add(GetWholeFieldNameFromXY(lastSelY+j, lastSelX-j));
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
        return nextFields;

        void CalculateField(int y, int x)
        {
            if (IsFieldFree(y, x)) nextFields.Add(GetWholeFieldNameFromXY(y, x));
        }
    }


}
