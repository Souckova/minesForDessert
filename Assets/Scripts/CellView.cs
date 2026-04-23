using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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

    public Color zeroColor;
    public Color oneColor;
    public Color twoColor;
    public Color threeColor;
    public Color fourColor;
    public Color fiveColor;
    public Color sixColor;
    public Color sevenColor;
    public Color eightColor;

    private Dictionary<string, Color> numberColors;

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

        numberColors = new Dictionary<string, Color>
        {
            { "0", zeroColor },
            { "1", oneColor },
            { "2", twoColor },
            { "3", threeColor },
            { "4", fourColor },
            { "5", fiveColor },
            { "6", sixColor },
            { "7", sevenColor },
            { "8", eightColor },
        };
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
        string cellValue = cellData.ToStringForGame();

        text.text = cellValue;
        if(numberColors.TryGetValue(cellValue, out Color targetColor))
        {
            text.text = cellValue;
            text.color = targetColor;
        }
        else
        {
            text.text = "";
            text.color = zeroColor;
        }
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