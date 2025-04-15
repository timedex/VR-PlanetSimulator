using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class OrbitasPlanetas : MonoBehaviour
{
    private Transform pivote;
    public int orbita = 0;
    private bool Encender = false;
    public bool Gravedad = false;
    public GameObject PlanetaAtraido;

    public Planetas planetas;
    public bool Regresar = false;
    public GameObject explosionPrefab; // A�adir la referencia al prefab de explosi�n
    public GameObject canvas;
    void Awake()
    {
        pivote = GameObject.FindGameObjectWithTag("Sun").transform;
    }

    void Start()
    {
        // Mostrar el canvas cuando se crea el planeta
        if (canvas != null)
        {
            canvas.SetActive(true);
        }

        // A�adir eventos para mostrar y ocultar el canvas cuando se agarra y se suelta el planeta
        var xrGrabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (xrGrabInteractable == null)
        {
            xrGrabInteractable = gameObject.AddComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        }
        xrGrabInteractable.selectEntered.AddListener(OnSelectEntered);
        xrGrabInteractable.selectExited.AddListener(OnSelectExited);
    }

    void Update()
    {
        if (Gravedad)
        {
            IniciarGravedad(PlanetaAtraido);
        }
        if (Encender)
        {
            switch (orbita)
            {
                case 0:
                    transform.RotateAround(pivote.transform.position, Vector3.up, planetas.velocidad * Time.deltaTime);
                    return;
                case 1:
                    transform.RotateAround(pivote.transform.position, Vector3.right, planetas.velocidad * Time.deltaTime);
                    return;
                case 2:
                    transform.RotateAround(pivote.transform.position, new Vector3(45, 45, 0), planetas.velocidad * Time.deltaTime);
                    return;
                case 3:
                    transform.RotateAround(pivote.transform.position, new Vector3(5, -15, 0), planetas.velocidad * Time.deltaTime);
                    return;
            }
        }
        if (Regresar)
        {
            transform.position = Vector3.MoveTowards(transform.position, planetas.Orbita, 3 * Time.deltaTime);
            if (transform.position == planetas.Orbita)
            {
                Encender = true;
                Regresar = false;
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                // Desactivar el canvas cuando el planeta alcanza su �rbita
                if (canvas != null)
                {
                    canvas.SetActive(false);
                }
            }
        }
    }

    public void CambiarOrbita(int valor)
    {
        Encender = false;
        orbita = valor;
        Regresar = true;
    }

    public void Inicio()
    {
        Regresar = true;
    }

    public void Diriguirse()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        GameObject ObjetoPlaneta = Instantiate(planetas.PrefabPlaneta);
        ObjetoPlaneta.transform.position = transform.position;
        ObjetoPlaneta.GetComponent<OrbitasPlanetas>().planetas = planetas;
        ObjetoPlaneta.GetComponent<OrbitasPlanetas>().Inicio();
        Destroy(gameObject);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        var canvas = transform.Find("Canvas").gameObject;
        if (canvas != null)
        {
            canvas.SetActive(true);
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        var canvas = transform.Find("Canvas").gameObject;
        if (canvas != null && !Regresar) // Asegurarse de que no estamos en el proceso de regresar a la �rbita
        {
            canvas.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Planeta"))
        {
            Explode();
            collision.gameObject.GetComponent<OrbitasPlanetas>().Explode(); // Hacer explotar el otro planeta
        }
    }

    void Explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    void IniciarGravedad(GameObject Planeta)
    {
        transform.position = Vector3.MoveTowards(transform.position, Planeta.transform.position, 0.6f * Time.deltaTime);
    }
}




