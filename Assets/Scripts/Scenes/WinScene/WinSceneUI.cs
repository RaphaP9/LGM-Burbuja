using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinSceneUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button menuButton;

    [Header("Scenes")]
    [SerializeField] private string menuScene;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        menuButton.onClick.AddListener(LoadMenuScene);
    }

    private void LoadMenuScene() => ScenesManager.Instance.FadeLoadTargetScene(menuScene);
}
