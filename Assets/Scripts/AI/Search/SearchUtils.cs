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
     * Utilidades para realizar las b�squedas, como obtener todos los operadores que fueron aplicados para generar una lista de nodos (una ruta), o comprobar si un nodo contiene una soluci�n o no para el problema.
     * Forma parte de la infraestructura necesaria para implementar la b�squeda de manera gen�rica y flexible.
     */
    public class SearchUtils {

        // Devuelve la lista de operadores que fueron aplicados para generar una lista de nodos (una ruta)
        public static List<Operator> GetOperatorsFromNodes(List<Node> nodeList) {
            List<Operator> operators = new List<Operator>();

            if (nodeList.Count == 1)
                // Si s�lo hay un nodo, es porque es el ra�z y ya ha pasado la prueba de objetivo, con lo que se devuelve NoOp 
                operators.Add(NoOperator.Instance);
            else // Si no, sacamos los operadores de los dem�s nodos, ignorando el del ra�z (que siempre es NoOp) 
                for (int i = 1; i < nodeList.Count; i++) { // Por eso empezamos a contar desde 1
                    Node node = nodeList[i];
                    operators.Add(node.Operator);
                }

            return operators;
        }

        // Devuelve si es cierto o no que un nodo contenga una soluci�n para el problema
        // Realmente este m�todo s�lo tiene inter�s para meter comprobaciones adicionales, ahora mismo no hace nada especial
        public static bool IsGoal(Problem p, Node n) {

            bool isGoal = false;
            GoalTest gt = p.GoalTest;
            if (gt.IsGoal(n.Setup))
                // Si us�ramos una interfaz adicional a GoalTest (SolutionChecker se llama la clase) aqu� dentro deber�amos comprobar adicionalmente
                // si es una soluci�n aceptable o no (IsAcceptableSolution se llama el m�todo) 
                isGoal = true;
            
            return isGoal;
        }
    }
}