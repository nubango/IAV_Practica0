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

    /**
     * Indica si una configuraci�n cualquiera del problema es una configuraci�n objetivo. 
     * Forma parte de la infraestructura necesaria para implementar la b�squeda de manera gen�rica y flexible.
     */
    public interface GoalTest {

        // Devuelve cierto o falso seg�n esta configuraci�n sea o no una configuraci�n objetivo
        bool IsGoal(object setup);
    }
}