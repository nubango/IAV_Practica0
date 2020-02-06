/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en c�digo original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.IA.Search.Uninformed {

    using System.Collections.Generic;
    using UCM.IAV.IA;
    using UCM.IAV.IA.Util;
    using UCM.IAV.IA.Search;

    /**
     * Realiza la b�squeda primero en anchura seg�n el algoritmo BREADTH-FIRST-SEARCH(problem) del AIMA.
     * Soporta tamto la versi�n para grafos como para �rboles simplemente pas�ndole el ejemplar correspondiente de TreeSearch o GraphSearch en el constructor.
     * No se ha creado una clase gen�rica <S, O> para determinar el tipo de Setup y de Operator que se usa.
     */
    public class BreadthFirstSearch : Search {

        // Internamente en esta b�squeda hay una b�squeda basada en una cola como frontera
        private QueueSearch search;

        // Constructor por defecto, asume la versi�n para grafos
        public BreadthFirstSearch() : this(new GraphSearch()) { }

        // Constructor que recibe el tipo de b�squeda que realizar 
        // Soporta tamto la versi�n para grafos como para �rboles simplemente pas�ndole el ejemplar correspondiente de TreeSearch o GraphSearch en el constructor.
        public BreadthFirstSearch(QueueSearch search) {            
            this.search = search;
            // Esta b�squeda aplica la prueba de objetivo a cada nodo cuando es generado, ANTES DE A�ADIRSE A LA FRONTERA y no cuando es seleccionado para expandirse 
            this.search.TestGoalBeforeAddToFrontier = true;
        }

        // Busca la soluci�n a un problema y devuelve la lista de operadores para llegar desde la configuraci�n actual a una configuraci�n objetivo
        public List<Operator> Search(Problem p) {
            // Esta b�squeda utiliza una cola FIFO como frontera
            return search.Search(p, new FIFOQueue<Node>());  
        }

        // Devuelve la informaci�n de las m�tricas de la b�squeda
        public Metrics GetMetrics() {
            return search.Metrics;
        }
    }
}