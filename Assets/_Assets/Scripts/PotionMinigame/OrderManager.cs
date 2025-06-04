using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    // Lista de todos los ingredientes disponibles (asignados en el inspector)
    public List<Sprite> ingredientSprites;
    public List<string> ingredientNames;

    // Espacio en la UI para mostrar la orden actual
    public Transform orderDisplayParent;
    public GameObject orderIconPrefab; // Prefab para mostrar un ingrediente en la orden

    // Orden actual
    private List<string> currentOrder = new List<string>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GenerateNewOrder();
    }

    // Generar una nueva orden aleatoria
    public void GenerateNewOrder()
    {
        currentOrder.Clear();
        foreach (Transform child in orderDisplayParent)
        {
            Destroy(child.gameObject);
        }

        int numberOfIngredients = Random.Range(2, 4); 
        for (int i = 0; i < numberOfIngredients; i++)
        {
            int randomIndex = Random.Range(0, ingredientNames.Count);
            currentOrder.Add(ingredientNames[randomIndex]);
        }

        // Mostrar la orden en la UI
        DisplayOrder();
    }

    private void DisplayOrder()
    {
        foreach (var ingredient in currentOrder)
        {
            // Buscar el sprite correspondiente al ingrediente
            int index = ingredientNames.IndexOf(ingredient);
            if (index != -1)
            {
                GameObject orderIcon = Instantiate(orderIconPrefab, orderDisplayParent);
                orderIcon.GetComponent<Image>().sprite = ingredientSprites[index];
            }
        }
    }

    // Verificar si la orden actual se ha completado
    public void CheckOrder()
    {
        if (CauldronManager.Instance.CheckOrder(currentOrder))
        {
            Debug.Log("¡Orden completada!");
            CauldronManager.Instance.ResetCauldron();
            
            GenerateNewOrder();
        }
        else
        {
            Debug.Log("Orden incorrecta...");
        }
       
        
    }

    public void OnConfirmButtonPressed()
    {
        CheckOrder(); 
    }

    public void OnResetButtonPressed()
    {
        CauldronManager.Instance.ResetCauldron(); // Solo reiniciar el caldero
        Debug.Log("Caldero reiniciado, pero la orden sigue activa.");
    }
}