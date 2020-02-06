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
     * Realiza la b�squeda primero en profundidad, cuyo algoritmo expl�cito se comenta pero no se detalla el AIMA y que consiste en expandir siempre el nodo m�s profundo de la frontera.
     * Soporta tamto la versi�n para grafos como para �rboles simplemente pas�ndole el ejemplar correspondiente de TreeSearch o GraphSearch en el constructor.
     * No se ha creado una clase gen�rica <S, O> para determinar el tipo de Setup y de Operator que se usa.
     */
    public class DepthFirstSearch : Search {

        // Internamente en esta b�squeda hay una b�squeda basada en una cola como frontera
        QueueSearch search;

        // Constructor por defecto, asume la versi�n para grafos (en el caso del DFS podr�a asumirse la versi�n para �rboles)
        public DepthFirstSearch() : this(new GraphSearch()) { }

        // Constructor que recibe el tipo de b�squeda que realizar 
        // Soporta tamto la versi�n para grafos como para �rboles simplemente pas�ndole el ejemplar correspondiente de TreeSearch o GraphSearch en el constructor.
        public DepthFirstSearch(QueueSearch search) {
            this.search = search;
            // Esta b�squeda aplica la prueba de objetivo a cada nodo cuando es seleccionado para expandirse, no antes
        }

        // Busca la soluci�n a un problema y devuelve la lista de operadores para llegar desde la configuraci�n actual a una configuraci�n objetivo
        public List<Operator> Search(Problem p) {
            // Esta b�squeda utiliza una cola FIFO como frontera
            return search.Search(p, new LIFOQueue<Node>()); 
        }

        // Devuelve la informaci�n de las m�tricas de la b�squeda
        public Metrics GetMetrics() {
            return search.Metrics;
        }
    }
}