using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamerName : MonoBehaviour
{
    [SerializeField, Tooltip("Input gamer name")] TextMeshProUGUI txtGamerName;
    private void OnDisable() { RenameGamer(); }

    void RenameGamer() { SceneData.instance.gamerName = txtGamerName.text; }
}
