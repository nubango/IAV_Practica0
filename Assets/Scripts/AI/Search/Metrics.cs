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

    using System.Linq;
    using System.Collections.Generic;

    /* 
     * Almac�n con las distintas m�tricas que pueden ir generando los algoritmos de b�squeda.
     * Forma parte de la infraestructura necesaria para implementar la b�squeda de manera gen�rica y flexible.
     */
    public class Metrics {

        // Internamente se implementa como un diccionario con parejas de cadenas de texto
        // Es muy vers�til as� aunque apenas aporta valor; seguramente se podr�a redise�ar con enumerados y valores tipo int o double dependiendo del caso o algo as�
        private Dictionary<string, string> dict;

        // Crea un almac�n de m�tricas vac�o
        public Metrics() {
            dict = new Dictionary<string, string>();
        }

        // Guarda una medida con su nombre y su valor entero
        public void Set(string name, int i) {
            dict[name] = i.ToString();
        }

        // Guarda una medida con su nombre y su valor real
        public void Set(string name, double d) {
            dict[name] = d.ToString("0.0"); // Lo guardamos s�lo con un decimal
        }

        // Recupera una medida con valor entero a partir de su nombre
        public int GetInt(string name) {
            return int.Parse(dict[name]);
        }

        // Recupera una medida con valor real a partir de su nombre
        public double GetDouble(string name) {
            return double.Parse(dict[name]);
        }

        // Recupera una medida con valor tipo cadena de texto a partir de su nombre (por si hubiese alg�n caso as�)
        public string Get(string name) {
            return dict[name];
        }
        
        // Cadena de texto representativa  
        public override string ToString() {
            return string.Join("   ", dict.Select(item => item.Key + ": " + item.Value)); 
        }
    }
}
 