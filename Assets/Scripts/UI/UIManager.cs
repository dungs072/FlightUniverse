using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance{get;private set;}
    [Header("Play game panel")]
    [SerializeField] private GameObject playGamePanel;
    [SerializeField] private CrossHair crossHair;
    //boss
    [SerializeField] private GameObject bossHealthBarParent;
    [SerializeField] private RectTransform bossHealthBar;
    [SerializeField] private TMP_Text healthBarTitle;
    //boss
    [SerializeField] private RectTransform playerHealthBar;
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
    #region GamePlay
    public void TogglePlayGamePanel(bool state)
    {
        playGamePanel.SetActive(state);
    }
    public void SetBossHealthBarValue(int currentHealth, int maxHealth)
    {
        float percentage = (float)currentHealth/(float)maxHealth;
        bossHealthBar.localScale = new Vector3(percentage,1f,1f);
    }
    public void ToggleBossHealthBar(bool state)
    {
        bossHealthBarParent.SetActive(state);
    }
    public void SetHealthBarTitle(string text)
    {
        healthBarTitle.text = text;
    }
    public void SetPlayerHealthBarValue(int currentHealth, int maxHealth)
    {
        float percentage = (float)currentHealth/(float)maxHealth;
        playerHealthBar.localScale = new Vector3(1f,percentage,1f);
    }
    #endregion
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
