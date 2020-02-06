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
    using UCM.IAV.IA.Search;

    /**
     * Indica si una configuraci�n cualquiera del puzle de bloques deslizantes es una configuraci�n objetivo, que coincide con la configuraci�n inicial del mismo. 
     */
    public class SlidingPuzzleGoalTest : GoalTest { 

        // Guarda una copia de un puzle como el que se est� resolviendo, pero en su configuraci�n objetivo, que coincide con la inicial
        private SlidingPuzzle goal;

        // Construye una prueba de objetivo de dimensiones (rows) por (columns) 
        // Como m�nimo las dimensiones tienen que ser de 1x1 
        public SlidingPuzzleGoalTest(uint rows, uint columns) {
            if (rows == 0) throw new ArgumentException(string.Format("{0} is not a valid rows value", rows), "rows");
            if (columns == 0) throw new ArgumentException(string.Format("{0} is not a valid columns value", columns), "columns");

            goal = new SlidingPuzzle(rows, columns); // Por defecto estar� en la configuraci�n objetivo, que es tambi�n la inicial
        }
        
        // Devuelve cierto o falso seg�n la configuraci�n que nos pasan sea o no objetivo
        public bool IsGoal(object setup) {
            if (setup == null || !(setup is SlidingPuzzle))
                return false;

            SlidingPuzzle puzzle = (SlidingPuzzle)setup;
            // Si no tienen las mismas dimensiones, el equals va a fallar
            return puzzle.Equals(goal);
        }

        // Cadena de texto representativa 
        public override string ToString() {
            return "Goal" + goal.ToString();
        }
    }
}