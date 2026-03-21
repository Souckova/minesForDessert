using UnityEngine;
using UnityEngine.UI;

public class CellView : MonoBehaviour
{
    private Cell cellData;

    //Ten text je jenom nějaký provizorní zobrazení hodnot přímo ve hře, pak to samozřejmě bude vypadat jinak
    public Text text;

    //metoda volaná při spawnování, používá se jen jednou
    public void SetData(Cell cell)
    {
        cellData = cell;
        UpdateVisual();
    }

    //pak až budem chtít zobrazit ve hře změny
    void UpdateVisual()
    {
        text.text = cellData.ToStringForGame();
    }

    public void RevealCell()
    {
        cellData.isFlagged = false;
        cellData.isRevealed = true;
        UpdateVisual();
    }

    void FlagCell()
    {
        if(!cellData.isRevealed)
        {
            cellData.isFlagged = true;
            UpdateVisual();
        }
    }
}