/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en código original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.IA.Search {
    
    using UCM.IAV.IA;

    /**
     * El modelo de transición que permite obtener la configuración resultante (o sucesora) tras aplicar un operador a una configuración dada.
     * Forma parte de la infraestructura necesaria para implementar la búsqueda de manera genérica y flexible.
     */
    public interface TransitionModel {
        
        // Dado una configuración y un operador, devuelve la configuración resultante
        object GetResult(object setup, Operator op);
    }
}