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

    using System;
    using System.Collections.Generic;

    /* 
     * El 'No Operador' (NoOp), operador especial que sirve para indicar fallo en la b�squeda.
     * Utiliza un patr�n Ejemplar �nico (Singleton) de dominio, sencillo de implementar y sin demasiadas restricciones.
     * Forma parte de la infraestructura necesaria para implementar la b�squeda de manera gen�rica y flexible.
     */
    public class NoOperator : BasicOperator {

        // El ejemplar �nico de NoOp 
        public static NoOperator Instance { get; private set; }

        private NoOperator() : base("NoOp") { }

        // Comienzo de los operadores
        public override bool isNoOperator() {
            return true;
        } 
    }
}