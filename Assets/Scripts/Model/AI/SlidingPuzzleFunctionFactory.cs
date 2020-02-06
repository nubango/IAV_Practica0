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

    /*
     * Factor�a para obtener componentes del problema de los puzles de bloques deslizantes, evitando tener que crearlos cada vez que creemos un resolutor de dicho problema.
     * Ahora mismo permite obtener la funci�n que da los operadores aplicables y el modelo de transici�n.
     */
    public class SlidingPuzzleFunctionFactory {

        // La funci�n de operadores aplicables
        private static ApplicableOperatorsFunction applicableOperatorsFunction = null;
        // El modelo de transici�n
        private static TransitionModel transitionModel = null;

        // Devuelve la funci�n que da los operadores aplicables, cre�ndola por primera vez si no existiera
        public static ApplicableOperatorsFunction GetApplicableOperatorsFunction() {
            if (applicableOperatorsFunction == null) 
                applicableOperatorsFunction = new SlidingPuzzleApplicableOperatorsFunction();
            return applicableOperatorsFunction;
        }

        // Devuelve el modelo de transici�n, cre�ndolo por primera vez si no existiera
        public static TransitionModel GetTransitionModel() {
            if (transitionModel == null)
                transitionModel = new SlidingPuzzleResultFunction();
            return transitionModel;
        }

        // Clase interna privada que saben devolver el conjunto de operadores aplicables a una determinada configuraci�n del problema.
        // Los operadores se pueden leer como 'Puedo mover el hueco hacia arriba' o como 'Puedo mover la pieza de arriba del hueco'
        private class SlidingPuzzleApplicableOperatorsFunction : ApplicableOperatorsFunction {

            // Devuelve los operadores aplicables a una determinada configuraci�n del problema
            // Se devuelve HashSet porque es un conjunto sin orden, aunque tambi�n se podr�a haber decidido devolver una lista
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
         * Clase interna privada que permite obtener la configuraci�n resultante (o sucesora) tras aplicar un operador a una configuraci�n dada. 
         */
        private class SlidingPuzzleResultFunction : TransitionModel {

            // Dado una configuraci�n y un operador, devuelve la configuraci�n resultante
            public object GetResult(object setup, Operator op) {
                if (setup == null || !(setup is SlidingPuzzle)) throw new ArgumentException("Setup is not a valid SlidingPuzzle");

                // Lo que recibe es un SlidingPuzzle
                SlidingPuzzle puzzle = (SlidingPuzzle)setup;

                // Se clona el puzle deslizante a nivel profundo
                SlidingPuzzle puzzleClone = puzzle.DeepClone();

                if (SlidingPuzzleSolver.UP.Equals(op))
                    if (puzzleClone.CanMoveUp(puzzleClone.GapPosition))
                        puzzleClone.MoveUp(puzzleClone.GapPosition);
                    else // No puede ocurrir que el operador aplicable no funcione, porque ya se comprob� que era aplicable
                        throw new InvalidOperationException("This operator is not working propertly");

                if (SlidingPuzzleSolver.DOWN.Equals(op))
                    if (puzzleClone.CanMoveDown(puzzleClone.GapPosition))
                        puzzleClone.MoveDown(puzzleClone.GapPosition);
                    else // No puede ocurrir que el operador aplicable no funcione, porque ya se comprob� que era aplicable
                        throw new InvalidOperationException("This operator is not working propertly");

                if (SlidingPuzzleSolver.LEFT.Equals(op))
                    if (puzzleClone.CanMoveLeft(puzzleClone.GapPosition))
                        puzzleClone.MoveLeft(puzzleClone.GapPosition);
                    else // No puede ocurrir que el operador aplicable no funcione, porque ya se comprob� que era aplicable
                        throw new InvalidOperationException("This operator is not working propertly");

                if (SlidingPuzzleSolver.RIGHT.Equals(op))
                    if (puzzleClone.CanMoveRight(puzzleClone.GapPosition))
                        puzzleClone.MoveRight(puzzleClone.GapPosition);
                    else // No puede ocurrir que el operador aplicable no funcione, porque ya se comprob� que era aplicable
                        throw new InvalidOperationException("This operator is not working propertly");

                // Si el operador no se reconoce o es un NoOp, se devolver� la configuraci�n actual (que ser�a id�ntica a la original, no ha habido cambios)
                return puzzleClone; 
            }
        }
    }
}