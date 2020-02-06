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
     * Realiza la búsqueda primero en profundidad, cuyo algoritmo explícito se comenta pero no se detalla el AIMA y que consiste en expandir siempre el nodo más profundo de la frontera.
     * Soporta tamto la versión para grafos como para árboles simplemente pasándole el ejemplar correspondiente de TreeSearch o GraphSearch en el constructor.
     * No se ha creado una clase genérica <S, O> para determinar el tipo de Setup y de Operator que se usa.
     */
    public class DepthFirstSearch : Search {

        // Internamente en esta búsqueda hay una búsqueda basada en una cola como frontera
        QueueSearch search;

        // Constructor por defecto, asume la versión para grafos (en el caso del DFS podría asumirse la versión para árboles)
        public DepthFirstSearch() : this(new GraphSearch()) { }

        // Constructor que recibe el tipo de búsqueda que realizar 
        // Soporta tamto la versión para grafos como para árboles simplemente pasándole el ejemplar correspondiente de TreeSearch o GraphSearch en el constructor.
        public DepthFirstSearch(QueueSearch search) {
            this.search = search;
            // Esta búsqueda aplica la prueba de objetivo a cada nodo cuando es seleccionado para expandirse, no antes
        }

        // Busca la solución a un problema y devuelve la lista de operadores para llegar desde la configuración actual a una configuración objetivo
        public List<Operator> Search(Problem p) {
            // Esta búsqueda utiliza una cola FIFO como frontera
            return search.Search(p, new LIFOQueue<Node>()); 
        }

        // Devuelve la información de las métricas de la búsqueda
        public Metrics GetMetrics() {
            return search.Metrics;
        }
    }
}