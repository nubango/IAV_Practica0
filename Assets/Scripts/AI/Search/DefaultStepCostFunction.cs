/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com

    Basado en código original del repositorio de libro Artificial Intelligence: A Modern Approach
*/
namespace UCM.IAV.IA.Search { 

    using UCM.IAV.IA;

    /**
     * Asigna el coste por defecto a cada paso de la solución (un operador aplicado sobre una configuración que resulta en otra), que concretamente es 1 para todos los casos.
     * Forma parte de la infraestructura necesaria para implementar la búsqueda de manera genérica y flexible.
     */
    public class DefaultStepCostFunction : StepCostFunction {

        // Devuelve el coste del paso de la solución (configuración inicial, operador, configuración final) recibido en los parámetros
        public double GetCost(object initialSetup, Operator op, object resultSetup) {
            return 1.0d;
        }
    }
}