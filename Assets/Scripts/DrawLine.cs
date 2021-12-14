using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject parachute;
    public GameObject InstantiatedCubes;

    private GameObject currentLine;

    public LineRenderer lineRenderer;

    public List<Vector2> drawPositions;

    public bool creatingObject;

    public GameObject drawing;

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.x < 5 && mousePos.x > -5 && mousePos.y > -3.5f && mousePos.y < -0.5f)
        {           
            if (Input.GetMouseButtonDown(0))
            {
                creatingObject = true;
                CreateLineFunction();
            }

            if (Input.GetMouseButton(0))
            {
                creatingObject = false;
                if (Vector2.Distance(mousePos, drawPositions[drawPositions.Count - 1]) > .1f)
                {
                    UpdateLineFunction(mousePos);                 
                }
                
            }
            if (Input.GetMouseButtonUp(0))
            {
                creatingObject = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                StartCoroutine(cubesCoroutine());
            }
        }

        else
        {
            drawing = GameObject.Find("Line(Clone)");
            if (drawing && !creatingObject)
            {
                Debug.Log("YOU ARE OUT OF THE PALETTE, DRAW AGAIN!");
                Destroy(drawing);
                drawPositions.Clear();
            }
        }
    }

    void CreateLineFunction()
    {
        currentLine = Instantiate(linePrefab, Vector3.zero , Quaternion.identity);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        drawPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        drawPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        lineRenderer.SetPosition(0, drawPositions[0]);
        lineRenderer.SetPosition(1, drawPositions[1]);
    }

    void UpdateLineFunction(Vector2 newDrawPos)
    {
        drawPositions.Add(newDrawPos);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1,newDrawPos);
    }

    void CreateCubes()
    {
        foreach (Transform child in InstantiatedCubes.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        
        for (int i = 0; i < drawPositions.Count; i++)
        {
            Vector3 cubesTransform= new Vector3(drawPositions[i].x, drawPositions[i].y + 5, 0);
            GameObject cubes = Instantiate(parachute, cubesTransform, Quaternion.identity);
            cubes.transform.parent = GameObject.Find("CubesInstantiate").transform;
        }
    }

    IEnumerator cubesCoroutine()
    {
        yield return new WaitForSeconds(1f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        CreateCubes();
        drawPositions.Clear();
        Destroy(currentLine);       
    }

}
