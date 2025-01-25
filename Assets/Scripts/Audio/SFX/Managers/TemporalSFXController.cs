using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TemporalSFXController : MonoBehaviour
{
    private bool pausable;
    public bool Pausable => pausable;

    public void SetPausable(bool pausable) => this.pausable = pausable;
}
