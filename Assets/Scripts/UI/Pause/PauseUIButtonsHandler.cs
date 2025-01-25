using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUIButtonsHandler : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button backToMenuButton;

    [Header("Scenes")]
    [SerializeField] private string mainMenuScene;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        backToMenuButton.onClick.AddListener(LoadMainMenuScene);
    }

    private void LoadMainMenuScene() => ScenesManager.Instance.FadeLoadTargetScene(mainMenuScene);
}
