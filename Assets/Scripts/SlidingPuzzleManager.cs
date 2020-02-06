/*    
    Copyright (C) 2019 Federico Peinado
    http://www.federicopeinado.com

    Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
    Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

    Autor: Federico Peinado 
    Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Puzzles {
    
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using Model; 
    using Model.AI;
    using UCM.IAV.IA;
    using UCM.IAV.IA.Search;

    /* 
     * Gestor de la escena del puzle de bloques deslizantes que actúa como controlador entre la vista (objetos de la escena de Unity) y el modelo (lógica del puzle deslizante).
     * Normalmente este gestor seguiría el patrón Singleton y vendría en forma de prefab para ser llamado por un objeto Loader (incluso para leer dimensiones del puzle de fichero), 
     * aunque en este caso no es necesario: https://unity3d.com/es/learn/tutorials/projects/2d-roguelike-tutorial/writing-game-manager
     * Es un componente diseñado para Unity 2018.2.
    */
    public class SlidingPuzzleManager : MonoBehaviour {

        // Número de movimientos aleatorios que se harán en el puzle cuando se utilice Random
        // Sería interesante mejorar esto para que no siempre sean el mismo número de movimientos, sino que entren en un rango dependiente del tamaño del puzle también
        private static readonly uint RANDOM_MOVES = 50;

        // El tablero de bloques (suele ser un prefab)
        public BlockBoard board;
                     
        // Los paneles de información general y métricas del HUD
        public GameObject infoPanel;
        public GameObject metricsPanel;

        // Los textos de tiempo, pasos y métricas del HUD
        public Text timeNumber;
        public Text stepsNumber;
        public Text metricsString;

        // El texto introducido en los campos de entrada de filas y columnas
        public InputField rowsInput;
        public InputField columnsInput;

        // Dimensiones iniciales del puzle (3x3 en caso de que el diseñador no especifique nada en el Inspector)
        public uint rows = 3;
        public uint columns = 3;

        // El modelo interno del puzle deslizante 
        private SlidingPuzzle puzzle;

        // El resolutor del puzle (que admite varias estrategias de resolución)
        private SlidingPuzzleSolver solver;

        // Tiempo, memoria, pasos de la solución y métricas resultantes acumuladas en la búsqueda
        private double time = 0.0d; // en segundos
        private int memory = 0; // en kibibytes (KiB) tal vez (véase SystemInfo.systemMemorySize)
        private uint steps = 0;
        private string metrics = "";

        // Generador de números aleatorios del sistema (podría usar el de Unity, también)
        private System.Random random;

        // Se llama antes de dibujar el primero frame y es donde se puede pasar información entre los distintos objetos que se han despertado
        void Start() {
            // Podría lanzar excepciones si el objeto no ha sido inicializado con gameobjects en todos sus campos clave (salvo que pueda cargar info de fichero o algo así...)

            // Se inicializa la semilla del generador de números aleatorios
            random = new System.Random();

            Initialize(rows, columns);
        }

        // Inicializa o reinicia el gestor de la escena del puzle de bloques deslizantes
        private void Initialize(uint rows, uint columns) {            
            // Lanza excepciones si el objeto no ha sido inicializado con gameobjects en todos sus campos clave 
            if (board == null) throw new InvalidOperationException("The board reference is null");
            if (infoPanel == null) throw new InvalidOperationException("The infoPanel reference is null");
            if (metricsPanel == null) throw new InvalidOperationException("The metricsPanel reference is null");
            if (timeNumber == null) throw new InvalidOperationException("The timeNumber reference is null");
            if (stepsNumber == null) throw new InvalidOperationException("The stepsNumber reference is null");
            if (rowsInput == null) throw new InvalidOperationException("The rowsInputText reference is null");
            if (columnsInput == null) throw new InvalidOperationException("The columnsInputText reference is null");

            this.rows = rows;
            this.columns = columns;
            rowsInput.text = rows.ToString();
            columnsInput.text = columns.ToString();

            // Se crea el puzle internamente 
            puzzle = new SlidingPuzzle(rows, columns);

            // Inicializar todo el tablero de bloques
            board.Initialize(this, puzzle);

            CleanInfoAndMetrics();

            // Podríamos asumir que tras cada inicialización o reinicio, el puzle está ordenado y se puede mostrar todo el panel de información
            UpdateHUD();

        }

        // Pone los contadores de información y las métricas a cero
        public void CleanInfoAndMetrics() {
            time = 0.0d;
            memory = 0;
            steps = 0;
            metrics = "";
        }

        // Devuelve cierto si un bloque se puede mover, si se lo permite el gestor
        public bool CanMove(MovableBlock block) {
            if (block == null) throw new ArgumentNullException(nameof(block));

            return puzzle.CanMoveByDefault(block.position);
        }

        // Mueve un bloque, según las normas que diga el gestor
        public MovableBlock Move(MovableBlock block) {
            if (block == null) throw new ArgumentNullException(nameof(block));
            if (!CanMove(block)) throw new InvalidOperationException("The required movement is not possible");

            Position originPosition = block.position;

            Debug.Log(ToString() + " moves " + block.ToString() + "."); 
            var targetPosition = puzzle.MoveByDefault(block.position);
            // Si hemos tenido éxito ha cambiado la matriz lógica del puzle... pero no ha cambiado la posición (lógica), ni la mía ni la del hueco. Toca hacerlo ahora
            block.position = targetPosition;
            MovableBlock targetBlock = board.GetBlock(targetPosition);
            targetBlock.position = originPosition;

            UpdateHUD();

            return targetBlock;
        }

        // Actualiza la información del panel, mostrándolo si corresponde
        private void UpdateHUD() {

            // Según el puzle esté en orden o no, enseño el panel de información o no
            if (puzzle.IsInDefaultOrder()) {
                timeNumber.text = (time * 1000).ToString("0.0"); // Lo enseñamos en milisegundos y sólo con un decimal
                stepsNumber.text = steps.ToString();
                metricsString.text = metrics;
                infoPanel.gameObject.SetActive(true);
                metricsPanel.gameObject.SetActive(true);
            } else {
                infoPanel.gameObject.SetActive(false);
                metricsPanel.gameObject.SetActive(false);
            }
        }

        // Reinicia el puzle entero, recreándolo con las nuevas dimensiones si han cambiado 
        public void ResetPuzzle() {

            // Tampoco debería hacer nada si han fallado las conversiones a uint...
            if (rowsInput.text != null && columnsInput.text != null) {
                uint newRows = Convert.ToUInt32(rowsInput.text);
                uint newColumns = Convert.ToUInt32(columnsInput.text);

                if (newRows > 0u && newColumns > 0u) { // Al menos 1x1 debe ser
                    // Si el usuario no ha cambiado las dimensiones y está todo ordenado, no necesito resetearlo
                    if (newRows != rows || newColumns != columns || !puzzle.IsInDefaultOrder())
                        Initialize(newRows, newColumns);
                    else {
                        CleanInfoAndMetrics();
                        UpdateHUD();
                    }
                }
            }
        }

        // Recoloca aleatoriamente las piezas del puzle, haciendo RANDOM_MOVES movimientos, simulando -hasta que funcione- el tocar una posición aleatoria "a ciegas" y sin tener en cuenta las dimensiones del puzle
        // Lo razonable sería consultar al puzle para ver donde está el hueco y hacer intentos aleatorios pero siempre alrededor del hueco, en sus bloques colindantes, al menos
        public void RandomPuzzle() {

            // Tampoco debería hacer nada si han fallado las conversiones a uint...
            if (rowsInput.text != null && columnsInput.text != null) {
                uint newRows = Convert.ToUInt32(rowsInput.text);
                uint newColumns = Convert.ToUInt32(columnsInput.text);

                if (newRows > 0u && newColumns > 0u) { // Al menos 1x1 debe ser

                    if (newRows != rows || newColumns != columns)
                        Initialize(newRows, newColumns);
                    else {
                        time = 0.0f;
                        steps = 0;
                        UpdateHUD();
                    }

                    if (newRows > 1u || newColumns > 1u) { // Sólo se pueden recolocar aleatoriamente las piezas de puzles que admitan cierta interacción, claro
                        var randomMoves = RANDOM_MOVES;

                        while (randomMoves > 0) {
                            // La forma correcta de generar un uint aleatorio es complicada: return (uint)(rand.Next(1 << 30)) << 2 | (uint)(rand.Next(1 << 2));
                            // Asumo que serán números pequeños y hago la conversión
                            var randomRow = (uint)random.Next(0, Convert.ToInt32(rows));
                            var randomColumn = (uint)random.Next(0, Convert.ToInt32(columns));

                            var block = board.GetBlock(new Position(randomRow, randomColumn));
                            if (block.OnMouseUpAsButton())
                                randomMoves--;
                        }
                    }
                }
            }
        }

        // Muestra la solución obtenida paso a paso y con una pequeña animación o pausa
        private void ShowSolution(List<Operator> operators) { 

            if (operators.Count == 1 && operators[0].isNoOperator())
                steps = 0;
            else {
                foreach (Operator op in operators) { // Pausar tras cada movimiento realizado
                    Position position = solver.GetOperatedPosition(puzzle, op);
                    board.Move(board.GetBlock(position), BlockBoard.AI_DELAY);
                    Debug.Log("Applying " + op.ToString() + " operator");
                }

                steps = Convert.ToUInt32(operators.Count);
            }

            metricsString.text = metrics.ToString();
            UpdateHUD();

            // Mostrar tanto el resultado como las métricas... se podría mostrar paso a paso y en otro color, para diferenciar cómo queda todo al terminar 
        }


        // Se crea el resolutor con la estrategia BFS y se resuelve el problema
        public void SolvePuzzleByBFS() {

            // Medir la memoria: https://answers.unity.com/questions/506736/measure-cpu-and-memory-load-in-code.html

            // Si está ordenado, podría optar por no hacer nada... aunque llamaré igualmente y espero que devuelva la solución vacía 
            solver = new SlidingPuzzleSolver(rows, columns);
            // El resolutor ya está construido porque no requiere nada

            time = Time.realtimeSinceStartup;
            memory = SystemInfo.systemMemorySize; // Se podría sumar a la parte gráfica para tener la ocupación real en memoria (SystemInfo.graphicsMemorySize)
            List<Operator> operators = solver.Solve(puzzle, SlidingPuzzleSolver.Strategy.BFS);
            time = Time.realtimeSinceStartup - time;
            memory = SystemInfo.systemMemorySize - memory; // No tiene mucho sentido hacerlo aquí, porque ya se habrá liberado la mmemoria, habría que escoger el punto álgido (máximo de memoria utilizada)
            metrics = solver.GetMetrics().ToString();
            ShowSolution(operators);
        }

        // Se crea el resolutor con la estrategia DFS y se resuelve el problema
        public void SolvePuzzleByDFS() {

            // Si está ordenado, podría optar por no hacer nada... aunque llamaré igualmente y espero que devuelva la solución vacía 
            solver = new SlidingPuzzleSolver(rows, columns);
            // El resolutor ya está construido porque no requiere nada

            time = Time.realtimeSinceStartup;
            memory = SystemInfo.systemMemorySize; // Se podría sumar a la parte gráfica para tener la ocupación real en memoria (SystemInfo.graphicsMemorySize)
            List<Operator> operators = solver.Solve(puzzle, SlidingPuzzleSolver.Strategy.DFS);
            time = Time.realtimeSinceStartup - time;
            memory = SystemInfo.systemMemorySize - memory; // No tiene mucho sentido hacerlo aquí, porque ya se habrá liberado la mmemoria, habría que escoger el punto álgido (máximo de memoria utilizada)
            metrics = solver.GetMetrics().ToString();
            ShowSolution(operators);
        }

        // Salir de la aplicación
        public void Quit() {
            Application.Quit();
        }

        // Cadena de texto representativa
        public override string ToString() {
            return "Manager(" + board.ToString() + " <-> " + puzzle.ToString() + ")";
        }
    }
}

    
