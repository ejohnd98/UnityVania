using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayerProxy : MonoBehaviour
{
    public void PlaySound(string sndName){
        SoundSystem.instance.PlaySound(sndName);
    }
}
