using UnityEngine;

/// <summary>
/// Скрипт стартового окна
/// </summary>
public class StartWindowScript : MonoBehaviour {

    private ChessBoardScript boardScript;

	void Start () {
        boardScript = FindObjectOfType<ChessBoardScript>();
	}
	
    /// <summary>
    /// Начать за белых
    /// </summary>
	public void StartWhite()
    {
        if (boardScript)
        {
            boardScript.Initialize(Assets.Scripts.ChessBoardElement.PlayerColor.White);
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Начать за черных lol
    /// </summary>
    public void StartBlack()
    {
        if(boardScript)
        {
            boardScript.Initialize(Assets.Scripts.ChessBoardElement.PlayerColor.Black);
            gameObject.SetActive(false);
        }
    }
}
