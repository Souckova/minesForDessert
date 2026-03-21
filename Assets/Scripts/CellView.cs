using UnityEngine;
using UnityEngine.UI;

public class CellView : MonoBehaviour
{
    private Cell cellData;
    private Board board;

    //Ten text je jenom nějaký provizorní zobrazení hodnot přímo ve hře, pak to samozřejmě bude vypadat jinak
    public Text text;

    //metoda volaná při spawnování, používá se jen jednou
    public void SetData(Cell cell)
    {
        cellData = cell;
        cellData.OnRevealed += UpdateVisual;
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

    public void OnClick()
    {
        board.RevealCell(cellData);
    }
}