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

    using System;
    using System.Collections.Generic;
    using UCM.IAV.IA;

    /**
     * Estructura de datos con la que se construye el espacio de b�squeda. 
     * Contiene una configuraci�n (SETUP), un padre (PARENT, porque estas referencias van de hijos a padres), 
     * el operador (OPERATOR) que fue aplicado al padre para expandirlo y generar este nodo y el coste de la ruta (la llamada funci�n g(n)) 
     * desde la configuraci�n inicial hasta la de este nodo, que puede ser recorrida siguiendo las referencias al padre.
     * Forma parte de la infraestructura necesaria para implementar la b�squeda de manera gen�rica y flexible.
     */
    public class Node : IEquatable<Node>, IComparable<Node>, IComparable { // Realmente no es imprescindible que sean comparables

        // La configuraci�n del problema a la que representa este nodo 
        public object Setup { get; }

        // El padre desde el que se gener� este nodo
        public Node Parent { get; }

        // El operador que fue aplicado al padre para expandirlo y generar este nodo 
        public Operator Operator { get; }

        // El coste de la ruta desde la configuraci�n inicial hasta la de este nodo
        public double PathCost { get; }

        // Construye un nodo en base exclusivamente a su configuraci�n
        // Puede utilizarse para crear un nodo ra�z, ya que el �nico dato que establece es que el coste de la ruta es 0
        public Node(object setup) {
            Setup = setup;
            PathCost = 0.0d;
        }

        // Construye un nodo hijo en base a su configuraci�n, padre, operador que sirvi� para dar el paso de generarlo y coste de dicho paso
        public Node(object setup, Node parent, Operator op, double stepCost) : this(setup) { // Se aprovecha parte del constructor anterior
            Parent = parent;
            Operator = op;
            PathCost = parent.PathCost + stepCost;
        }

        // Devuelve cierto si es un nodo ra�z
        public bool IsRoot() {
            return Parent == null;
        }

        // Devuelve la lista de nodos de la ruta completa a este nodo, empezando desde la ra�z e incluyendo finalmente a este
        public List<Node> GetPathFromRoot() {

            List<Node> path = new List<Node>();
            Node current = this;
            while (!current.IsRoot()) {
                path.Add(current);
                current = current.Parent;
            } 
            path.Add(current); // Se a�ade tambi�n el nodo ra�z

            // Hay que invertir el orden en que hemos metido los nodos (creo que es mejor esto que ir usando Insert(0) todo el rato)
            path.Reverse();
            return path; 
        }

        // Compara este nodo con otro y dice si sus configuraciones son iguales
        // �Ojo! No se tiene en cuenta si los costes o las rutas para llegar a ellos son distintas
        public bool Equals(Node n) {
            if (n == null) 
                return false;
            return Setup.Equals(n.Setup);
        }

        // Compara este nodo con otro objeto y dice si sus configuraciones son iguales 
        public override bool Equals(object obj) {
            if (obj.GetType() == typeof(Node))
                return Equals(obj as Node);
            return false;
        }

        // Devuelve c�digo hash del nodo (para optimizar el acceso en colecciones y as�)
        // No debe contener bucles, tiene que ser muy r�pida
        // �Ojo! No se tiene en cuenta si los costes o las rutas para llegar a ellos son distintas
        public override int GetHashCode() {
            return Setup.GetHashCode();
        }

        // Redefinici�n de los operadores de igualdad 
        public static bool operator ==(Node left, Node right) {
            if (ReferenceEquals(left, null))
                return ReferenceEquals(right, null);
            return left.Equals(right);
        }
        public static bool operator !=(Node left, Node right) {
            if (ReferenceEquals(left, null))
                return !ReferenceEquals(right, null);
            return !left.Equals(right);
        }

        // Compara este nodo con otro en coste, para ordenar
        // No se asume que la configuraci�n sea comparable, por eso no se recurre a ella para comparar
        public int CompareTo(Node other) {
            return PathCost.CompareTo(other.PathCost);
        }

        // Compara este nodo con otro objeto en coste, para ordenar (se implementa por compatibilidad con la antigua interfaz IComparable)
        int IComparable.CompareTo(object obj) {
            if (!(obj is Node))
                throw new ArgumentException("Argument is not a Node", "obj");
            Node other = (Node)obj;
            return CompareTo(other);
        }

        // Redefinici�n de los operadores relacionales en base a la comparaci�n en coste, para ordenar
        public static bool operator <(Node left, Node right) => left.CompareTo(right) < 0;
        public static bool operator >(Node left, Node right) => left.CompareTo(right) > 0;
        public static bool operator <=(Node left, Node right) => left.CompareTo(right) <= 0;
        public static bool operator >=(Node left, Node right) => left.CompareTo(right) >= 0;

        // Cadena de texto representativa (dibujar una matriz de posiciones separadas por espacios, con \n al final de cada columna o algo as�)
        public override string ToString() {
            // No muestro el padre ni el operador del que procede
            return "Node(" + Setup + " cost:" + PathCost + ")";      
        }
    }
}