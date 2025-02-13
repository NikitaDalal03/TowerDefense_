using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class TurretUiManager : MonoBehaviour
{
    public List<TurretData> allTurrets;
    public GameObject buttonPrefab;
    public Transform buttonContainer;
    public int turretsPerLevel = 3;

    private int currentLevel = 1;

    [SerializeField] TowerPlacement towerPlacement;

    void Start()
    {
        if (towerPlacement == null)
        {
            towerPlacement = GetComponent<TowerPlacement>();
        }

        // Ensure allTurrets is sorted and buttons are updated
        allTurrets.Sort(TurretData.CompareByCost);
        UpdateTurretButtons();
    }


    public void UpdateTurretButtons()
    {
        // Clear existing buttons
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        int numButtonsToCreate = turretsPerLevel * currentLevel;

        for (int i = 0; i < numButtonsToCreate && i < allTurrets.Count; i++)
        {
            TurretData turretData = allTurrets[i];
            CreateTurretButton(turretData);
        }
    }

    private void CreateTurretButton(TurretData turretData)
    {
        // Instantiate the button
        GameObject button = Instantiate(buttonPrefab, buttonContainer);

        // Ensure button prefab has the necessary components
        if (button == null)
        {
            Debug.LogError("Button prefab is missing.");
            return;
        }

        Image buttonImage = button.transform.Find("Icon")?.GetComponent<Image>();
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

        if (buttonImage == null || buttonText == null)
        {
            Debug.LogError("Button prefab is missing Image or TextMeshProUGUI components.");
            return;
        }

        buttonImage.sprite = turretData.turretIcon;
        buttonText.text = $"{turretData.cost}";

        // Adjust button size (as before)
        RectTransform buttonRect = button.GetComponent<RectTransform>();
        buttonRect.sizeDelta = new Vector2(0f, 60f); // Adjust width and height

        // Position the button
        float buttonSpacing = 250f;
        float containerWidth = buttonContainer.GetComponent<RectTransform>().rect.width;
        float rightmostPosition = containerWidth - buttonSpacing * (buttonContainer.childCount);
        button.transform.localPosition = new Vector3(rightmostPosition, 65f, 0);

        // Add listener for the button click
        Button btn = button.GetComponent<Button>();
        if (btn == null)
        {
            Debug.LogError("Button component is missing.");
            return;
        }

        btn.onClick.AddListener(() => OnTurretButtonClicked(turretData));
    }



    private void OnTurretButtonClicked(TurretData turretData)
    {
        if (turretData == null)
        {
            Debug.LogError("TurretData is null!");
            return;
        }

        Debug.Log($"Instantiating turret: {turretData.turretName}");
        towerPlacement.SetTowerToPlace(turretData.turretPrefab); 
    }


    // Call this when you want to increment the level
    public void NextLevel()
    {
        currentLevel++;
        UpdateTurretButtons();
    }
}
