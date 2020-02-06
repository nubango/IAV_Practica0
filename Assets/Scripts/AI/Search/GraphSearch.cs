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
    using UCM.IAV.IA.Util;

    /**
     * Realiza la búsqueda clásica en su versión para grafos, según el algoritmo GRAPH-SEARCH(problem) del AIMA, aunque con una salvedad:
     * aquí los nodos hijos se añaden a la frontera INCLUSO aunque ya haya otros iguales allí. Esto se hace por si se quiere combinar esta clase con fronteras basadas en colas de prioridad. 
     * Forma parte de la infraestructura necesaria para implementar la búsqueda de manera genérica y flexible.
     * No se ha creado una clase genérica <S, O> para determinar el tipo de Setup y de Operator que se usa.
     */
    public class GraphSearch : QueueSearch {

        // Conjunto con las configuraciones ya exploradas
        private HashSet<object> exploredSetups = new HashSet<object>();

        // Caché (diccionario auxiliar) que asocia configuraciones a sus correspondientes nodos, para guardar ahí los nodos de la frontera y evitar recrearlos para mejorar la eficiencia
        private Dictionary<object, Node> frontierCache = new Dictionary<object, Node>();

        // Lista auxiliar de nodos hijos listos para añadirse a la frontera
        private List<Node> addToFrontierNodes = new List<Node>(); // Se crea aquí y una sola vez, por eficiencia

        // Busca la solución a un problema, usando una cola como frontera (FIFO, LIFO, de prioridad...), y devuelve la lista de operadores para llegar desde la configuración actual a una configuración objetivo
        // Si se ha encontrado un objetivo, se devuelve únicamente la lista de operadores 
        // Si la propia configuración inicial ya es un objetivo, devuelve una lista con un sólo operador: NoOp 
        // Si la búsqueda no logra encontrar un objetivo, se devuelve una lista vacía para representar el fallo
        // En este caso se sobreescribe el método para reinicializar el conjunto de nodos explorados y para así permitir que se puedan hacer múltiples llamadas a él
        public override List<Operator> Search(Problem problem, IQueue<Node> frontier) {
            // Se vacía el conjunto de nodos explorados en cada llamada a este método
            exploredSetups.Clear();
            frontierCache.Clear(); // También se vacía la caché
            return base.Search(problem, frontier);
        }

        // Saca el primer nodo de la frontera (según el tipo de cola que sea esta)
        public override Node PopNodeFromFrontier() {
            Node node = base.PopNodeFromFrontier();
            frontierCache.Remove(node.Setup); // Se borra también de la caché
            return node;
        }

        // Se quita un nodo de la frontera, devolviendo cierto si ha podido hacerse y falso si no está dicho nodo en la frontera 
        // ¡Ojo! Nótese que aunque decimos que la frontera es una cola en realidad en ciertos casos debe permitir operaciones adicionales como esta (borrar un nodo cualquiera), que son más propias de una lista
        public override bool RemoveNodeFromFrontier(Node toRemove) {
            bool removed = base.RemoveNodeFromFrontier(toRemove);

            if (removed)
                frontierCache.Remove(toRemove.Setup); // En caso de hallarlo, se borra también de la caché

            return removed;
        }

        // Dado el nodo a expandir (y considerando el problema), devuelve los nodos hijos que hace falta añadir a la frontera
        public override List<Node> GetResultingNodesToAddToFrontier(Node node, Problem problem) {

            // Se añade la configuración actual a los ya exploradas (aquí no corresponde comprobar si es objetivo) 
            exploredSetups.Add(node.Setup); 
            addToFrontierNodes.Clear(); // Se vacía la lista auxiliar

            // Se expande el nodo, añadiendo todos los nodos hijos resultantes (creados por ExpandNode) a la frontera
            foreach (Node childNode in ExpandNode(node, problem)) {

                // Buscamos si hay un nodo en nuestra caché de frontera con la misma configuración que el nodo hijo
                Node frontierNode;
                frontierCache.TryGetValue(childNode.Setup, out frontierNode);

                // Si no se encuentra dicha configuración en la caché de frontera, ni tampoco es una configuración explorada, el nodo deberá añadirse a la frontera
                bool yesAddToFrontier = false;
                if (frontierNode == null && !exploredSetups.Contains(childNode.Setup)) 
                    yesAddToFrontier = true; 
                // ¡Ojo! En caso contrario y si hay un nodo igual en la frontera, AQUÍ deberíamos comparar costes y sustituir el de la frontera por el nuevo si su coste es menor 

                // Si corresponde, el nodo se mete en la lista de nodos para añadir a la frontera y también a nuestra caché
                if (yesAddToFrontier) {
                    addToFrontierNodes.Add(childNode);
                    frontierCache.Add(childNode.Setup, childNode);
                }
            }

            return addToFrontierNodes;
        }
    }
}