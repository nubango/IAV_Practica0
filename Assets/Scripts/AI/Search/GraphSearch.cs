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
    using UCM.IAV.IA.Util;

    /**
     * Realiza la b�squeda cl�sica en su versi�n para grafos, seg�n el algoritmo GRAPH-SEARCH(problem) del AIMA, aunque con una salvedad:
     * aqu� los nodos hijos se a�aden a la frontera INCLUSO aunque ya haya otros iguales all�. Esto se hace por si se quiere combinar esta clase con fronteras basadas en colas de prioridad. 
     * Forma parte de la infraestructura necesaria para implementar la b�squeda de manera gen�rica y flexible.
     * No se ha creado una clase gen�rica <S, O> para determinar el tipo de Setup y de Operator que se usa.
     */
    public class GraphSearch : QueueSearch {

        // Conjunto con las configuraciones ya exploradas
        private HashSet<object> exploredSetups = new HashSet<object>();

        // Cach� (diccionario auxiliar) que asocia configuraciones a sus correspondientes nodos, para guardar ah� los nodos de la frontera y evitar recrearlos para mejorar la eficiencia
        private Dictionary<object, Node> frontierCache = new Dictionary<object, Node>();

        // Lista auxiliar de nodos hijos listos para a�adirse a la frontera
        private List<Node> addToFrontierNodes = new List<Node>(); // Se crea aqu� y una sola vez, por eficiencia

        // Busca la soluci�n a un problema, usando una cola como frontera (FIFO, LIFO, de prioridad...), y devuelve la lista de operadores para llegar desde la configuraci�n actual a una configuraci�n objetivo
        // Si se ha encontrado un objetivo, se devuelve �nicamente la lista de operadores 
        // Si la propia configuraci�n inicial ya es un objetivo, devuelve una lista con un s�lo operador: NoOp 
        // Si la b�squeda no logra encontrar un objetivo, se devuelve una lista vac�a para representar el fallo
        // En este caso se sobreescribe el m�todo para reinicializar el conjunto de nodos explorados y para as� permitir que se puedan hacer m�ltiples llamadas a �l
        public override List<Operator> Search(Problem problem, IQueue<Node> frontier) {
            // Se vac�a el conjunto de nodos explorados en cada llamada a este m�todo
            exploredSetups.Clear();
            frontierCache.Clear(); // Tambi�n se vac�a la cach�
            return base.Search(problem, frontier);
        }

        // Saca el primer nodo de la frontera (seg�n el tipo de cola que sea esta)
        public override Node PopNodeFromFrontier() {
            Node node = base.PopNodeFromFrontier();
            frontierCache.Remove(node.Setup); // Se borra tambi�n de la cach�
            return node;
        }

        // Se quita un nodo de la frontera, devolviendo cierto si ha podido hacerse y falso si no est� dicho nodo en la frontera 
        // �Ojo! N�tese que aunque decimos que la frontera es una cola en realidad en ciertos casos debe permitir operaciones adicionales como esta (borrar un nodo cualquiera), que son m�s propias de una lista
        public override bool RemoveNodeFromFrontier(Node toRemove) {
            bool removed = base.RemoveNodeFromFrontier(toRemove);

            if (removed)
                frontierCache.Remove(toRemove.Setup); // En caso de hallarlo, se borra tambi�n de la cach�

            return removed;
        }

        // Dado el nodo a expandir (y considerando el problema), devuelve los nodos hijos que hace falta a�adir a la frontera
        public override List<Node> GetResultingNodesToAddToFrontier(Node node, Problem problem) {

            // Se a�ade la configuraci�n actual a los ya exploradas (aqu� no corresponde comprobar si es objetivo) 
            exploredSetups.Add(node.Setup); 
            addToFrontierNodes.Clear(); // Se vac�a la lista auxiliar

            // Se expande el nodo, a�adiendo todos los nodos hijos resultantes (creados por ExpandNode) a la frontera
            foreach (Node childNode in ExpandNode(node, problem)) {

                // Buscamos si hay un nodo en nuestra cach� de frontera con la misma configuraci�n que el nodo hijo
                Node frontierNode;
                frontierCache.TryGetValue(childNode.Setup, out frontierNode);

                // Si no se encuentra dicha configuraci�n en la cach� de frontera, ni tampoco es una configuraci�n explorada, el nodo deber� a�adirse a la frontera
                bool yesAddToFrontier = false;
                if (frontierNode == null && !exploredSetups.Contains(childNode.Setup)) 
                    yesAddToFrontier = true; 
                // �Ojo! En caso contrario y si hay un nodo igual en la frontera, AQU� deber�amos comparar costes y sustituir el de la frontera por el nuevo si su coste es menor 

                // Si corresponde, el nodo se mete en la lista de nodos para a�adir a la frontera y tambi�n a nuestra cach�
                if (yesAddToFrontier) {
                    addToFrontierNodes.Add(childNode);
                    frontierCache.Add(childNode.Setup, childNode);
                }
            }

            return addToFrontierNodes;
        }
    }
}