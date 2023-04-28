using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SudokuCell : MonoBehaviour
{
    private Board _board;
    private int _row;
    private int _col;
    private int _value;
    private string _id;
    [SerializeField] private Color _defaultColor;
    private bool _canEdit = true;
    public Text _text;
    public string ID
    {
        get => _id;
    }
    public int Value
    {
        get => _value;
    }
    public int Row
    {
        get => _row;
    }
    public int Column
    {
        get => _col;
    }
    public Color DefaultColor
    {
        get => DefaultColor;
    }

    public void SetValues(int _row, int _col, int num, string _id, Board _board)
    {
        this._row = _row;
        this._col = _col;
        this._id = _id;
        this._board = _board;

        _value = num;

        if (num != 0)
        {
            _text.text = num.ToString();
            _value = num;
        }
        else
        {
            _text.text = " ";
            num = 0;
        }

        if (num != 0)
        {
            _canEdit = false;
        }
        else
        {
            _text.color = new Color32(0, 102,187,255);
        }
    }

    public bool IsEmpty()
    {
        if(_text.text != " ")
        {
            return false;
        }
        return true;
    }

    public void ButtonClicked()
    {
        _board.DisableHighLights();
        InputButton.instance.ActivateInputButton(this);
        _board.HighLightValue(_value);
    }

    public void UpdateValue(int newValue)
    {
        if(!_canEdit){ return; }

        _value = newValue;
        _board.HighLightValue(_value);

        if (_value != 0)
        {
            _text.text = _value.ToString();
        }
        else
        {
            _text.text = "";
        }
        _board.UpdatePuzzle(_row, _col, _value);
    }
}
