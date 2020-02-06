/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Puzzles.Model.AI {

    using System;
    using System.Collections.Generic;
    using UCM.IAV.IA;
    using UCM.IAV.IA.Search;

    /*
     * Factoría para obtener componentes del problema de los puzles de bloques deslizantes, evitando tener que crearlos cada vez que creemos un resolutor de dicho problema.
     * Ahora mismo permite obtener la función que da los operadores aplicables y el modelo de transición.
     */
    public class SlidingPuzzleFunctionFactory {

        // La función de operadores aplicables
        private static ApplicableOperatorsFunction applicableOperatorsFunction = null;
        // El modelo de transición
        private static TransitionModel transitionModel = null;

        // Devuelve la función que da los operadores aplicables, creándola por primera vez si no existiera
        public static ApplicableOperatorsFunction GetApplicableOperatorsFunction() {
            if (applicableOperatorsFunction == null) 
                applicableOperatorsFunction = new SlidingPuzzleApplicableOperatorsFunction();
            return applicableOperatorsFunction;
        }

        // Devuelve el modelo de transición, creándolo por primera vez si no existiera
        public static TransitionModel GetTransitionModel() {
            if (transitionModel == null)
                transitionModel = new SlidingPuzzleResultFunction();
            return transitionModel;
        }

        // Clase interna privada que saben devolver el conjunto de operadores aplicables a una determinada configuración del problema.
        // Los operadores se pueden leer como 'Puedo mover el hueco hacia arriba' o como 'Puedo mover la pieza de arriba del hueco'
        private class SlidingPuzzleApplicableOperatorsFunction : ApplicableOperatorsFunction {

            // Devuelve los operadores aplicables a una determinada configuración del problema
            // Se devuelve HashSet porque es un conjunto sin orden, aunque también se podría haber decidido devolver una lista
            public HashSet<Operator> GetApplicableOperators(object setup) {
                if (setup == null || !(setup is SlidingPuzzle)) throw new ArgumentException("Setup is not a valid SlidingPuzzle");

                // Lo que recibe es un SlidingPuzzle
                SlidingPuzzle puzzle = (SlidingPuzzle)setup;  

                HashSet<Operator> operators = new HashSet<Operator>();
                if (puzzle.CanMoveUp(puzzle.GapPosition)) 
                    operators.Add(SlidingPuzzleSolver.UP);
                if (puzzle.CanMoveDown(puzzle.GapPosition))
                    operators.Add(SlidingPuzzleSolver.DOWN);
                if (puzzle.CanMoveLeft(puzzle.GapPosition))
                    operators.Add(SlidingPuzzleSolver.LEFT);
                if (puzzle.CanMoveRight(puzzle.GapPosition))
                    operators.Add(SlidingPuzzleSolver.RIGHT);

                return operators;
            }
        }

        /**
         * Clase interna privada que permite obtener la configuración resultante (o sucesora) tras aplicar un operador a una configuración dada. 
         */
        private class SlidingPuzzleResultFunction : TransitionModel {

            // Dado una configuración y un operador, devuelve la configuración resultante
            public object GetResult(object setup, Operator op) {
                if (setup == null || !(setup is SlidingPuzzle)) throw new ArgumentException("Setup is not a valid SlidingPuzzle");

                // Lo que recibe es un SlidingPuzzle
                SlidingPuzzle puzzle = (SlidingPuzzle)setup;

                // Se clona el puzle deslizante a nivel profundo
                SlidingPuzzle puzzleClone = puzzle.DeepClone();

                if (SlidingPuzzleSolver.UP.Equals(op))
                    if (puzzleClone.CanMoveUp(puzzleClone.GapPosition))
                        puzzleClone.MoveUp(puzzleClone.GapPosition);
                    else // No puede ocurrir que el operador aplicable no funcione, porque ya se comprobó que era aplicable
                        throw new InvalidOperationException("This operator is not working propertly");

                if (SlidingPuzzleSolver.DOWN.Equals(op))
                    if (puzzleClone.CanMoveDown(puzzleClone.GapPosition))
                        puzzleClone.MoveDown(puzzleClone.GapPosition);
                    else // No puede ocurrir que el operador aplicable no funcione, porque ya se comprobó que era aplicable
                        throw new InvalidOperationException("This operator is not working propertly");

                if (SlidingPuzzleSolver.LEFT.Equals(op))
                    if (puzzleClone.CanMoveLeft(puzzleClone.GapPosition))
                        puzzleClone.MoveLeft(puzzleClone.GapPosition);
                    else // No puede ocurrir que el operador aplicable no funcione, porque ya se comprobó que era aplicable
                        throw new InvalidOperationException("This operator is not working propertly");

                if (SlidingPuzzleSolver.RIGHT.Equals(op))
                    if (puzzleClone.CanMoveRight(puzzleClone.GapPosition))
                        puzzleClone.MoveRight(puzzleClone.GapPosition);
                    else // No puede ocurrir que el operador aplicable no funcione, porque ya se comprobó que era aplicable
                        throw new InvalidOperationException("This operator is not working propertly");

                // Si el operador no se reconoce o es un NoOp, se devolverá la configuración actual (que sería idéntica a la original, no ha habido cambios)
                return puzzleClone; 
            }
        }
    }
}