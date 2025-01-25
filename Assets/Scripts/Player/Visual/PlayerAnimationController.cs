using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerJump.OnPlayerJump += PlayerJump_OnPlayerJump;
        PlayerFall.OnPlayerFall += PlayerFall_OnPlayerFall;
        PlayerLand.OnPlayerLand += PlayerLand_OnPlayerLand;
    }

    private void OnDisable()
    {
        PlayerJump.OnPlayerJump -= PlayerJump_OnPlayerJump;
        PlayerFall.OnPlayerFall -= PlayerFall_OnPlayerFall;
        PlayerLand.OnPlayerLand -= PlayerLand_OnPlayerLand;
    }
    private void PlayerJump_OnPlayerJump(object sender, System.EventArgs e)
    {
        
    }
    private void PlayerFall_OnPlayerFall(object sender, System.EventArgs e)
    {
        
    }

    private void PlayerLand_OnPlayerLand(object sender, PlayerLand.OnPlayerLandEventArgs e)
    {
        
    }
}
