using Fishing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    [SerializeField] private Button slash;
    [SerializeField] private Text fishCount;
    private bool iso = true;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        slash.onClick.AddListener(OnSlashClick);
    }
    private void OnSlashClick()
    {

        if (iso)
        {
            GameManager.Instance.SwitchGameState(GameState.SlashFish);
            iso = false;
        }
        else
        {
            GameManager.Instance.SwitchGameState(GameState.Fishing);
            iso = true;
        }
    }
}
