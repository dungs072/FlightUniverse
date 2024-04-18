using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance{get;private set;}
    [SerializeField] private CrossHair crossHair;
    public CrossHair CrossHair { get{return crossHair;} }
    private void Awake() {
        if(Instance==null)
        {
            Instance = this;
        }
        else
        {
            DontDestroyOnLoad(Instance);
        }
    }
}
