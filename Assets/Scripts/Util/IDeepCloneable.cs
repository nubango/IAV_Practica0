/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Util {

    /* 
     * Interfaz que implementan los objetos que son clonables a nivel profundo (copia profunda).
     * Esto lo usamos porque ICloneable es un interfaz de C# cuya sintaxis no está bien definida (no deja claro si la copia debe ser superficial o profunda)
     */
    public interface IDeepCloneable {

        // Devuelve este objeto clonado a nivel profundo
        object DeepClone();
    }

    /* 
     * Interfaz genérico que implementan los objetos de tipo T que son clonables a nivel profundo (copia profunda).
     * Esto lo usamos porque ICloneable es un interfaz de C# cuya sintaxis no está bien definida (no deja claro si la copia debe ser superficial o profunda)
     */
    public interface IDeepCloneable<T> : IDeepCloneable {

        // Devuelve este objeto de tipo T clonado a nivel profundo
        // Oculta el método del interfaz anterior al que extiende
        new T DeepClone();
    }
}


