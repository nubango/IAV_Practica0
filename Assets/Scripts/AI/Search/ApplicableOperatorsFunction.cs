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

    using System.Collections.Generic;
    using UCM.IAV.IA;

    /**
     * Interfaz de los que saben devolver el conjunto de operadores aplicables a una determinada configuraci�n del problema. 
     * Forma parte de la infraestructura necesaria para implementar la b�squeda de manera gen�rica y flexible. 
     */
    public interface ApplicableOperatorsFunction {

        // Devuelve los operadores aplicables a una determinada configuraci�n del problema 
        // Se devuelve HashSet porque es un conjunto sin orden, aunque tambi�n se podr�a haber decidido devolver una lista
        HashSet<Operator> GetApplicableOperators(object setup);
    }
}