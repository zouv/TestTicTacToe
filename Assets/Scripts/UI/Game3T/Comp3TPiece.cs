using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comp3TPiece : MonoBehaviour
{
    public UIGame3T uiRoot;

    private Transform _tsX;
    private Transform _tsO;
    private PiecePos _pos;

    public void Init(PiecePos pos)
    {
        _pos = pos;
    }

    void Awake()
    {
        _tsX = this.transform.Find("X"); // Íæ¼Ò
        _tsO = this.transform.Find("O"); // AI
    }

    void OnEnable()
    {
        ResetPlace();
    }

    public void ResetPlace()
    {
        _tsX.gameObject.SetActive(false);
        _tsO.gameObject.SetActive(false);
    }

    public void OnClick()
    {
        if (uiRoot.IsAllowedToPlace()) return;
        else if (_tsO.gameObject.activeSelf) return;
        PlayerPlace();
        uiRoot.SetPlayerPlace(_pos);
        uiRoot.AITurn();
    }

    private void PlayerPlace()
    {
        _tsX.gameObject.SetActive(true);
    }

    public void AIPlace()
    {
        _tsO.gameObject.SetActive(true);
    }
}
