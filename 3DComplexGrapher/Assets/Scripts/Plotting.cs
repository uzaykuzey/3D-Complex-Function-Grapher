using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Plotting : MonoBehaviour
{
    [SerializeField] private Button plottingButton;
    [SerializeField] private Button errorScreenButton;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject originalCube;
    [SerializeField] private int sqrtCubeCount;
    [SerializeField] private double adjustmentAmount;
    [SerializeField] private GameObject invalidExpression;

    public static Plotting Instance { get; private set; }
    public bool ErrorScreen
    {
        get
        {
            return invalidExpression.GetComponent<RawImage>().enabled;
        }

        set
        {
            invalidExpression.GetComponent<RawImage>().enabled=value;
            foreach(TextMeshProUGUI o in invalidExpression.GetComponentsInChildren<TextMeshProUGUI>())
            {
                o.enabled = value;
            }
            foreach (Image o in invalidExpression.GetComponentsInChildren<Image>())
            {
                o.enabled = value;
            }
        }
    }

    private Cube[] cubes;
    private ComplexFunction function;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        ErrorScreen = false;
        plottingButton.onClick.AddListener(() =>
        {
            ParseFunction(inputField.text);
        });

        errorScreenButton.onClick.AddListener(() =>
        {
            print("hello");
            ErrorScreen = false;
        });

        cubes = new Cube[sqrtCubeCount*sqrtCubeCount];

        foreach(GameObject o in GameObject.FindGameObjectsWithTag("Axis"))
        {
            o.GetComponent<MeshRenderer>().material.color = Color.black;
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
            if(ErrorScreen)
            {
                ErrorScreen = false;
            }
            else
            {
                ParseFunction(inputField.text);
            }
        }
    }

    void ParseFunction(string input)
    {
        input = input.ToLower();
        var lexer = new MathLexer(new AntlrInputStream(input));
        var tokens = new CommonTokenStream(lexer);
        var parser = new MathParser(tokens);

        parser.RemoveErrorListeners();
        parser.AddErrorListener(new ThrowingErrorListener());

        IterativeVariable.iterativeValues.Clear();

        try
        {
            var tree = parser.add_expr();
            FunctionBuilderVisitor visitor = new FunctionBuilderVisitor();
            function = 5 * visitor.Visit(tree);

            PlotAll();
        }
        catch
        {
            ErrorScreen = true;
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
