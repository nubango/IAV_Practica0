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

    /**
     * Representa a un problema al que se enfrenta un resolutor de IA, una parte fundamental del dominio de trabajo que contiene:
     * - La configuración inicial 
     * - La función que indica los operadores aplicables a una determinada configuración
     * - El modelo de transición, que dada una configuración y un operador nos proporciona la configuración resultante
     * - La prueba de objetivo, que determina si una determinada configuración es objetivo o no
     * - La función que indica el coste de cada paso de la solución (cambio desde una cierta configuración a otra, aplicando un determinado operador)
     * En lugar de ofrecer getters a todos estos componentes, podrían ofrecerse directamente métodos para interactuar con ellos...
     * Forma parte de la infraestructura necesaria para implementar la búsqueda de manera genérica y flexible.
     */
    public class Problem {

        // La configuración inicial
        public object InitialSetup { get; }

        // La función de los operadores aplicables
        public ApplicableOperatorsFunction ApplicableOperatorsFunction { get;  }

        // El modelo de transición
        public TransitionModel TransitionModel { get; }

        // La prueba de objetivo
        public GoalTest GoalTest { get; }

        // La función del coste de paso
        public StepCostFunction StepCostFunction { get; }

        // La configuración actual, con la que se trabaja internamente
        private object setup;

        // Crea un problema partiendo de todos sus componentes, menos la función de coste de paso donde se usará una por defecto 
        public Problem(object iSetup, ApplicableOperatorsFunction aoFunction, TransitionModel tModel, GoalTest gTest)
                : this(iSetup, aoFunction, tModel, gTest, new DefaultStepCostFunction()) { }

        // Crea un problema partiendo de todos sus componentes
        public Problem(object iSetup, ApplicableOperatorsFunction aoFunction, TransitionModel tModel, GoalTest gTest, StepCostFunction scFunction) {
            this.InitialSetup = iSetup;
            this.ApplicableOperatorsFunction = aoFunction;
            this.TransitionModel = tModel;
            this.GoalTest = gTest;
            this.StepCostFunction = scFunction;
        }

        // Cadena de texto representativa  
        public override string ToString() {
            // No muestro las funciones de operadores aplicables o coste de paso, ni el modelo de transición
            return "Problem(" + InitialSetup + " -> " + GoalTest + ")";          
        }
    }
}