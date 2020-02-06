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
     * Realiza la búsqueda según la estrategia con que se quiera implementar.
     * No se ha creado una clase genérica <S, O> para determinar el tipo de Setup y de Operator que se usa.
     */
    public interface Search {

        // Busca la solución a un problema y devuelve la lista de operadores para llegar desde la configuración actual a una configuración objetivo
        List<Operator> Search(Problem p);

        // Devuelve la información de las métricas de la búsqueda
        Metrics GetMetrics();
    }
}