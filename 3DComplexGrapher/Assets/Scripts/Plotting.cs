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
    [SerializeField] private GameObject axisCanvas;
    [SerializeField] private Button centerButton;
    [SerializeField] private GameObject centerCanvas;
    [SerializeField] private TMP_InputField centerField;

    public static Plotting Instance { get; private set; }

    public static int firstInputLength;

    public static ComplexNumber center;

    
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
            inputField.text = InputConversion(text);
        });

        centerField.onValueChanged.AddListener((text) => 
        { 
            centerField.text = InputConversion(text); 
        });

        centerField.text = "0";
        centerCanvas.SetActive(false);

        centerButton.onClick.AddListener(() =>
        {
            centerCanvas.SetActive(!centerCanvas.activeSelf);
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

    private string InputConversion(string text)
    {
        text = Regex.Replace(
                    text.ToLower(),
                    @"(\s+|\$w|\$s|\$i|\$r|pow|w|sign|sin|si|im|re)",
                    match =>
                    {
                        switch (match.Value)
                        {
                            case "$w":
                                return "$w";
                            case "$s":
                                return "$s";
                            case "$i":
                                return "$i";
                            case "$r":
                                return "$r";
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
        return text;
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
                Plot(inputField.text);
            }
        }
    }

    public void DetermineCenter(string input)
    {
        try
        {
            input = input == "" ? "0": input;
            ComplexFunction centerFunction = ParseInput(input);
            if(!centerFunction.IsComplexConstant() || !centerFunction.Defined(0))
            {
                throw new Exception("Center is not complex constant");
            }
            center = -centerFunction.Calculate(0);
            axisCanvas.SetActive(center.Abs()<=7);
            axisCanvas.transform.position = new Vector3((float) (adjustmentAmount * center.real), axisCanvas.transform.position.y, (float) (adjustmentAmount * center.imaginary) + 6.1922f);
        }
        catch(Exception e)
        {
            throw e;
        }
    }

    public ComplexFunction ParseInput(string input)
    {
        input = Regex.Replace(input.ToLower().Trim(), @"\s+", " ");
        firstInputLength = -1;
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
            ComplexFunction f = visitor.Visit(tree);

            if (firstInputLength != input.Replace(" ", "").Length)
            {
                ErrorScreen = true;
                throw new Exception("Invalid function");
            }

            return f;
        }
        catch
        {
            ErrorScreen = true;
            throw new Exception("Invalid function");
        }
    }

    public void Plot(string input)
    {
        try
        {
            inputField.text = input;
            DetermineCenter(centerField.text);
            function = 5 * ParseInput(input);
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
