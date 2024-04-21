using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance{get;private set;}
    [Header("Play game panel")]
    [SerializeField] private GameObject playGamePanel;
    [SerializeField] private CrossHair crossHair;
    [Header("Selection SpaceShip panel")]
    [SerializeField] private GameObject selectionSpaceShipPanel;
    [SerializeField] private SpaceShipSelectionUI spaceShipSelectionUI;
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
    public void TogglePlayGamePanel(bool state)
    {
        playGamePanel.SetActive(state);
    }
    public void ToggleSelectionSpaceShipPanel(bool state)
    {
        selectionSpaceShipPanel.SetActive(state);
    }
    public void OnFightClick()
    {
        ToggleSelectionSpaceShipPanel(false);
        TogglePlayGamePanel(true);
        GameObject player = ReferenceManager.Instance.Player;
        if(player.TryGetComponent(out PlayerInitialization playerInitialization))
        {
            playerInitialization.StartGame();
        }
        if(player.TryGetComponent(out PlayerData playerData))
        {
            var data = spaceShipSelectionUI.CurrentSelectSpaceShipData;
            if(data!=null)
            {
                playerData.SetUpSpaceShipData(data);
            }
            
        }
    }
}
