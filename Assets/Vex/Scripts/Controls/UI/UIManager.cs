using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public ActionsBar ActionsBar;
    public Camera GameCamera;

    private void Awake()
    {
        Instance = this;
    }
}
