/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Puzzles.Model {

    using System;

    /*
     * Posición (row, column) de una matriz bidimensional compuesta por (rows) filas y (columns) columnas. 
     * Las coordenadas de una posición son enteros sin signo, comprendidos entre 0 y UInt32.MaxValue - 1.
     * En C# 7 esto tal vez podría implementarse con una tupla.
     */
    public class Position {

        // Coordenadas fila y columna (podrían ser de sólo lectura)
        private uint row;
        private uint column;

        // Construye la posición de coordenadas (0u ,0u) por defecto
        public Position() : this(0u, 0u) {
        }

        // Construye la posición de coordenadas (row, column)
        // Ni la fila ni la columna pueden ser UInt32.MaxValue.
        public Position(uint row, uint column) {

            if (row == UInt32.MaxValue) throw new ArgumentException(string.Format("{0} is not a valid row", row), "row");
            if (column == UInt32.MaxValue) throw new ArgumentException(string.Format("{0} is not a valid column", column), "column");

            this.row = row;
            this.column = column;
        }

        // Devuelve la fila de esta posición
        public uint GetRow() {
            return row;
        }

        // Devuelve la columna de esta posición
        public uint GetColumn() {
            return column;
        }

        /* Para qué querrías hacer esto?

        // Intercambia las coordenadas con otra posición
        public void Exchange(Position position) {
            if (position == null) throw new ArgumentNullException(nameof(position));

            // Se usan variables auxiliares para hacer el intercambio
            uint auxRow = position.row;
            uint auxColumn = position.column;

            position.row = row;
            position.column = column;

            row = auxRow;
            column = auxColumn;
        }
        */


            // No parece buena idea modificar mucho las posiciones, es mejor obtener otras nuevas aunque suponga crear más objetos

        // Devuelve cierto si esta posición está arriba de la posición 'position'
        public bool IsUp(Position position) {
            if (position == null) throw new ArgumentNullException(nameof(position));

            return column == position.column && row + 1u == position.row;
        }

        /*
        // Cambia esta posición hacia arriba, fallando si no es posible hacerlo 
        public void GoUp() {
            if (row == 0u) throw new InvalidOperationException("The required position is not accessible");

            row--;
        }*/

        // Devuelve la posición de arriba de esta posición, fallando si no es posible darla 
        public Position Up() {
            if (row == 0u) throw new InvalidOperationException("The required position is not accessible");

            return new Position(row - 1u, column); 
        }

        // Devuelve cierto si esta posición está abajo de la posición 'position'
        public bool IsDown(Position position) {
            if (position == null) throw new ArgumentNullException(nameof(position));

            return column == position.column && position.row + 1u == row;
        }

        /*
        // Cambia esta posición hacia abajo, fallando si no es posible hacerlo 
        public void GoDown() {
            if (row + 1u == UInt32.MaxValue) throw new InvalidOperationException("The required position is not accessible");

            row++;
        }*/

        // Devuelve la posición de abajo de esta posición, fallando si no es posible darla 
        public Position Down() {
            if (row + 1u == UInt32.MaxValue) throw new InvalidOperationException("The required position is not accessible");

            return new Position(row + 1u, column);
        }

        // Devuelve cierto si esta posición está a la izquierda de la posición 'position'
        public bool IsLeft(Position position) {
            if (position == null) throw new ArgumentNullException(nameof(position));

            return row == position.row && column + 1u == position.column;
        }

        /*
        // Cambia esta posición hacia la izquierda, fallando si no es posible hacerlo 
        public void GoLeft() {
            if (column == 0u) throw new InvalidOperationException("The required position is not accessible");

            column--;
        }*/

        // Devuelve la posición hacia la izquierda de esta posición, fallando si no es posible darla 
        public Position Left() {
            if (column == 0u) throw new InvalidOperationException("The required position is not accessible");

            return new Position(row, column - 1u);
        }

        // Devuelve cierto si esta posición está a la derecha de la posición 'position'
        public bool IsRight(Position position) {
            if (position == null) throw new ArgumentNullException(nameof(position));

            return row == position.row && position.column + 1u == column;
        }

        /*
        // Cambia esta posición hacia la derecha, fallando si no es posible hacerlo 
        public void GoRight() {
            if (column + 1u == UInt32.MaxValue) throw new InvalidOperationException("The required position is not accessible");

            column++;
        } */

        // Devuelve la posición hacia la derecha de esta posición, fallando si no es posible darla 
        public Position Right() {
            if (column + 1u == UInt32.MaxValue) throw new InvalidOperationException("The required position is not accessible");

            return new Position(row, column + 1u);
        }

        // Compara esta posición con otro objeto y dice si son iguales
        // Sobreescribe la función Equals de Object
        public override bool Equals(object o) {
            return Equals(o as Position);
        }

        // Compara esta posición con otra y dice si coordenadas
        public bool Equals(Position p) {
            if (p == null || p.row != row || p.column != column) {
                return false;
            }
            return true;
        }

        // Devuelve código hash de la posición (para optimizar el acceso en colecciones y así)
        // He puesto una función cualquiera, no especialmente óptima
        public override int GetHashCode() {
            return Convert.ToInt32(row * column);
        }

        // Cadena de texto representativa
        public override string ToString() {
            return "(" + row + ", " + column + ")";
        }
    }
}
