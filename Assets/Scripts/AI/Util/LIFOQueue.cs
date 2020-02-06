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
    using System.Linq;

    /**
     * Implementación de una 'cola' LIFO que también puede eliminar un elemento cualquiera.
     * Se basa en la clase estándar Stack<E>, aunque considerando la operación de eliminación es posible que exista alguna implementación más eficaz.
     */
    public class LIFOQueue<E> : Stack<E>, IQueue<E> {

        // Añade un elemento a la cola, según funcione esta
        public void Enqueue(E element) {
            Push(element); 
        }

        // Saca el primer elemento de la cola, según funcione esta
        public E Dequeue() {
            return Pop();
        }

        // Elimina el primer elemento de la cola que coincida con el que me pasan, devuelve cierto si lo ha encontrado y falso si no
        // Recorre la cola al completo, y cada elemento (salvo si es el que se quiere eliminar) lo vuelve a meter en la cola, dándole la vuelta a todo después, luego la complejidad es O(2*n)
        public bool Remove(E e) {

            // Desencola y vuelve a encolar Count veces
            bool removed = false;
            for (int count = 0; count < this.Count; count++) {
                E aux = Pop();
                // Si no he eliminado todavía nada, y el elemento es el que busco, no lo meto en la cola y marco removed
                if (!removed)
                    if (aux.Equals(e)) {
                        removed = true;
                        continue; // Que cosas más feas que hago, eh? :o)
                    }
                Push(aux);
            }

            // Se invierte todo (ya con el elemento eliminado -o no-) para que finalmente la pila esté en el orden original
            for (int count = 0; count < this.Count; count++) {
                E aux = Pop(); 
                Push(aux);
            }

            return removed; 
        }

        // Cadena de texto representativa  
        public override string ToString() {
            return "LIFOQueue[" + string.Join(",", this.Select(item => item.ToString())) + "]";
        }
    }
}
 