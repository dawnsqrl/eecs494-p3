using System.Collections;
using UnityEngine;

public class TutorialNoteManager : MonoBehaviour
{
    [SerializeField] private string msg;

    // "Consuming a block of hyphae grants a snail exp points. A snail gains new skills / resources when the exp bar is filled. [Enter]
    // "A snail move faster on its silver slime. The slime prevents hyphae from expanding. 
    // "This is a wild snail nest. Green snails are drawn out of their nest to help with mushroom hunt."
    // "Large mushrooms are all a snail can dream of. Go get'em!"
    // "Small mushrooms are dangerous warriors of the mushroom kingdom. They attack snails on sight."
    // "A snail regenerates health faster and becomes invisible to mushrooms in bushes.

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BaseCar"))
        {
            EventBus.Publish(new DisplayHintEvent(1, msg));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("BaseCar"))
        {
            StartCoroutine(DeactivateNote());
        }
    }

    private IEnumerator DeactivateNote()
    {
        EventBus.Publish(new DismissHintEvent(1));
        yield return new WaitForSeconds(1);
        EventBus.Publish(new DismissHintEvent(1));
    }
}