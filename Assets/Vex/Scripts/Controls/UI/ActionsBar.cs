using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[Serializable]
public class DynamicButtonEvent : UnityEvent { }

[Serializable]
public class DynamicButton
{
    public string label;
    public DynamicButtonEvent onPressed;
}

/// <summary>
/// The UI bar along the bottom of the screen
/// </summary>
public class ActionsBar : MonoBehaviour
{
    [SerializeField] Button buttonPrefab;

    private List<Button> currentButtons = new List<Button>();

    public void Refresh(List<DynamicButton> buttonInfo)
    {
        Clear();

        currentButtons = buttonInfo.Select(x => 
        {
            var b = Instantiate(buttonPrefab);

            b.GetComponentInChildren<Text>().text = x.label;
            b.onClick.AddListener(() => { x.onPressed?.Invoke(); });

            b.transform.SetParent(transform);

            return b;
        }).ToList();
    }

    public void Clear()
    {
        currentButtons.DestroyAll();
    }
}
