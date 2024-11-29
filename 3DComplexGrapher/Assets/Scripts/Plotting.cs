using Antlr4.Runtime;
using ComplexParser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Plotting : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject originalCube;
    [SerializeField] private int sqrtCubeCount;
    [SerializeField] private double adjustmentAmount;
    [SerializeField] private GameObject topOfText;

    public static Plotting Instance { get; private set; }

    private Cube[] cubes;
    private ComplexFunction function;
    private List<GameObject> additionalCubes;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        additionalCubes = new();
        button.onClick.AddListener(() =>
        {
            ParseFunction(inputField.text);
        });

        cubes = new Cube[sqrtCubeCount*sqrtCubeCount];

        foreach(GameObject o in GameObject.FindGameObjectsWithTag("Axis"))
        {
            o.GetComponent<MeshRenderer>().material.color = UnityEngine.Color.black;
        }

        for(int i=-sqrtCubeCount/2; i < sqrtCubeCount/2; i++)
        {
            for(int j=-sqrtCubeCount/2;j<sqrtCubeCount/2;j++)
            {
                GameObject newCube=Instantiate(originalCube);
                newCube.transform.position = new Vector3(i, 0, j);
                newCube.GetComponent<Cube>().SideLength = 1;
                newCube.GetComponent<Cube>().Coordinates = new ComplexNumber(i, j);
                cubes[(i + sqrtCubeCount / 2) * sqrtCubeCount + (j + sqrtCubeCount / 2)] = newCube.GetComponent<Cube>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return)) 
        {
            ParseFunction(inputField.text);
        }
    }

    void ParseFunction(string input)
    {
        input = input.ToLower();
        var lexer = new MathLexer(new AntlrInputStream(input));
        var tokens = new CommonTokenStream(lexer);
        var parser = new MathParser(tokens);

        IterativeVariable.iterativeValues.Clear();
        foreach(GameObject o in additionalCubes)
        {
            Destroy(o);
        }
        additionalCubes.Clear();

        try
        {
            var tree = parser.add_expr();
            FunctionBuilderVisitor visitor = new FunctionBuilderVisitor();
            function = 5 * visitor.Visit(tree);

            PlotAll();
        }
        catch (Exception ex)
        {
            print("Parse error: " + ex.Message);
        }
    }

    void PlotAll()
    {
        foreach (Cube cube in cubes)
        {
            cube.Plot(function);
        }
    }

    public ComplexNumber Adjust(ComplexNumber number)
    {
        return number / adjustmentAmount;
    }
}
