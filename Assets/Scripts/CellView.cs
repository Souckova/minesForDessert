using UnityEngine;
using UnityEngine.UI;

using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CellView : MonoBehaviour
{
    private Cell cellData;
    private Board board;

    public GameObject explosionEffect;

    public BoxCollider flagTrigger;
    public XRPokeFollowAffordance pokeFollow;
    public XRSimpleInteractable interactable;
    public Transform btnTransform;

    //Ten text je jenom nějaký provizorní zobrazení hodnot přímo ve hře, pak to samozřejmě bude vypadat jinak
    public Text text;

    //metoda volaná při spawnování, používá se jen jednou
    public void SetData(Cell cell)
    {
        cellData = cell;
        cellData.OnRevealed += UpdateVisual;
        cellData.OnRevealed += DisableButton;
        cellData.OnExplode += ShowExplosion;
        cellData.OnFlag += UpdateVisual;
        cellData.OnUnFlag += UpdateVisual;
        UpdateVisual();
    }

    //předání instance pole ve kterém je mina
    public void SetBoard(Board board)
    {
        this.board = board;
    }

    //pak až budem chtít zobrazit ve hře změny
    void UpdateVisual()
    {
        text.text = cellData.ToStringForGame();
    }

    void DisableButton()
    {
        pokeFollow.maxDistance = 0f;
        pokeFollow.returnToInitialPosition = false;

        Vector3 position = btnTransform.localPosition;
        Vector3 targetPosition = new Vector3(position.x, 0f, position.z);

        btnTransform.localPosition = targetPosition;

        interactable.interactionLayers = 0;
    }

    public void OnClick()
    {
        board.RevealCell(cellData);
    }

    public void OnFlagPlaced()
    {
        board.FlagCell(cellData);
    }

    public void OnFlagRemoved()
    {
        board.UnFlagCell(cellData);
    }

    void ShowExplosion()
    {
        GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
        ParticleSystem explosionParticle = explosion.GetComponent<ParticleSystem>();
        Destroy(explosion, explosionParticle.main.duration);
        Debug.Log("Výbuch");
    }
}