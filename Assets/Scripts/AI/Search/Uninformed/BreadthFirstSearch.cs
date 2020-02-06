/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en código original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.IA.Search.Uninformed {

    using System.Collections.Generic;
    using UCM.IAV.IA;
    using UCM.IAV.IA.Util;
    using UCM.IAV.IA.Search;

    /**
     * Realiza la búsqueda primero en anchura según el algoritmo BREADTH-FIRST-SEARCH(problem) del AIMA.
     * Soporta tamto la versión para grafos como para árboles simplemente pasándole el ejemplar correspondiente de TreeSearch o GraphSearch en el constructor.
     * No se ha creado una clase genérica <S, O> para determinar el tipo de Setup y de Operator que se usa.
     */
    public class BreadthFirstSearch : Search {

        // Internamente en esta búsqueda hay una búsqueda basada en una cola como frontera
        private QueueSearch search;

        // Constructor por defecto, asume la versión para grafos
        public BreadthFirstSearch() : this(new GraphSearch()) { }

        // Constructor que recibe el tipo de búsqueda que realizar 
        // Soporta tamto la versión para grafos como para árboles simplemente pasándole el ejemplar correspondiente de TreeSearch o GraphSearch en el constructor.
        public BreadthFirstSearch(QueueSearch search) {            
            this.search = search;
            // Esta búsqueda aplica la prueba de objetivo a cada nodo cuando es generado, ANTES DE AÑADIRSE A LA FRONTERA y no cuando es seleccionado para expandirse 
            this.search.TestGoalBeforeAddToFrontier = true;
        }

        // Busca la solución a un problema y devuelve la lista de operadores para llegar desde la configuración actual a una configuración objetivo
        public List<Operator> Search(Problem p) {
            // Esta búsqueda utiliza una cola FIFO como frontera
            return search.Search(p, new FIFOQueue<Node>());  
        }

        // Devuelve la información de las métricas de la búsqueda
        public Metrics GetMetrics() {
            return search.Metrics;
        }
    }
}