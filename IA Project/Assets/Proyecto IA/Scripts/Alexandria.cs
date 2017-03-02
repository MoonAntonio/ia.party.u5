//                                  ┌∩┐(◣_◢)┌∩┐
//																				\\
// Alexandria.cs (02/03/2017)													\\
// Autor: Antonio Mateo (Moon Pincho) 									        \\
// Descripcion:		Manager de todas las unidades								\\
// Fecha Mod:		02/03/2017													\\
// Ultima Mod:		Version Inicial												\\
//******************************************************************************\\

#region Librerias
using UnityEngine;
#endregion

namespace MoonPincho
{
    /// <summary>
    /// <para>Manager de todas las unidades</para>
    /// </summary>
	public class Alexandria : MonoBehaviour 
	{
        #region Variables
        /// <summary>
        /// <para>Unidades en escena de las particulas.</para>
        /// </summary>
        public GameObject[] particulas;                                         // Unidades en escena de las particulas
        /// <summary>
        /// <para>Prefab de las particulas.</para>
        /// </summary>
        public GameObject unitPrefab;                                           // Prefab de las particulas
        /// <summary>
        /// <para>Maximo de particulas que spawmea el manager.</para>
        /// </summary>
        public int maxParticulas = 10;                                          // Maximo de particulas que spawmea el manager
        /// <summary>
        /// <para>Rango donde spawmean las particulas.</para>
        /// </summary>
        public Vector3 rango = new Vector3(5, 5, 5);                            // Rango donde spawmean las particulas
        /// <summary>
        /// <para>Patron de particulas, se dirijen al punto central del objetivo.</para>
        /// </summary>
        public bool isObjetivo = true;                                          // Patron de particulas, se dirijen al punto central del objetivo
        /// <summary>
        /// <para>Patron de particulas, intentan ser lo mas precisas con su direccion principal.</para>
        /// </summary>
        public bool isObediente = true;                                         // Patron de particulas, intentan ser lo mas precisas con su direccion principal
        /// <summary>
        /// <para>Patron de particulas, intentan ser lo menos precisas con su direccion.</para>
        /// </summary>
        public bool isVacilon = false;                                          // Patron de particulas, intentan ser lo menos precisas con su direccion
        /// <summary>
        /// <para>Distancia minima entre particulas.</para>
        /// </summary>
        [Range(0, 200)]
        public int distanciaMinima = 50;                                        // Distancia minima entre particulas
        /// <summary>
        /// <para>Maxima fuerza ejercida a las particulas.</para>
        /// </summary>
        [Range(0, 2)]
        public float maxFuerza = 0.5f;                                          // Maxima fuerza ejercida a las particulas
        /// <summary>
        /// <para>Maxima velocidad a la que pueden llegar las particulas.</para>
        /// </summary>
        [Range(0, 5)]
        public float maxVelocidad = 2.0f;                                       // Maxima velocidad a la que pueden llegar las particulas
        #endregion

        #region Init
        /// <summary>
        /// <para>Iniciador de Alexandria.</para>
        /// </summary>
        private void Start()// Iniciador de Alexandria
        {
            particulas = new GameObject[maxParticulas];

            for (int i = 0; i < maxParticulas; i++)
            {
                Vector3 partPos = new Vector3(Random.Range(-rango.x, rango.x), Random.Range(-rango.y, rango.y), Random.Range(0, 0));

                particulas[i] = Instantiate(unitPrefab, this.transform.position + partPos, Quaternion.identity) as GameObject;
                particulas[i].GetComponent<Particula>().manager = this.gameObject;
            }
        }
        #endregion

        #region Dev
        /// <summary>
        /// <para>Gizmo.</para>
        /// </summary>
        private void OnDrawGizmosSelected()// Gizmo
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(this.transform.position, rango * 2);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(this.transform.position, 0.2f);
        }
        #endregion
    }
}
