using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public enum HelpScreens
{
    General,
    Acknowledgements,
    ListOfFunctions,
    SavedFunctions
}

public class HelpControl : MonoBehaviour
{
    [SerializeField] private List<Pair<List<Button>, GameObject>> dict;

    void Start()
    {
        ViewScreen(null);
        foreach (var p in dict)
        {
            foreach (var button in p.Key)
            {
                var screen = p.Value;
                button.onClick.AddListener(() => ViewScreen(screen));
            }
        }
    }

    public void ViewScreen(GameObject screen)
    {
        foreach (var pair in dict)
        {
            if(pair.Value!=null)
            {
                pair.Value.SetActive(false);
            }
        }

        if(screen!=null)
        {
            screen.SetActive(true);
        }
    }
}

[System.Serializable]
public struct Pair<T, S>
{
    public T Key;
    public S Value;
}