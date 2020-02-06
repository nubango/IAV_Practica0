/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Puzzles {

    using UnityEngine;

    /*
     * Gestor que permite hacer zoom con la cámara, para ver mejor la escena.
     */
    public class CameraManager : MonoBehaviour {

        private static readonly string MOUSE_SCROLLWHEEL = "Mouse ScrollWheel";
        private static readonly float ZOOM_SPEED = 5.0f;

        // Actualiza la posición de la cámara en función de cómo esté la rueda del ratón
        void Update() {
            float scroll = Input.GetAxis(MOUSE_SCROLLWHEEL);
            transform.Translate(0, 0, scroll * ZOOM_SPEED, Space.Self);
        }
    }
}
