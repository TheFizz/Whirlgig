using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableGenerator : MonoBehaviour
{
    int segments = 8;
    int angleFraction = 20;

    public GameObject whirlgigArrow;
    public GameObject pinpoint;

    public GameObject segmentPrefab;
    public GameObject letterPrefab;
    public GameObject nextPrefab;
    public GameObject thirteenPrefab;

    public Material segmentMatPrimary;
    public Material segmentMatSecondary;
    public Material segmentMat13;

    public Texture2D dummy;

    List<Vector3> points = new List<Vector3>();

    void Start()
    {
        segmentMatPrimary.color = Static_Data.primaryColor;
        segmentMatSecondary.color = Static_Data.secondaryColor;
        segmentMat13.color = Static_Data.thirteenColor;

        segments = Static_Data.segments;
        GenerateTable();
        GenerateBorders();
        GenerateLetters();
        whirlgigArrow.transform.eulerAngles = new Vector3(0, ((360f / segments) + (360f / segments) / 2), 0);

    }
    void GenerateTable()
    {
        float segmentAngle;
        segmentAngle = 360f / segments;

        float partialSegmentAngle = segmentAngle / angleFraction;
        float currentRotation = segmentAngle;

        for (int i = 0; i < segments * angleFraction; i++)
        {
            whirlgigArrow.transform.eulerAngles = new Vector3(0, currentRotation, 0);
            points.Add(pinpoint.transform.position);
            currentRotation += partialSegmentAngle;
        }

        GameObject segmentHolder = new GameObject();
        segmentHolder.transform.position = Vector3.zero;
        segmentHolder.name = "SegmentHolder";
        bool activeMatPrim = true;

        for (int i = 0; i < segments; i++)
        {
            Mesh mesh = new Mesh();
            Vector3[] vertices = new Vector3[angleFraction + 2];
            vertices[0] = Vector3.zero;

            for (int j = 0; j < angleFraction + 1; j++)
            {
                int curPoint = i * angleFraction + j;
                if (curPoint <= points.Count - 1)
                    vertices[j + 1] = points[curPoint];
                else
                    vertices[j + 1] = points[0];
            }

            mesh.vertices = vertices;
            int[] tris = new int[angleFraction * 3];

            for (int k = 0; k < angleFraction; k++)
            {
                tris[k * 3] = 0;
                tris[k * 3 + 1] = k + 1;
                tris[k * 3 + 2] = k + 2;
            }

            mesh.triangles = tris;

            mesh.RecalculateNormals();

            var tmp = Instantiate(segmentPrefab);
            tmp.GetComponent<MeshFilter>().mesh = mesh;
            tmp.GetComponent<MeshCollider>().sharedMesh = mesh;
            tmp.name = "Segment " + i;
            tmp.transform.SetParent(segmentHolder.transform);
            tmp.isStatic = true;
            if (i == 12)
            {
                tmp.GetComponent<MeshRenderer>().material = segmentMat13;
                if (segments % 2 == 0)
                {
                    activeMatPrim = !activeMatPrim;
                }

                continue;
            }
            if (activeMatPrim)
            {
                tmp.GetComponent<MeshRenderer>().material = segmentMatPrimary;
                activeMatPrim = !activeMatPrim;
                continue;
            }
            if (!activeMatPrim)
            {
                tmp.GetComponent<MeshRenderer>().material = segmentMatSecondary;
                activeMatPrim = !activeMatPrim;
                continue;
            }

        }
    }
    void GenerateBorders()
    {
        GameObject borderHolder = new GameObject();
        borderHolder.transform.position = Vector3.zero;
        borderHolder.name = "BorderHolder";

        for (int i = 0; i < segments; i++)
        {
            Vector3 midpoint = (Vector3.zero + points[i * angleFraction]) * 0.5f;
            GameObject border = GameObject.CreatePrimitive(PrimitiveType.Cube);

            border.transform.localScale = new Vector3(0.2f, 0.01f, Vector3.Distance(Vector3.zero, pinpoint.transform.position));
            border.transform.position = midpoint;
            border.transform.LookAt(Vector3.zero);
            border.transform.GetComponent<MeshRenderer>().material.color = new Color32(132,118,84,255);
            border.transform.SetParent(borderHolder.transform);
            border.isStatic = true;
            GameObject.Destroy(border.transform.GetComponent<BoxCollider>());
        }
    }
    void GenerateLetters()
    {

        Static_Data.letters = new GameObject[segments];
        Static_Data.nexts = new GameObject[segments];

        GameObject letterHolder = new GameObject();
        letterHolder.transform.position = Vector3.zero;
        letterHolder.name = "LetterHolder";

        GameObject nextHolder = new GameObject();
        nextHolder.transform.position = Vector3.zero;
        nextHolder.name = "NextHolder";

        int anchorOffset = (angleFraction / 2);
        int curPlacingPoint = anchorOffset;

        if (Static_Data.customDummy != null)
        {
            dummy = Static_Data.customDummy;
        }

        for (int i = 0; i < segments; i++)
        {
            GameObject letter;
            if (i == 12)
                letter = Instantiate(thirteenPrefab);
            else
            {
                letter = Instantiate(letterPrefab);
                letter.GetComponentInChildren<TextMesh>().text = Static_Data.segmentOptions[i];

                if (Static_Data.whiteFont)
                {
                    letter.GetComponentInChildren<TextMesh>().color = Color.white;
                }

                try
                {
                    if (Static_Data.pics[i] != null) { letter.transform.Find("PicContainer").transform.GetComponent<Renderer>().material = Static_Data.pics[i]; }
                    else
                    {
                        Material mat = new Material(Shader.Find("Unlit/Texture"));
                        mat.mainTexture = dummy;
                        letter.transform.Find("PicContainer").transform.GetComponent<Renderer>().material = mat;
                    } //what.
                }

                catch { }

                if(!Static_Data.filesLoaded)
                {
                    letter.transform.Find("PicContainer").gameObject.SetActive(false);
                }

                if(Static_Data.customLetter!=null)
                {
                    letter.transform.Find("Plane").transform.GetComponent<Renderer>().material.mainTexture = Static_Data.customLetter;
                }
            }

            Vector3 placePoint = points[anchorOffset + (i * angleFraction)];

            placePoint.y = +0.02f;
            letter.transform.position = placePoint;
            letter.transform.position = Vector3.MoveTowards(letter.transform.position, Vector3.zero, 2f);
            letter.transform.LookAt(Vector3.zero);
            letter.transform.SetParent(letterHolder.transform);
            Static_Data.letters[i] = letter;

            GameObject next = Instantiate(nextPrefab);

            next.transform.position = letter.transform.position + new Vector3(0, 0.01f, 0);
            next.transform.LookAt(Vector3.zero);
            next.transform.SetParent(nextHolder.transform);
            next.SetActive(false);
            Static_Data.nexts[i] = next;
        }
    }
}
