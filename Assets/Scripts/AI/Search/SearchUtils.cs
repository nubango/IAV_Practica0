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
     * Utilidades para realizar las búsquedas, como obtener todos los operadores que fueron aplicados para generar una lista de nodos (una ruta), o comprobar si un nodo contiene una solución o no para el problema.
     * Forma parte de la infraestructura necesaria para implementar la búsqueda de manera genérica y flexible.
     */
    public class SearchUtils {

        // Devuelve la lista de operadores que fueron aplicados para generar una lista de nodos (una ruta)
        public static List<Operator> GetOperatorsFromNodes(List<Node> nodeList) {
            List<Operator> operators = new List<Operator>();

            if (nodeList.Count == 1)
                // Si sólo hay un nodo, es porque es el raíz y ya ha pasado la prueba de objetivo, con lo que se devuelve NoOp 
                operators.Add(NoOperator.Instance);
            else // Si no, sacamos los operadores de los demás nodos, ignorando el del raíz (que siempre es NoOp) 
                for (int i = 1; i < nodeList.Count; i++) { // Por eso empezamos a contar desde 1
                    Node node = nodeList[i];
                    operators.Add(node.Operator);
                }

            return operators;
        }

        // Devuelve si es cierto o no que un nodo contenga una solución para el problema
        // Realmente este método sólo tiene interés para meter comprobaciones adicionales, ahora mismo no hace nada especial
        public static bool IsGoal(Problem p, Node n) {

            bool isGoal = false;
            GoalTest gt = p.GoalTest;
            if (gt.IsGoal(n.Setup))
                // Si usáramos una interfaz adicional a GoalTest (SolutionChecker se llama la clase) aquí dentro deberíamos comprobar adicionalmente
                // si es una solución aceptable o no (IsAcceptableSolution se llama el método) 
                isGoal = true;
            
            return isGoal;
        }
    }
}