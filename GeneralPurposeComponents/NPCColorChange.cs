using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCColorChange : MonoBehaviour
{
    // Public attributes.

    // Mesh and color:
    [SerializeField, Tooltip("Color to chage")] Color nPCCurrentColor;

    // Dialog attributes:
    // [SerializeField,Tooltip("Drag the dialogue panel.")] GameObject DialoguePanel;
    // [SerializeField,Tooltip("Darg here the dialogue UI Text mesh pro.")] TextMeshProUGUI Dialogue;
    // private bool Talking =false;
    // private const string CTalk = "Fire1";
    // private const string CText = " Gran Fares, finalmente has despertado, busca a Shanti, la Diosa Única.";


    // Start is called before the first frame update
    void Start()
    {
        // Inicialize attributes and components.
        Initialize();
    }

    // Se llama a Update cada fotograma siempre que la clase MonoBehaviour esté habilitada
    private void Update()
    {
        // if (Talking)
        // {
        //     if (Input.GetButtonDown(CTalk)) DialoguePanel.SetActive(!DialoguePanel.activeSelf);
        // }
    }

    // Se llama a OnTriggerEnter cuando el colisionador other escribe en el disparador
    private void OnTriggerEnter(Collider other)
    {
        ChangeColor(Color.red);
        // Talking = true;

    }

    // Se llama a OnTriggerExit cuando el colisionador other deja de tocar el disparador
    private void OnTriggerExit(Collider other)
    {
        ChangeColor(nPCCurrentColor);
        // hDialoguePanel.SetActive(false);
        // Talking = false;
    }

    // Initialize attributes and components.
    private void Initialize()
    {
        nPCCurrentColor = GetComponent<MeshRenderer>().material.color;
        // DialoguePanel.SetActive(false);
        // Dialogue.text = CText;
    }

    // Change the NPC color.
    private void ChangeColor(Color change)
    {
        GetComponent<MeshRenderer>().material.color = change;
    }
}
