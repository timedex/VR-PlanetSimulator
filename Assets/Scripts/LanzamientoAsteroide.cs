using UnityEngine;

public class LanzamientoAsteroide : MonoBehaviour
{
    private bool lanzar = false;
    private Vector3 direccionLanzamiento;
    private float velocidadLanzamiento = 10f;
    private Transform pivote;

    public GameObject asteroidePrefab;
    public Transform manoDelJugador; // Aseg�rate de asignar el objeto que representa la mano desde el Inspector

    void Awake()
    {
        pivote = GameObject.FindGameObjectWithTag("Sun").transform;
    }

    void Update()
    {
        if (lanzar)
        {
            // Instanciar el asteroide en la mano del jugador
            GameObject asteroide = Instantiate(asteroidePrefab, manoDelJugador.position, Quaternion.identity);
            // Aplicar una velocidad al asteroide en la direcci�n de lanzamiento
            asteroide.GetComponent<Rigidbody>().linearVelocity = velocidadLanzamiento * direccionLanzamiento;
            // Restablecer la bandera de lanzar a falso
            lanzar = false;
        }
    }

    public void LanzarAsteroide()
    {
        // Calcular la direcci�n de lanzamiento
        direccionLanzamiento = manoDelJugador.forward;
        // Indicar que se debe lanzar el asteroide
        lanzar = true;
    }
}


