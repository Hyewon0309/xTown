using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizingPanelScript : MonoBehaviour
{
    [SerializeField]
    private List<CustomizingButtonScript> _customizingColorButtons;
    [SerializeField]
    private List<CustomizingButtonScript> _customizingTextureButtons;
    [SerializeField]
    private List<Material> _avatarMaterial;

    public FlexibleColorPicker Fcp;

    private Color _normal = new Color(255 / 255, 255 / 255, 255 / 255, 255 / 255);
    private int _selected = 0;

    private void Start()
    {
        ClickButton(_selected);
    }

    private void Update()
    {
        _avatarMaterial[_selected].color= Fcp.color;
    }

    public void ClickButton(int id)
    {
        for(int i = 0; i < _customizingTextureButtons.Count; i++)
        {
            _selected = id;
            if (i == id)
            {
                //_avatarPart.gameObject.GetComponent<SkinnedMeshRenderer>().material = _avatarMaterial[i];
                _customizingTextureButtons[i].Select();
            }
            else
            {
                _customizingTextureButtons[i].Deselect();
            }
        }
    }

    public void ColorReset()
    {
        Fcp.color = Color.red;
        Fcp.color = _normal;
        Fcp.mode = FlexibleColorPicker.MainPickingMode.SV;
        for (int i = 0; i < _avatarMaterial.Count; i++)
        {
            _avatarMaterial[i].color = _normal;
        }
    }

    public void ResetSelected()
    {
        _selected = 0;
    }
}
