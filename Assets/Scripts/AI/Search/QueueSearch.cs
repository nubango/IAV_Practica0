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
     * Realiza la b�squeda cl�sica utilizando alg�n tipo de cola (FIFO, LIFO o de prioridad) como estructura para gestionar la frontera, y expandiendo nodos.  
     * Forma parte de la infraestructura necesaria para implementar la b�squeda de manera gen�rica y flexible.
     * No se ha creado una clase gen�rica <S, O> para determinar el tipo de Setup y de Operator que se usa.
     */
    public abstract class QueueSearch : NodeExpander {

        // Cadenas para expresar los textos de las m�tricas de tama�o de cola, m�ximo tama�o de cola y coste de ruta
        public static readonly string TEXT_QUEUE_SIZE = "Queue size";
        public static readonly string TEXT_MAX_QUEUE_SIZE = "Max. queue size";
        public static readonly string TEXT_PATH_COST = "Path cost";

        // La frontera que se deber�a inicializar en el constructor como FIFO, LIFO, prioridad...
        private IQueue<Node> frontier = null;

        // Centinela para indicar si se debe hacer la prueba de objetivo antes de a�adir el nodo a la frontera o no
        // Ppuede cambiarse din�micamente aunque seguramente ser�a deseable que fuera inmutable y establecido una �nica vez en el constructor
        public bool TestGoalBeforeAddToFrontier { get; set; } = false;

        // Busca la soluci�n a un problema, usando una cola como frontera (FIFO, LIFO, de prioridad...), y devuelve la lista de operadores para llegar desde la configuraci�n actual a una configuraci�n objetivo
        // Si se ha encontrado un objetivo, se devuelve �nicamente la lista de operadores 
        // Si la propia configuraci�n inicial ya es un objetivo, devuelve una lista con un s�lo operador: NoOp 
        // Si la b�squeda no logra encontrar un objetivo, se devuelve una lista vac�a para representar el fallo
        public virtual List<Operator> Search(Problem problem, IQueue<Node> frontier) {

            ClearMetrics();

            // Se inicializa la frontera con la configuraci�n inicial del problema 
            Node root = new Node(problem.InitialSetup);
            // Si debemos hacer la prueba de objetivo antes de meterlo en la frontera, la hacemos...
            if (TestGoalBeforeAddToFrontier) {
                // ... y si resulta que es un objetivo, devolvemos ya la soluci�n
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
                // Sacar un nodo de la frontera (el que corresponda seg�n el tipo de cola que sea la frontera) 
                Node node = PopNodeFromFrontier();
                UnityEngine.Debug.Log("Expanding " + node.ToString());

                SetQueueSizes(frontier.Count);

                // Si no hicimos la prueba de objetivo antes de meterlo en la frontera, toca hacerla ahora...
                if (!TestGoalBeforeAddToFrontier) {
                    // ... y si resulta que es un objetivo, devolvemos ya la soluci�n
                    if (SearchUtils.IsGoal(problem, node)) {

                        SetPathCost(node.PathCost);

                        return SearchUtils.GetOperatorsFromNodes(node.GetPathFromRoot());
                    }
                }

                // Expandimos el nodo, a�adiendo sus hijos a la frontera
                foreach (Node child in GetResultingNodesToAddToFrontier(node, problem)) {
                    // Si debemos hacer la prueba de objetivo antes de meterlo en la frontera, la hacemos...
                    if (TestGoalBeforeAddToFrontier) {
                        // ... y si resulta que es un objetivo, devolvemos ya la soluci�n
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

            // Si la frontera ha quedado vac�a y no hemos dado con ninguna soluci�n, devolvemos fallo (representado por una lista vac�a de operadores)
            return new List<Operator>();
        }

        // Dado el nodo a expandir (y considerando el problema), devuelve los nodos hijos que hace falta a�adir a la frontera
        public abstract List<Node> GetResultingNodesToAddToFrontier(Node node, Problem problem);
        
        // Saca el primer nodo de la frontera (seg�n el tipo de cola que sea esta)
        public virtual Node PopNodeFromFrontier() {
            return frontier.Dequeue();
        }

        // Se quita un nodo de la frontera, devolviendo cierto si ha podido hacerse y falso si no se encuentra dicho nodo en la frontera
        // �Ojo! N�tese que aunque decimos que la frontera es una 'cola' en realidad en ciertos casos debe permitir operaciones adicionales como esta (borrar un nodo cualquiera), que son m�s propias de una lista
        public virtual bool RemoveNodeFromFrontier(Node node) {
            // Todos los tipos de 'colas' est�n obligados a ofrecer un m�todo para eliminar un elemento cualquiera y devolver si han podido hacerlo
            return frontier.Remove(node);   
        }

        // Inicializa las m�tricas o instrumentaci�n del proceso, poniendo sus nuevos valores tambi�n a cero 
        public override void ClearMetrics() {
            base.ClearMetrics();
            Metrics.Set(TEXT_QUEUE_SIZE, 0); // El tama�o de la cola es un entero
            Metrics.Set(TEXT_MAX_QUEUE_SIZE, 0); // El m�ximo tama�o que ha alcanzado la cola es otro entero
            Metrics.Set(TEXT_PATH_COST, 0.0d); // El coste de la ruta es un real
        }

        // Establece las m�tricas de tama�o de la cola y m�ximo tama�o alcanzado en la cola
        private void SetQueueSizes(int queueSize) {

            Metrics.Set(TEXT_QUEUE_SIZE, queueSize);

            // Si se supera el m�ximo tama�o de cola alcanzado, actualizamos el m�ximo
            int maxQSize = Metrics.GetInt(TEXT_MAX_QUEUE_SIZE);
            if (queueSize > maxQSize) 
                Metrics.Set(TEXT_MAX_QUEUE_SIZE, queueSize);
        }

        // Establece la m�trica de coste de ruta
        private void SetPathCost(double pathCost) {
            Metrics.Set(TEXT_PATH_COST, pathCost);
        }
    }
}