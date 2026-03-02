using UnityEngine;
using TMPro;

public class CellView : MonoBehaviour
{
    private Cell cellData;

    //Ten text je jenom nějaký provizorní zobrazení hodnot přímo ve hře, pak to samozřejmě bude vypadat jinak
    public TMP_Text text;

    public void SetData(Cell cell)
    {
        cellData = cell;
        UpdateVisual();
    }

    //pak až budem chtít zobrazit ve hře změny
    void UpdateVisual()
    {
        text.text = cellData.ToString();
    }
}