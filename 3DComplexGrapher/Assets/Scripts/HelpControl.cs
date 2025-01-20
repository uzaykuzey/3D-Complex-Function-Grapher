using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpControl : MonoBehaviour
{
    [SerializeField] private List<GameObject> contents;
    [SerializeField] private List<Pair<List<Button>, GameObject>> dict;
    [SerializeField] private List<Pair<Button, string>> functionButtons;
    [SerializeField] private Button samuel;
    [SerializeField] private Button linkedIn;
    [SerializeField] private Button github;

    private Plotting plotting;

    void Start()
    {
        plotting = GetComponent<Plotting>();
        ViewScreen(null);
        foreach (var p in dict)
        {
            foreach (var button in p.First)
            {
                var screen = p.Second;
                button.onClick.AddListener(() => ViewScreen(screen));
            }
        }

        foreach(var p in functionButtons)
        {
            p.First.onClick.AddListener(() =>
            {
                plotting.Plot(p.Second);
            });
        }

        samuel.onClick.AddListener(() =>
        {
            Application.OpenURL("https://samuelj.li/complex-function-plotter/#z");
        });

        linkedIn.onClick.AddListener(() =>
        {
            Application.OpenURL("https://www.linkedin.com/in/ata-uzay-kuzey/");
        });

        github.onClick.AddListener(() =>
        {
            Application.OpenURL("https://github.com/uzaykuzey/3D-Complex-Function-Grapher");
        });
    }

    public void ViewScreen(GameObject screen)
    {
        foreach (var pair in dict)
        {
            if(pair.Second!=null)
            {
                pair.Second.SetActive(false);
            }
        }

        if(screen!=null)
        {
            screen.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        foreach(var o in contents)
        {
            float y = o.transform.localPosition.y;
            if (y < -1)
            {
                o.transform.localPosition = o.transform.localPosition - new Vector3(0, y, 0);
            }
            int c = o.name.Contains("1") ? 680 : 2000;
            if(y> c)
            {
                o.transform.localPosition = o.transform.localPosition - new Vector3(0, y - c + 40, 0);
            }
        }
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ViewScreen(null);
        }
    }

    public bool HasOpenScreen()
    {
        foreach(var pair in dict)
        {
            if(pair.Second!=null && pair.Second.activeSelf)
            {
                return true;
            }
        }
        return false;
    }
}

[System.Serializable]
public struct Pair<T, S>
{
    public T First;
    public S Second;
}