using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CauldronManager : MonoBehaviour
{
    public static CauldronManager Instance;
        
    public RectTransform calderoRectTransform;
        
    private List<string> selectedIngredients = new List<string>();

    //ver ingredientes en caldero
    public Transform calderoDisplayParent; //contenedor de iconos
    public GameObject ingredientIconPrefab; //Prefab iconos de ingredientes

    private void Awake()
    {
        Instance = this;
    }

    //Añadir un ingrediente a la lista de seleccionados y mostrarlo en el caldero
    public void AddIngredient(string ingredientName)
    {        
        selectedIngredients.Add(ingredientName);
        Debug.Log($"Ingrediente añadido: {ingredientName}");

        int index = OrderManager.Instance.ingredientNames.IndexOf(ingredientName);
        if (index != -1)
        {
            // Instanciar el prefab del icono del ingrediente
            GameObject ingredientIcon = Instantiate(ingredientIconPrefab, calderoDisplayParent);
            ingredientIcon.GetComponent<Image>().sprite = OrderManager.Instance.ingredientSprites[index];
        }
    }

    public void ResetCauldron()
    {
        selectedIngredients.Clear();
        Debug.Log("Caldero reiniciado");

        foreach (Transform child in calderoDisplayParent)
        {
            Destroy(child.gameObject);
        }
    }

    //verifica que los ingredientes seleccionados coinciden con la orden
    public bool CheckOrder(List<string> requiredIngredients)
    {
        if (selectedIngredients.Count != requiredIngredients.Count)
        {
            Debug.Log("cantidad de ingredientes no coincide");
            return false;
        }

        foreach (var ingredient in requiredIngredients)
        {
            if (!selectedIngredients.Contains(ingredient))
            {
                Debug.Log($"Falta el ingrediente: {ingredient}");
                return false;
            }
        }

        Debug.Log("orden correcta!");
        return true;
    }
}