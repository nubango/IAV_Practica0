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
     * Realiza la búsqueda clásica utilizando algún tipo de cola (FIFO, LIFO o de prioridad) como estructura para gestionar la frontera, y expandiendo nodos.  
     * Forma parte de la infraestructura necesaria para implementar la búsqueda de manera genérica y flexible.
     * No se ha creado una clase genérica <S, O> para determinar el tipo de Setup y de Operator que se usa.
     */
    public abstract class QueueSearch : NodeExpander {

        // Cadenas para expresar los textos de las métricas de tamaño de cola, máximo tamaño de cola y coste de ruta
        public static readonly string TEXT_QUEUE_SIZE = "Queue size";
        public static readonly string TEXT_MAX_QUEUE_SIZE = "Max. queue size";
        public static readonly string TEXT_PATH_COST = "Path cost";

        // La frontera que se debería inicializar en el constructor como FIFO, LIFO, prioridad...
        private IQueue<Node> frontier = null;

        // Centinela para indicar si se debe hacer la prueba de objetivo antes de añadir el nodo a la frontera o no
        // Ppuede cambiarse dinámicamente aunque seguramente sería deseable que fuera inmutable y establecido una única vez en el constructor
        public bool TestGoalBeforeAddToFrontier { get; set; } = false;

        // Busca la solución a un problema, usando una cola como frontera (FIFO, LIFO, de prioridad...), y devuelve la lista de operadores para llegar desde la configuración actual a una configuración objetivo
        // Si se ha encontrado un objetivo, se devuelve únicamente la lista de operadores 
        // Si la propia configuración inicial ya es un objetivo, devuelve una lista con un sólo operador: NoOp 
        // Si la búsqueda no logra encontrar un objetivo, se devuelve una lista vacía para representar el fallo
        public virtual List<Operator> Search(Problem problem, IQueue<Node> frontier) {

            ClearMetrics();

            // Se inicializa la frontera con la configuración inicial del problema 
            Node root = new Node(problem.InitialSetup);
            // Si debemos hacer la prueba de objetivo antes de meterlo en la frontera, la hacemos...
            if (TestGoalBeforeAddToFrontier) {
                // ... y si resulta que es un objetivo, devolvemos ya la solución
                if (SearchUtils.IsGoal(problem, root)) {

                    SetPathCost(root.PathCost);

                    return SearchUtils.GetOperatorsFromNodes(root.GetPathFromRoot());
                }
            }
            this.frontier = frontier;
            this.frontier.Enqueue(root);

            SetQueueSizes(frontier.Count);

            // Mientras que queden nodos en la frontera, expandirlos y buscar en sus hijos
            while (!(frontier.Count == 0)) {
                // Sacar un nodo de la frontera (el que corresponda según el tipo de cola que sea la frontera) 
                Node node = PopNodeFromFrontier();
                UnityEngine.Debug.Log("Expanding " + node.ToString());

                SetQueueSizes(frontier.Count);

                // Si no hicimos la prueba de objetivo antes de meterlo en la frontera, toca hacerla ahora...
                if (!TestGoalBeforeAddToFrontier) {
                    // ... y si resulta que es un objetivo, devolvemos ya la solución
                    if (SearchUtils.IsGoal(problem, node)) {

                        SetPathCost(node.PathCost);

                        return SearchUtils.GetOperatorsFromNodes(node.GetPathFromRoot());
                    }
                }

                // Expandimos el nodo, añadiendo sus hijos a la frontera
                foreach (Node child in GetResultingNodesToAddToFrontier(node, problem)) {
                    // Si debemos hacer la prueba de objetivo antes de meterlo en la frontera, la hacemos...
                    if (TestGoalBeforeAddToFrontier) {
                        // ... y si resulta que es un objetivo, devolvemos ya la solución
                        if (SearchUtils.IsGoal(problem, child)) {

                            SetPathCost(child.PathCost);

                            return SearchUtils.GetOperatorsFromNodes(child.GetPathFromRoot());
                        }
                    }

                    frontier.Enqueue(child);
                    UnityEngine.Debug.Log("Adding " + child.ToString() + " to the frontier");
                }

                SetQueueSizes(frontier.Count);
            }

            // Si la frontera ha quedado vacía y no hemos dado con ninguna solución, devolvemos fallo (representado por una lista vacía de operadores)
            return new List<Operator>();
        }

        // Dado el nodo a expandir (y considerando el problema), devuelve los nodos hijos que hace falta añadir a la frontera
        public abstract List<Node> GetResultingNodesToAddToFrontier(Node node, Problem problem);
        
        // Saca el primer nodo de la frontera (según el tipo de cola que sea esta)
        public virtual Node PopNodeFromFrontier() {
            return frontier.Dequeue();
        }

        // Se quita un nodo de la frontera, devolviendo cierto si ha podido hacerse y falso si no se encuentra dicho nodo en la frontera
        // ¡Ojo! Nótese que aunque decimos que la frontera es una 'cola' en realidad en ciertos casos debe permitir operaciones adicionales como esta (borrar un nodo cualquiera), que son más propias de una lista
        public virtual bool RemoveNodeFromFrontier(Node node) {
            // Todos los tipos de 'colas' están obligados a ofrecer un método para eliminar un elemento cualquiera y devolver si han podido hacerlo
            return frontier.Remove(node);   
        }

        // Inicializa las métricas o instrumentación del proceso, poniendo sus nuevos valores también a cero 
        public override void ClearMetrics() {
            base.ClearMetrics();
            Metrics.Set(TEXT_QUEUE_SIZE, 0); // El tamaño de la cola es un entero
            Metrics.Set(TEXT_MAX_QUEUE_SIZE, 0); // El máximo tamaño que ha alcanzado la cola es otro entero
            Metrics.Set(TEXT_PATH_COST, 0.0d); // El coste de la ruta es un real
        }

        // Establece las métricas de tamaño de la cola y máximo tamaño alcanzado en la cola
        private void SetQueueSizes(int queueSize) {

            Metrics.Set(TEXT_QUEUE_SIZE, queueSize);

            // Si se supera el máximo tamaño de cola alcanzado, actualizamos el máximo
            int maxQSize = Metrics.GetInt(TEXT_MAX_QUEUE_SIZE);
            if (queueSize > maxQSize) 
                Metrics.Set(TEXT_MAX_QUEUE_SIZE, queueSize);
        }

        // Establece la métrica de coste de ruta
        private void SetPathCost(double pathCost) {
            Metrics.Set(TEXT_PATH_COST, pathCost);
        }
    }
}