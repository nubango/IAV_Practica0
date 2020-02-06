/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Puzzles.Model.AI {

    using System;
    using System.Collections.Generic;
    using UCM.IAV.IA;
    using UCM.IAV.IA.Search;
    using UCM.IAV.IA.Search.Uninformed;

    /* 
     * El resolutor de IA especializado en puzles de bloques deslizantes.
     * Se basa en la clase SlidingPuzzle, el modelo l�gico de este tipo de puzles.
     */
    public class SlidingPuzzleSolver { //Todas estas clases podr�an ser abreviadas escribiendo 'SP' en vez de 'SlidingPuzzle', llam�ndolo algo como 'SPSolver'.

        // Estrategias posibles (se podr�an a�adir m�s)
        public enum Strategy { BFS, DFS }
         
        // Los operadores se pueden leer como 'Puedo mover el hueco hacia arriba' o como 'Puedo mover la pieza de arriba del hueco'
        // Tal vez ser�a mejor hacerlo con enumerados
        public static Operator UP = new BasicOperator("Up");
        public static Operator DOWN = new BasicOperator("Down");
        public static Operator LEFT = new BasicOperator("Left");
        public static Operator RIGHT = new BasicOperator("Right");
                
        // El almac�n de m�tricas con informaci�n de la b�squeda (si se ha realizado)
        private Metrics metrics;

        // La funci�n de los operadores aplicables
        private ApplicableOperatorsFunction aoFunction;
        // El modelo de transici�n
        private TransitionModel tModel;
        // La prueba de objetivo (podr�a poner directamente el tipo SlidingPuzzleGoalTest)
        private GoalTest gTest;

        // Construye un resolutor de dimensiones (rows) por (columns), como m�nimo debe ser de 1x1
        // No necesita el puzle en s�, pues eso lo recibir� despu�s
        public SlidingPuzzleSolver(uint rows, uint columns) {
            if (rows == 0) throw new ArgumentException(string.Format("{0} is not a valid rows value", rows), "rows");
            if (columns == 0) throw new ArgumentException(string.Format("{0} is not a valid columns value", columns), "columns");

            aoFunction = SlidingPuzzleFunctionFactory.GetApplicableOperatorsFunction();
            tModel = SlidingPuzzleFunctionFactory.GetTransitionModel();
            gTest = new SlidingPuzzleGoalTest(rows, columns);
        }

        // Resuelve el puzle, en su configuraci�n actual, utilizando la estrategia introducida como par�metro 
        public List<Operator> Solve(SlidingPuzzle initialSetup, SlidingPuzzleSolver.Strategy strategy) {

            // Construimos el problema a partir del puzle que nos llega, que act�a como configuraci�n inicial 
            // No se indica funci�n de coste de paso, con lo que se utilizar� la que viene por defecto
            Problem problem = new Problem(initialSetup, aoFunction, tModel, gTest);  
             
            // Se crea la b�squeda seg�n la estrategia escogida
            Search search = null;
            switch (strategy) {
                case Strategy.BFS: search = new BreadthFirstSearch(); break; // Por defecto se usa un GraphSearch 
                case Strategy.DFS: search = new DepthFirstSearch(new GraphSearch()); break;  // Por defecto se usa un GraphSearch  
                // ... hay cabida para otras estrategias
            }           

            // Se realiza la b�squeda sobre el problema
            List<Operator> operators = search.Search(problem);
            // Al terminar, se guardan las m�tricas resultantes
            metrics = search.GetMetrics();

            return operators;  
        }

        // Devuelve la posici�n que se ve afectada por este operador (la que se mover�a si lo aplic�semos)
        public Position GetOperatedPosition(SlidingPuzzle puzzle, Operator op) {
            if (puzzle == null) throw new ArgumentException("Argument is not valid", "puzzle");  

            if (SlidingPuzzleSolver.UP.Equals(op))
                return puzzle.GapPosition.Up();
            if (SlidingPuzzleSolver.DOWN.Equals(op))
                return puzzle.GapPosition.Down();
            if (SlidingPuzzleSolver.LEFT.Equals(op))
                return puzzle.GapPosition.Left();
            if (SlidingPuzzleSolver.RIGHT.Equals(op))
                return puzzle.GapPosition.Right();

            // Si el operador es nulo, no se entiende o directamente es NoOp respondemos con una excepci�n
            throw new ArgumentException("Argument is not a valid operator", "op");
        }

        // Devuelve las m�tricas resultantes de una b�squeda
        // S�lo tiene sentido llamar a este m�todo tras haber hecho la b�squeda y haber obtenido m�tricas, claro. Si no, puede devolverse null
        public Metrics GetMetrics() { 
            return metrics;
        }        
    }
}