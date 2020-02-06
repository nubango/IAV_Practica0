/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en c�digo original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.IA {

    /*
     * Un operador que puede ser aplicado por el resolutor para cambiar la configuraci�n del problema.
     * Forma parte de la infraestructura necesaria para implementar la b�squeda de manera gen�rica y flexible.
     */
    public interface Operator {

        // Dice si se trata de un 'no operador' (NoOp), operador especial que sirve para indicar fallo en la b�squeda 
        bool isNoOperator(); 
    }
}