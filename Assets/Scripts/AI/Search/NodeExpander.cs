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

    /**
     * Base de todas las clases que necesiten expandir un nodo para obtener todos sus hijos a la vez y obtener m�tricas, como la b�squeda cl�sica.
     * Forma parte de la infraestructura necesaria para implementar la b�squeda de manera gen�rica y flexible. 
     */
    public class NodeExpander {

        // Cadena para expresar el texto de la m�trica de nodos expandidos
        public static readonly string TEXT_EXPANDED_NODES = "Expanded nodes";

        // Contiene un almac�n interno con las m�tricas
        public Metrics Metrics { get; protected set;  } 

        // Constructor por defecto
        public NodeExpander() {
            Metrics = new Metrics();
            ClearMetrics();
        }

        // Inicializa las m�tricas o instrumentaci�n del proceso, poniendo sus valores a cero
        // Ser� habitual que se vea sobreescrito por las clases hijas
        public virtual void ClearMetrics() {
            Metrics.Set(TEXT_EXPANDED_NODES, 0); // Los nodos expandidos son un n�mero entero
        }

        // Devuelve la lista de todos los nodos hijos del nodo que se desea expandir
        public List<Node> ExpandNode(Node node, Problem problem) {
            List<Node> childNodes = new List<Node>();            
            foreach (Operator op in problem.ApplicableOperatorsFunction.GetApplicableOperators(node.Setup)) {
                object newSetup = problem.TransitionModel.GetResult(node.Setup, op);
                double stepCost = problem.StepCostFunction.GetCost(node.Setup, op, newSetup);
                childNodes.Add(new Node(newSetup, node, op, stepCost));
            }
            Metrics.Set(TEXT_EXPANDED_NODES, Metrics.GetInt(TEXT_EXPANDED_NODES) + 1); // Sumamos 1 nodo expandido m�s
            return childNodes;
        }
    }
}