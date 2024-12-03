using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isMine; 
    public int adjacentMines;
    public bool isRevealed; 
    public bool isFlagged; 

    private Renderer cellRenderer;

    [SerializeField] private Texture[] textures; // Array of textures for numbers
    [SerializeField] private Texture flagTexture; // Texture for flag
    [SerializeField] private Texture mineTexture; // Texture for mine

    void Start()
    {
        cellRenderer = GetComponent<Renderer>();
    }

    public void Reveal()
    {
        if (isFlagged) return; // impossible to open tagged cell

        isRevealed = true;

        if (isMine)
        {
            cellRenderer.material.mainTexture = mineTexture;
            Debug.Log("Game Over!");
        }
        else
        {
            cellRenderer.material.color = Color.white; // open the cell
            
            AudioManager.PlaySound(SoundType.Button3, 0.7f);

            
            if (adjacentMines >= 0 && adjacentMines < textures.Length)
            {
                cellRenderer.material.mainTexture = textures[adjacentMines];
            }

            if (adjacentMines > 0)
            {
                Debug.Log("Adjacent Mines: " + adjacentMines);
            }
        }
    }

    public void ToggleFlag()
    {
        if (isRevealed) return; // impossible to mark a cell as flag

        isFlagged = !isFlagged;

        if (isFlagged)
        {
            cellRenderer.material.mainTexture = flagTexture; 
        }
        else
        {
            cellRenderer.material.mainTexture = null; 
        }
    }


}
