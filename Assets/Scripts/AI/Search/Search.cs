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

    using System.Collections.Generic;
    using UCM.IAV.IA;

    /**
     * Realiza la b�squeda seg�n la estrategia con que se quiera implementar.
     * No se ha creado una clase gen�rica <S, O> para determinar el tipo de Setup y de Operator que se usa.
     */
    public interface Search {

        // Busca la soluci�n a un problema y devuelve la lista de operadores para llegar desde la configuraci�n actual a una configuraci�n objetivo
        List<Operator> Search(Problem p);

        // Devuelve la informaci�n de las m�tricas de la b�squeda
        Metrics GetMetrics();
    }
}