using Controllers;
using Interactions.Enums;
using Player;

namespace Interactions.Interfaces
{
    public interface IInteractive
    {
        /// <summary>
        /// Metodo utilizado para iniciar una interaccion entre dos personajes. En este caso seria de un NPC/Enemigo al Player.
        /// </summary>
        /// <param name="controller">El controlador del que inicia la interaccion.</param>
        /// <param name="interactionKind">El tipo de interaccion requerida.</param>
        /// <returns>Retorna true si la interaccion ha sido exitosa, de lo contrario si no es posible iniciar la interaccion o ha ocurrido un error, retorna false.</returns>
        bool Interact(BaseController controller, EInteractionKind interactionKind);
        /// <summary>
        /// Metodo utilizado para iniciar una interaccion entre dos personajes. En este caso seria del Player a un NPC/Enemigo.
        /// </summary>
        /// <param name="controller">El controlador del que inicia la interaccion (Player).</param>
        /// <param name="interactionKind">El tipo de interaccion requerida.</param>
        /// <returns>Retorna true si la interaccion ha sido exitosa, de lo contrario si no es posible iniciar la interaccion o ha ocurrido un error, retorna false.</returns>
        bool Interact(PlayerController controller, EInteractionKind interactionKind);

        /// <summary>
        /// Comunica al Player que se esta listo para iniciar una interaccion.
        /// Este metodo es el que inicia la animacion de la burbuja de dialogo o de interaccion !!!
        /// </summary>
        /// <param name="controller">El controlador del que inicia la interaccion (Player).</param>
        /// <param name="isReady">Si se esta listo para interactuar.</param>
        void ReadyToInteract(PlayerController controller, bool isReady);        
        
        /// <summary>
        /// Comunica al NPC/Enemigo que se esta listo para iniciar una interaccion.
        /// Este metodo es el que inicia la animacion de la burbuja de dialogo o de interaccion !!!
        /// </summary>
        /// <param name="controller">El controlador del que inicia la interaccion.</param>
        /// <param name="isReady">Si se esta listo para interactuar.</param>
        void ReadyToInteract(BaseController controller, bool isReady);
    }
}
//NOVENO INTENTO DE RUNNER
