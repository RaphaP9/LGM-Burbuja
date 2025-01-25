using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button playGameButton;
    [SerializeField] private Button quitButton;

    [Header("Scenes")]
    [SerializeField] private string gameplayScene;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        playGameButton.onClick.AddListener(LoadGameplayScene);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void LoadGameplayScene() => ScenesManager.Instance.FadeLoadTargetScene(gameplayScene);
    private void QuitGame() => Application.Quit();  
}
