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
     * Asigna un coste a cada paso de la soluci�n (un operador aplicado sobre una configuraci�n que resulta en otra).
     * Forma parte de la infraestructura necesaria para implementar la b�squeda de manera gen�rica y flexible.
     */
    public interface StepCostFunction {

        // Devuelve el coste del paso de la soluci�n (configuraci�n inicial, operador, configuraci�n final) recibido en los par�metros
        double GetCost(object initialSetup, Operator op, object resultSetup);
    }
}