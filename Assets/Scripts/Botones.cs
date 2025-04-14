using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Botones : MonoBehaviour
{
    public GameObject Boton;
    public Planetas[] planeta;
    public GameObject Contenedor;

    private void Awake()
    {
        for (int i = 0; i < planeta.Length; i++)
        {
            GameObject botones;
            botones = Instantiate(Boton, Contenedor.transform);
            botones.name = planeta[i].PlanetaPosicion.ToString();
            botones.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = planeta[i].PlanetaName;
            botones.GetComponent<CrearPlaneta>().Planeta = planeta[i];
            botones.GetComponent<Image>().sprite = planeta[i].ImagenPlaneta;
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
