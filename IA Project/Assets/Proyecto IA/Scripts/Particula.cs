//                                  ┌∩┐(◣_◢)┌∩┐
//																				\\
// Particula.cs (02/03/2017)													\\
// Autor: Antonio Mateo (Moon Pincho) 									        \\
// Descripcion:		Control general de la particula								\\
// Fecha Mod:		02/03/2017													\\
// Ultima Mod:		Version Inicial												\\
//******************************************************************************\\

#region Librerias
using UnityEngine;
#endregion

namespace MoonPincho
{
    /// <summary>
    /// <para>Control general de la particula</para>
    /// </summary>
	public class Particula : MonoBehaviour 
	{
        #region Variables
        /// <summary>
        /// <para>Manager del cual a sido spawmeado.</para>
        /// </summary>
        public GameObject manager;                          // Manager del cual a sido spawmeado
        /// <summary>
        /// <para>Posicion actual de la particula.</para>
        /// </summary>
        public Vector2 posicion = Vector2.zero;             // Posicion actual de la particula
        /// <summary>
        /// <para>Velocidad de la particula.</para>
        /// </summary>
        public Vector2 velocidad;                           // Velocidad de la particula
        /// <summary>
        /// <para>Objetivo de la particula.</para>
        /// </summary>
        private Vector2 objetivo = Vector2.zero;            // Objetivo de la particula
        /// <summary>
        /// <para>Fuerza actual de la particula.</para>
        /// </summary>
        private Vector2 actualFuerza;                       // Fuerza actual de la particula
        #endregion

        #region Init
        /// <summary>
        /// <para>Iniciador de Particula.</para>
        /// </summary>
        private void Start()// Iniciador de Particula
        {
            // Spawn
            velocidad = new Vector2(Random.Range(0.01f, 0.1f), Random.Range(0.01f, 0.1f));
            posicion = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        }
        #endregion

        #region Actualizador
        /// <summary>
        /// <para>Actualizador de Particula.</para>
        /// </summary>
        private void Update()// Actualizador de Particula
        {
            Comprobacion();
            objetivo = manager.transform.position;
        }
        #endregion

        #region Metodos
        /// <summary>
        /// <para>Comprobacion de la particula.</para>
        /// </summary>
        private void Comprobacion()// Comprobacion de la particula
        {
            posicion = this.transform.position;
            velocidad = this.GetComponent<Rigidbody2D>().velocity;

            // Si la particula tiene que ser obediente
            if (manager.GetComponent<Alexandria>().isObediente && Random.Range(0, 50) <= 1)
            {
                Vector2 ali = Alinear();
                Vector2 cho = Choque();
                Vector2 objetiv;

                // Si se tiene que ir al objetivo
                if (manager.GetComponent<Alexandria>().isObjetivo)
                {
                    objetiv = Buscar(objetivo);
                    actualFuerza = objetiv + ali + cho;
                }
                else
                {
                    actualFuerza = ali + cho;
                }
                    
                // Normalizar la fuerza
                actualFuerza = actualFuerza.normalized;
            }

            // Si la particula tiene que ser vacilona
            if (manager.GetComponent<Alexandria>().isVacilon && Random.Range(0, 50) <= 1)
            {
                if (Random.Range(0, 50) < 1)
                {
                    // Cambiar direccion
                    actualFuerza = new Vector2(Random.Range(0.01f, 0.1f), Random.Range(0.01f, 0.1f));
                }           
            }

            // Aplicamos la fuerza despues de la comprobacion
            AplicarFuerza(actualFuerza);
        }

        /// <summary>
        /// <para>Aplicamos la fuerza a la particula.</para>
        /// </summary>
        /// <param name="f">Posicion de la fuerza.</param>
        private void AplicarFuerza(Vector2 f)// Aplicamos la fuerza a la particula
        {
            Vector3 force = new Vector3(f.x, f.y, 0);

            // Normalizar la fuerza
            if (force.magnitude > manager.GetComponent<Alexandria>().maxFuerza)
            {
                force = force.normalized;
                force *= manager.GetComponent<Alexandria>().maxFuerza;
            }

            // Fijar la fuerza
            this.GetComponent<Rigidbody2D>().AddForce(force);

            // Normalizar la velocidad
            if (this.GetComponent<Rigidbody2D>().velocity.magnitude > manager.GetComponent<Alexandria>().maxVelocidad)
            {
                this.GetComponent<Rigidbody2D>().velocity = this.GetComponent<Rigidbody2D>().velocity.normalized;
                this.GetComponent<Rigidbody2D>().velocity *= manager.GetComponent<Alexandria>().maxVelocidad;
            }

            // Dev
            Debug.DrawRay(this.transform.position, force, Color.white);
        }
        #endregion

        #region Funcionalidad
        /// <summary>
        /// <para>Buscar la distancia hasta el objetivo.</para>
        /// </summary>
        /// <param name="objetivo">Posicion del objetivo.</param>
        /// <returns></returns>
        private Vector2 Buscar(Vector2 objetivo)// Buscar la distancia hasta el objetivo
        {
            return (objetivo - posicion);
        }

        /// <summary>
        /// <para>Alinear con las demas particulas.</para>
        /// </summary>
        /// <returns></returns>
        private Vector2 Alinear()// Alinear con las demas particulas
        {
            float distMin = manager.GetComponent<Alexandria>().distanciaMinima;
            Vector2 sum = Vector2.zero;
            int count = 0;

            foreach (GameObject part in manager.GetComponent<Alexandria>().particulas)
            {
                if (part == this.gameObject)
                {
                    continue;
                }

                float d = Vector2.Distance(posicion, part.GetComponent<Particula>().posicion);

                // Aumentar el count
                if (d < distMin)
                {
                    sum += part.GetComponent<Particula>().velocidad;
                    count++;
                }
            }
            // Devolver
            if (count > 0)
            {
                sum /= count;
                Vector2 dir = sum - velocidad;
                return dir;
            }

            return Vector2.zero;
        }

        /// <summary>
        /// <para>Cuando la particula choca con otra.</para>
        /// </summary>
        /// <returns></returns>
        private Vector2 Choque()// Cuando la particula choca con otra
        {
            float distMin = manager.GetComponent<Alexandria>().distanciaMinima;
            Vector2 sum = Vector2.zero;
            int count = 0;

            foreach (GameObject other in manager.GetComponent<Alexandria>().particulas)
            {
                if (other == this.gameObject)
                {
                    continue;
                }

                float d = Vector2.Distance(posicion, other.GetComponent<Particula>().posicion);

                // Aumentar el count
                if (d < distMin)
                {
                    sum += other.GetComponent<Particula>().posicion;
                    count++;
                }
            }

            // Devolver
            if (count > 0)
            {
                sum /= count;
                return Buscar(sum);
            }

            return Vector2.zero;
        }
        #endregion
    }
}
