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

    /**
     * Representa a un problema al que se enfrenta un resolutor de IA, una parte fundamental del dominio de trabajo que contiene:
     * - La configuraci�n inicial 
     * - La funci�n que indica los operadores aplicables a una determinada configuraci�n
     * - El modelo de transici�n, que dada una configuraci�n y un operador nos proporciona la configuraci�n resultante
     * - La prueba de objetivo, que determina si una determinada configuraci�n es objetivo o no
     * - La funci�n que indica el coste de cada paso de la soluci�n (cambio desde una cierta configuraci�n a otra, aplicando un determinado operador)
     * En lugar de ofrecer getters a todos estos componentes, podr�an ofrecerse directamente m�todos para interactuar con ellos...
     * Forma parte de la infraestructura necesaria para implementar la b�squeda de manera gen�rica y flexible.
     */
    public class Problem {

        // La configuraci�n inicial
        public object InitialSetup { get; }

        // La funci�n de los operadores aplicables
        public ApplicableOperatorsFunction ApplicableOperatorsFunction { get;  }

        // El modelo de transici�n
        public TransitionModel TransitionModel { get; }

        // La prueba de objetivo
        public GoalTest GoalTest { get; }

        // La funci�n del coste de paso
        public StepCostFunction StepCostFunction { get; }

        // La configuraci�n actual, con la que se trabaja internamente
        private object setup;

        // Crea un problema partiendo de todos sus componentes, menos la funci�n de coste de paso donde se usar� una por defecto 
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
            // No muestro las funciones de operadores aplicables o coste de paso, ni el modelo de transici�n
            return "Problem(" + InitialSetup + " -> " + GoalTest + ")";          
        }
    }
}