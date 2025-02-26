using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FillPanelController : MonoBehaviour
{
    
    public GameObject buttonPrefab; 
    public Transform contentParent; 

  
    public List<string> buttonLabels = new List<string> { "Botón 1", "Botón 2", "Botón 3", "Botón 4" };

    void Start()
    {
        GenerateButtons();
    }

    public void GenerateButtons()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        foreach (string label in buttonLabels)
        {
            GameObject newButton = Instantiate(buttonPrefab, contentParent);
            newButton.GetComponentInChildren<Text>().text = label; 

            newButton.GetComponent<Button>().onClick.AddListener(() => RemoveButton(newButton));
        }
    }

    public void RemoveButton(GameObject button)
    {
        Destroy(button);
    }
}
