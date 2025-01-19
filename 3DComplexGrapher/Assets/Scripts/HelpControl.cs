using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpControl : MonoBehaviour
{
    [SerializeField] private List<GameObject> contents;
    [SerializeField] private List<Pair<List<Button>, GameObject>> dict;
    [SerializeField] private List<Pair<Button, string>> functionButtons;

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
            if(y>680)
            {
                o.transform.localPosition = o.transform.localPosition - new Vector3(0, y - 640, 0);
            }
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