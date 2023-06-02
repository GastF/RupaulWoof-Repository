using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstObject : MonoBehaviour
{
    [SerializeField] private GameObject firstElement;
    [SerializeField] private Transform rayCastPlace;
    private LayerMask objectsInBeltLayer;
    public float detectionRange = 14f;
    private bool isDetectingObject = false;

    // Referencia al material para resaltar el objeto seleccionado
    [SerializeField] private Material highlightedMaterial;
    private Material originalMaterial; // Almacenar el material original del objeto

    private void Start()
    {
        objectsInBeltLayer = LayerMask.GetMask("ObjectsInBelt");
    }

    private void Update()
    {
        Vector2 rayDirection = Vector2.right;
        RaycastHit2D hit = Physics2D.Raycast(rayCastPlace.position, rayDirection, detectionRange, objectsInBeltLayer);

        Debug.DrawRay(rayCastPlace.position, rayDirection * detectionRange, Color.red);

        if (hit.collider != null)
        {
            GameObject detectedObject = hit.collider.gameObject;

            if (detectedObject != firstElement)
            {
                // Restaurar el material original del objeto anterior
                RestoreOriginalMaterial();

                firstElement = detectedObject;
                isDetectingObject = true;

                // Almacenar el material original del nuevo objeto
                Renderer objectRenderer = firstElement.GetComponent<Renderer>();
                originalMaterial = objectRenderer.material;

                // Resaltar el nuevo objeto seleccionado
                objectRenderer.material = highlightedMaterial;
            }
        }
        else
        {
            if (firstElement != null)
            {
                // Restaurar el material original cuando no se detecta ning�n objeto
                RestoreOriginalMaterial();

                firstElement = null;
                isDetectingObject = false;
            }
        }

        Debug.Log(firstElement);
    }

    public GameObject GetFirstElement()
    {
        if (isDetectingObject)
            return firstElement;

        return null;
    }

    public void ClearFirstElement()
    {
        // Restaurar el material original al limpiar el objeto seleccionado
        RestoreOriginalMaterial();

        firstElement = null;
        isDetectingObject = false;
    }

    private void RestoreOriginalMaterial()
    {
        // Verificar si hay un objeto seleccionado y si tiene un Renderer y un material original
        if (firstElement != null && firstElement.TryGetComponent(out Renderer objectRenderer) && originalMaterial != null)
        {
            // Restaurar el material original del objeto
            objectRenderer.material = originalMaterial;
            originalMaterial = null;
        }
    }


}
