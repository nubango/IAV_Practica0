/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en código original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.IA.Util {

    using System.Collections.Generic;

    /** 
     * Interfaz para las implementaciones de 'colas' (tanto colas propiamente dichas -FIFO-, como pilas -LIFO- como colas de prioridad) que también pueden eliminar un elemento cualquiera.
     * Permite ser enumerable y por lo tanto recorrida con un bucle foreach.
     */
    public interface IQueue<T> : IEnumerable<T> {

        // Cantidad de elementos de la cola
        int Count { get; }

        // Añade un elemento a la cola, según funcione esta
        void Enqueue(T item);

        // Saca el primer elemento de la cola, según funcione esta
        T Dequeue();

        // Elimina el primer elemento de la cola que coincida con el que me pasan, devuelve cierto si lo ha encontrado y falso si no
        bool Remove(T item);
    }
}
