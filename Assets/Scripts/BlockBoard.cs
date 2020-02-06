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
    using System.Collections;
    using UnityEngine;
    using Model;
    using System.Collections.Generic;

    /* 
     * Tablero de bloques que sabe representar una matriz de bloques móviles y moverlos entre sus distintas posiciones.
     * Es un componente diseñado para Unity 2018.2, se asume que se encarga de propiedades como su tamaño y el de los bloques.
     */
    public class BlockBoard : MonoBehaviour {

        // Constantes
        public static readonly float USER_DELAY = 0.0f;
        public static readonly float AI_DELAY = 0.2f;

        private static readonly float POSITION_FACTOR_R = 1.1f;
        private static readonly float POSITION_FACTOR_C = 1.1f;
        private static readonly float SCALE_FACTOR_R = 1.1f;
        private static readonly float SCALE_FACTOR_C = 1.1f;
        
        // Aquí es donde necesito el tipo de prefab (sólo de lectura, mejor)
        public MovableBlock blockPrefab;

        // El gestor del puzle deslizante al que obedece
        private SlidingPuzzleManager manager;

        // La matriz de bloques 
        // Podría ser de tipo Matrix, como el modelo, pero no tiene por qué compartir implementación
        // Podría guardar la información de donde está el bloque invisible o SpecialValue, aunque no es necesario
        private MovableBlock[,] blocks;

        // Centinela para indicar si se está en la corrutina de mover bloques
        private bool blockInMotion = false;
        // Lista de bloques para ir moviendo (ojo, van de dos en dos... porque en C# 6 no hay tuplas todavía)
        private Queue<MovableBlock> blocksInMotion = new Queue<MovableBlock>(); 

        // Inicializa con el manager y el puzle, sólo si todavía no tiene bloques generados; si no, actúa como un reinicio (destruye los que hay y genera nuevos)
        // Ni el manager ni el puzle recibidos pueden ser nulo
        // A partir de C# 7 (en Unity 2018.3) es posible marcar parámetros de entrada con "in"
        public void Initialize(SlidingPuzzleManager manager, SlidingPuzzle puzzle) {
            if (manager == null) throw new ArgumentNullException(nameof(manager));
            if (puzzle == null) throw new ArgumentNullException(nameof(puzzle));

            this.manager = manager;

            // Si no hay bloques, se tienen que crear, y si los hay pero ha cambiado el tamaño, también (sólo que antes hay que destruir los bloques viejos)
            // Podrían aprovecharse parcialmente o totalmente los bloques viejos, según la variación de las dimensiones, aunque ahora mismo no se hace
            if (blocks == null) {
                blocks = new MovableBlock[puzzle.Rows, puzzle.Columns];

                // Ajustar las dimensiones del tablero de bloques
                // La cámara podría también ajustarse si fuese hija y dependiera de alguna manera del tablero de bloques  
                transform.localScale = new Vector3(SCALE_FACTOR_C * blocks.GetLength(1), transform.localScale.y, SCALE_FACTOR_R * blocks.GetLength(0));
                /* Valores INICIALES de X e Y 
                  float x = (columns / 2.0f) * 1.1f - 0.55f; //Tanto el 1.1f como el 0.5 debería ser preguntarle al Box por su tamaño horizontal (y o bien sumarle ese margen de 0.1 o coger la mitad) 
                  float y = (rows / 2.0f) * 1.1f - 0.55f;  //0.5 debería ser preguntarle al Box por su tamaño vertical (y coger la mitad) 
                */

            } else if (blocks.GetLength(0) != puzzle.Rows || blocks.GetLength(1) != puzzle.Columns) {
                DestroyBlocks();
                blocks = new MovableBlock[puzzle.Rows, puzzle.Columns];

                // Ajustar las dimensiones del tablero de bloques
                // La cámara podría también ajustarse si fuese hija y dependiera de alguna manera del tablero de bloques  
                transform.localScale = new Vector3(SCALE_FACTOR_C * blocks.GetLength(1), transform.localScale.y, SCALE_FACTOR_R * blocks.GetLength(0));
                /* Valores INICIALES de X e Y 
                  float x = (columns / 2.0f) * 1.1f - 0.55f; //Tanto el 1.1f como el 0.5 debería ser preguntarle al Box por su tamaño horizontal (y o bien sumarle ese margen de 0.1 o coger la mitad) 
                  float y = (rows / 2.0f) * 1.1f - 0.55f;  //0.5 debería ser preguntarle al Box por su tamaño vertical (y coger la mitad) 
                */
            }

            GenerateBlocks(puzzle);
        }

        // Destruye todos los bloques de la matriz
        // No se utiliza GetLowerBound ni GetUpperBound porque sabemos que son matrices que siempre empiezan en cero y acaban en positivo
        private void DestroyBlocks() {
            if (blocks == null) throw new InvalidOperationException("This object has not been initialized");

            var rows = blocks.GetLength(0);
            var columns = blocks.GetLength(1);

            for (var r = 0u; r < rows; r++) {
                for (var c = 0u; c < columns; c++) {
                    if (blocks[r, c] != null)
                        Destroy(blocks[r, c].gameObject);
                }
            }
        }

        // Genera todos los bloques de la matriz
        // No se utiliza GetLowerBound ni GetUpperBound porque sabemos que son matrices que siempre empiezan en cero y acaban en positivo
        private void GenerateBlocks(SlidingPuzzle puzzle) {
            if (puzzle == null) throw new ArgumentNullException(nameof(puzzle));

            var rows = blocks.GetLength(0);
            var columns = blocks.GetLength(1);

            for (var r = 0u; r < rows; r++) {
                for (var c = 0u; c < columns; c++) {
                    MovableBlock block = blocks[r, c];
                    if (block == null) {
                        // Crea un ejemplar del prefab de bloque en la posición correcta según su posición en la matriz (podrían hacerse hijos del tablero)
                        // Se asume un tablero en (0f, 0f, 0f) aunque debería tomarse la posición real del tablero como referencia
                        block = Instantiate(blockPrefab,
                            new Vector3(-((blocks.GetLength(1) / 2.0f) * POSITION_FACTOR_C - (POSITION_FACTOR_C / 2.0f)) + c * POSITION_FACTOR_C,
                                         0,
                                         (blocks.GetLength(0) / 2.0f) * POSITION_FACTOR_R - (POSITION_FACTOR_R / 2.0f) - r * POSITION_FACTOR_R),
                            Quaternion.identity); // En Y, que es la separación del board, estoy poniendo 0 pero la referencia la debería dar el Board

                        blocks[r, c] = block;
                    }
                    // Estuviera o no ya creado el bloque, se inicializa y reposiciona
                    Position position = new Position(r, c);
                    block.position = position;

                    uint value = puzzle.GetValue(position);
                    // El texto que se pone en el bloque es el valor +1, salvo si es el último valor, que no se mandará texto para que sea un bloque no visible
                    if (value == 0)
                        block.Initialize(this, null);
                    else
                        block.Initialize(this, value.ToString());
                    Debug.Log(ToString() + "generated " + block.ToString() + ".");
                }
            }

        }

        // Devuelve si se puede mover un bloque en el tablero
        public bool CanMove(MovableBlock block) {
            if (block == null) throw new ArgumentNullException(nameof(block));

            return manager.CanMove(block);
        }

        // Mueve un bloque en el tablero, devolviendo el otro bloque que ahora pasa a ocupar el lugar de este
        // Si no se puede realizar el movimiento, da fallo
        public MovableBlock Move(MovableBlock block, float delay) {
            if (block == null) throw new ArgumentNullException(nameof(block));
            if (!CanMove(block)) throw new InvalidOperationException("The required movement is not possible");

            Debug.Log(ToString() + " moves " + block.ToString() + ".");

            // Intercambio de valores entre las dos posiciones de la matriz de bloques?
            MovableBlock otherBlock = manager.Move(block);
            // Ya ha cambiado el puzle, y mi posición lógica (y la del hueco -otherBlock-)... faltan las posiciones físicas en la escena y ubicaciones en la matriz de bloques de ambos
               
            // Cambio la ubicación en la matriz del bloque auxiliar para que sea la correcta
            blocks[otherBlock.position.GetRow(), otherBlock.position.GetColumn()] = otherBlock;
            // Cambio la ubicación en la matriz del bloque resultante para que sea la correcta
            blocks[block.position.GetRow(), block.position.GetColumn()] = block;

            // Los meto en la lista para que se muevan cuando corresponda...
            blocksInMotion.Enqueue(block);
            blocksInMotion.Enqueue(otherBlock);
            // Debería meter el delay o marcar de alguna manera si el movimiento es de humano o máquina

            return otherBlock;
        }

        // Método para actualizar frame a frame el movimiento de los bloques
        private void Update() {
            if (!blockInMotion && blocksInMotion.Count > 0)  // Only start the coroutine if loading is false, meaning it hasn't already been started
                // Se podría mostrar una animación de movimiento incluso, ya que las únicas restricciones de movimiento son las del modelo
                StartCoroutine(BlockInMotion(USER_DELAY));
        }

        // Corrutina para pausar entre movimientos y poder verlos
        IEnumerator BlockInMotion(float delay) {
            blockInMotion = true;
            // Animar tal vez las dos piezas...
            yield return new WaitForSeconds(delay);

            MovableBlock block = blocksInMotion.Dequeue();
            MovableBlock otherBlock = blocksInMotion.Dequeue();
            // Ya ha cambiando el puzle, la posición lógica y ubicación en la matriz de bloques de ambos bloques
            // Sólo queda intercambiar la parte visual, la posición física de ambos en la escena 
            block.ExchangeTransform(otherBlock);

            blockInMotion = false;
        }





        // Pone los contadores de información a cero
        public void UserInteraction() {
            manager.CleanInfoAndMetrics();
        }

        // Devuelve un bloque situado en la matriz de bloques
        // Si no uso Position habría menos seguridad en el acceso (pero podría trabajar con una tupla de dos parámetros también como permite la sintaxis de C# 7)
        public MovableBlock GetBlock(Position position) {
                if (position == null) throw new ArgumentNullException(nameof(position));

                return blocks[position.GetRow(), position.GetColumn()];
        }

        // Cadena de texto representativa
        public override string ToString() {
            return "Board{" + blocks.ToString() + "}";
        }
    }
}
