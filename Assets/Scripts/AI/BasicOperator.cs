/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com
    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).
    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com
    Basado en código original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.IA {

    /*
     * Implementación básica de un operador con nombre.
     * Forma parte de la infraestructura necesaria para implementar la búsqueda de manera genérica y flexible.
     */
    public class BasicOperator : Operator {

        public string ID { get; }

        // Construye un operador únicamente con su nombre
        public BasicOperator(string name) {
            this.ID = name;
        }

        // Dice si se trata de un 'No Operador' (NoOp), operador especial que sirve para indicar fallo en la búsqueda 
        public virtual bool isNoOperator() {
            return false;
        }

        // Cadena de texto representativa  
        public override string ToString() {
            return ID;
        }
    }
}