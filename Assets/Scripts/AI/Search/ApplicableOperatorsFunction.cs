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

    using System.Collections.Generic;
    using UCM.IAV.IA;

    /**
     * Interfaz de los que saben devolver el conjunto de operadores aplicables a una determinada configuración del problema. 
     * Forma parte de la infraestructura necesaria para implementar la búsqueda de manera genérica y flexible. 
     */
    public interface ApplicableOperatorsFunction {

        // Devuelve los operadores aplicables a una determinada configuración del problema 
        // Se devuelve HashSet porque es un conjunto sin orden, aunque también se podría haber decidido devolver una lista
        HashSet<Operator> GetApplicableOperators(object setup);
    }
}