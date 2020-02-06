/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Puzzles {

    using System;
    using UnityEngine;
    using Model;

    /* 
     * Bloque con texto que responde a los clics del ratón y puede ser movido sobre un tablero de bloques.
     * Es un componente diseñado para Unity 2018.2, se asume que propiedades como su tamaño serán estándar.
     */
    public class MovableBlock : MonoBehaviour {

        // El tablero de bloques al que notifica
        private BlockBoard board;

        // La posición asociada (el tablero de bloques la guarda aquí por eficiencia)
        public Position position;

        // Inicializa con el tablero de bloques y el texto (siempre que haya hijo con componente TextMesh), sólo si todavía no tiene puesto ningún tablero de bloques  
        // El tablero de bloques recibido no puede ser nulo, pero el texto sí (representa un bloque no visible)
        public void Initialize(BlockBoard board, string text) {
            if (board == null) throw new ArgumentNullException(nameof(board));

            this.board = board;
            this.gameObject.SetActive(text != null); // Se pondrá activo o no según el texto (esto lo hacemos para reactivar bloques que hubieran sido agujeros antes (en un reinicio)

            if (text != null && GetComponentInChildren<TextMesh>() != null)
                this.GetComponentInChildren<TextMesh>().text = text;

            Debug.Log(ToString() + " initialized.");
        }

        // Intercambia la posición física en la escena con otro bloque
        public void ExchangeTransform(MovableBlock block) {
            if (block == null) throw new ArgumentNullException(nameof(block));

            // Se usa una variable auxiliar para hacer el intercambio
            Vector3 auxTransformPosition = block.transform.position;

            block.transform.position = transform.position;

            transform.position = auxTransformPosition;
        }

        // Notifica el intento de movimiento al tablero de bloques (si lo hay), cuando se recibe un clic completo de ratón (apretar y soltar) 
        // Podría reaccionarse con un sonido si el intento falla, aunque ahora no se hace nada
        // La he puesto pública para que se puedan simular pulsaciones sobre un bloque desde el gestor
        public bool OnMouseUpAsButton() {
            if (board == null) throw new InvalidOperationException("This object has not been initialized");

            Debug.Log("Trying to move " + ToString() + "...");
            if (board.CanMove(this)) {

                board.UserInteraction(); // Aviso de que lo he tocado para que se pongan los contadores a cero (por si contenían algo)
                board.Move(this, BlockBoard.USER_DELAY);
                Debug.Log(ToString() + " was moved.");

                return true;
            }

            Debug.Log(ToString() + " cannot be moved.");
            return false;
        }

        // Cadena de texto representativa
        public override string ToString() {
            return "Block[" + this.GetComponentInChildren<TextMesh>()?.text.ToString() + "] at " + position;
        }
    }
}