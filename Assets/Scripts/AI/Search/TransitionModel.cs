/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en c�digo original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.IA.Search {
    
    using UCM.IAV.IA;

    /**
     * El modelo de transici�n que permite obtener la configuraci�n resultante (o sucesora) tras aplicar un operador a una configuraci�n dada.
     * Forma parte de la infraestructura necesaria para implementar la b�squeda de manera gen�rica y flexible.
     */
    public interface TransitionModel {
        
        // Dado una configuraci�n y un operador, devuelve la configuraci�n resultante
        object GetResult(object setup, Operator op);
    }
}