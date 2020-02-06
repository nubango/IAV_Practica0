/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en código original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.IA {

    /*
     * Un operador que puede ser aplicado por el resolutor para cambiar la configuración del problema.
     * Forma parte de la infraestructura necesaria para implementar la búsqueda de manera genérica y flexible.
     */
    public interface Operator {

        // Dice si se trata de un 'no operador' (NoOp), operador especial que sirve para indicar fallo en la búsqueda 
        bool isNoOperator(); 
    }
}