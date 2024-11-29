using ComplexParser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;


public class Cube : MonoBehaviour
{
    public ComplexNumber Coordinates { get; set; }
    public float SideLength 
    { 
        get 
        { 
            return sideLength; 
        } 
        set 
        {
            sideLength = value;
            halfSideLength = sideLength / 2f;
            SetMesh();
        } 
    }
    public ComplexNumber AdjustedCoordinates 
    { 
        get
        {
            return Plotting.Instance.Adjust(Coordinates);
        } 
    }

    public float HalfSideLength
    {
        get
        {
            return halfSideLength;
        }
    }


    private float sideLength;
    private float halfSideLength;
    private MeshRenderer meshRenderer;

    private void SetMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "8-Vertex Cube";

        Vector3[] vertices = new Vector3[]
        {
            // Bottom vertices
            new Vector3(-HalfSideLength, -HalfSideLength, -HalfSideLength), // 0
            new Vector3( HalfSideLength, -HalfSideLength, -HalfSideLength), // 1
            new Vector3( HalfSideLength, -HalfSideLength,  HalfSideLength), // 2
            new Vector3(-HalfSideLength, -HalfSideLength,  HalfSideLength), // 3

            // Top vertices
            new Vector3(-HalfSideLength,  HalfSideLength, -HalfSideLength), // 4
            new Vector3( HalfSideLength,  HalfSideLength, -HalfSideLength), // 5
            new Vector3( HalfSideLength,  HalfSideLength,  HalfSideLength), // 6
            new Vector3(-HalfSideLength,  HalfSideLength,  HalfSideLength)  // 7
        };

        // Define the triangles
        int[] triangles = new int[]
        {
            // Bottom face (clockwise winding order)
            0, 1, 2,
            0, 2, 3,

            // Top face (clockwise winding order)
            4, 6, 5,
            4, 7, 6,

            // Front face (clockwise winding order)
            0, 5, 1,
            0, 4, 5,

            // Back face (clockwise winding order)
            2, 6, 7,
            2, 7, 3,

            // Left face (clockwise winding order)
            0, 7, 4,
            0, 3, 7,

            // Right face (clockwise winding order)
            1, 6, 2,
            1, 5, 6
        };


        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer=GetComponent<MeshRenderer>();
    }


    public void Plot(ComplexFunction function)
    {
        if(meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }
        meshRenderer.enabled = false;
        if (!function.Defined(AdjustedCoordinates))
        {
            return;
        }

        if(Coordinates==0)
        {
            int k = 0;
        }

        ComplexNumber value = function.Calculate(AdjustedCoordinates);
        if(value.Abs()>200)
        {
            return;
        }
        transform.position = new((float) Coordinates.real, (float) value.Abs() - 10, (float) Coordinates.imaginary);

        Vector3 corner1 = new((float)Coordinates.real - HalfSideLength, Mathf.Min((float)function.CalculateApproximately(Plotting.Instance.Adjust(Coordinates - HalfSideLength - halfSideLength * ComplexNumber.I)).Abs(), 200), (float)Coordinates.imaginary - HalfSideLength);
        Vector3 corner2 = new((float)Coordinates.real + HalfSideLength, Mathf.Min((float)function.CalculateApproximately(Plotting.Instance.Adjust(Coordinates + HalfSideLength - halfSideLength * ComplexNumber.I)).Abs(), 200), (float)Coordinates.imaginary - HalfSideLength);
        Vector3 corner3 = new((float)Coordinates.real + HalfSideLength, Mathf.Min((float)function.CalculateApproximately(Plotting.Instance.Adjust(Coordinates + HalfSideLength + halfSideLength * ComplexNumber.I)).Abs(), 200), (float)Coordinates.imaginary + HalfSideLength);
        Vector3 corner4 = new((float)Coordinates.real - HalfSideLength, Mathf.Min((float)function.CalculateApproximately(Plotting.Instance.Adjust(Coordinates - HalfSideLength + halfSideLength * ComplexNumber.I)).Abs(), 200), (float)Coordinates.imaginary + HalfSideLength);

        corner1.y -= 10;
        corner2.y -= 10;
        corner3.y -= 10;
        corner4.y -= 10;

        Vector3[] vertices = new Vector3[]
        {
            // Bottom vertices
            corner1 - transform.position,
            corner2 - transform.position,
            corner3 - transform.position,
            corner4 - transform.position,

            // Top vertices
            corner1 - transform.position + new Vector3(0, 0.1f, 0),
            corner2 - transform.position + new Vector3(0, 0.1f, 0),
            corner3 - transform.position + new Vector3(0, 0.1f, 0),
            corner4 - transform.position + new Vector3(0, 0.1f, 0)
        };

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshRenderer.enabled = true;

        double argz = value.Arg() / Math.PI;

        double hue = argz >= 0 ? argz / 2 : argz / 2 + 1;

        meshRenderer.material.color = /*value==0 ? new Color(0.5f, 0.5f, 0.5f) : */Color.HSVToRGB((float) hue, 1, 1);


    }
}
