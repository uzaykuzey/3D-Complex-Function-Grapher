using Antlr4.Runtime;
using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Plotting : MonoBehaviour
{
    [SerializeField] private Button plottingButton;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject originalCube;
    [SerializeField] private int sqrtCubeCount;
    [SerializeField] private double adjustmentAmount;
    [SerializeField] private GameObject invalidExpression;

    public static Plotting Instance { get; private set; }

    public static int firstInputLength;

    
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
            CancelInvoke(nameof(CloseError));
            if (value)
            {
                Invoke(nameof(CloseError), 2);
            }
        }
    }

    public void CloseError()
    {
        ErrorScreen=false;
    }

    private Cube[] cubes;
    public ComplexFunction function;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        ErrorScreen = false;
        inputField.textComponent.richText = false;
        inputField.onValueChanged.AddListener((text) =>
        {
            text = Regex.Replace(
                    text.ToLower(),
                    @"(\s+|\$w|pow|w|sign|sin|si|im|re)",
                    match =>
                    {
                        switch (match.Value)
                        {
                            case "$w":
                                return "$w";
                            case "pow":
                                return "pow";
                            case "w":
                                return "W";
                            case "sign":
                                return "sign";
                            case "sin":
                                return "sin";
                            case "si":
                                return "Si";
                            case "im":
                                return "Im";
                            case "re":
                                return "Re";
                            default:
                                return " ";
                        }
                    });


            inputField.text = text;
        });

        plottingButton.onClick.AddListener(() =>
        {
            Plot(inputField.text);
        });

        cubes = new Cube[sqrtCubeCount*sqrtCubeCount];

        foreach(GameObject o in GameObject.FindGameObjectsWithTag("Axis"))
        {
            if(o.TryGetComponent(out MeshRenderer renderer))
            {
                renderer.material.color = Color.black;
            }
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
        if(UnityEngine.Input.GetKeyDown(KeyCode.Return)) 
        {
            if(ErrorScreen)
            {
                ErrorScreen = false;
            }
            else
            {
                Plot(inputField.text);
            }
        }
    }

    public void Plot(string input)
    {

        input = Regex.Replace(input.ToLower().Trim(), @"\s+", " ");
        firstInputLength=-1;
        inputField.text = input;
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

            if (firstInputLength != input.Replace(" ", "").Length)
            {
                ErrorScreen = true;
                return;
            }

            PlotAll();
        }
        catch
        {
            ErrorScreen = true;
            return;
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
