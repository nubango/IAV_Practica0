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
     * Base de todas las clases que necesiten expandir un nodo para obtener todos sus hijos a la vez y obtener métricas, como la búsqueda clásica.
     * Forma parte de la infraestructura necesaria para implementar la búsqueda de manera genérica y flexible. 
     */
    public class NodeExpander {

        // Cadena para expresar el texto de la métrica de nodos expandidos
        public static readonly string TEXT_EXPANDED_NODES = "Expanded nodes";

        // Contiene un almacén interno con las métricas
        public Metrics Metrics { get; protected set;  } 

        // Constructor por defecto
        public NodeExpander() {
            Metrics = new Metrics();
            ClearMetrics();
        }

        // Inicializa las métricas o instrumentación del proceso, poniendo sus valores a cero
        // Será habitual que se vea sobreescrito por las clases hijas
        public virtual void ClearMetrics() {
            Metrics.Set(TEXT_EXPANDED_NODES, 0); // Los nodos expandidos son un número entero
        }

        // Devuelve la lista de todos los nodos hijos del nodo que se desea expandir
        public List<Node> ExpandNode(Node node, Problem problem) {
            List<Node> childNodes = new List<Node>();            
            foreach (Operator op in problem.ApplicableOperatorsFunction.GetApplicableOperators(node.Setup)) {
                object newSetup = problem.TransitionModel.GetResult(node.Setup, op);
                double stepCost = problem.StepCostFunction.GetCost(node.Setup, op, newSetup);
                childNodes.Add(new Node(newSetup, node, op, stepCost));
            }
            Metrics.Set(TEXT_EXPANDED_NODES, Metrics.GetInt(TEXT_EXPANDED_NODES) + 1); // Sumamos 1 nodo expandido más
            return childNodes;
        }
    }
}