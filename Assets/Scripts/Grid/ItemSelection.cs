using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelection : MonoBehaviour
{
    public Text _nameText;
    public Text _definationText;
    public Image _definationImage;
    public GameObject _productionFrame;
    Image _productionImage;
    UnitSpawnButton _productionButton;
    void Start()
    {
        _nameText.text = string.Empty;
        _definationImage.enabled = false;
        _productionImage = _productionFrame.transform.GetChild(0).GetComponent<Image>();
        var allControllers = GameObject.FindObjectsOfType<GridItemController>();
        _productionButton = FindObjectOfType<UnitSpawnButton>();
        //foreach (var controller in allControllers)
        //{
        //    controller.onItemPicked += HandleItemPicked;
        //    controller.onItemHovered += HandleItemHover;
        //}
    }
    public void HandleItemHover(IProductionItem item, GameObject buildGameobject)
    {
        if (item != null && item.canDrop)
        {
            _nameText.text = (item as ItemDefinition).Name;
            _definationText.text = (item as ItemDefinition).Defination;
            _definationImage.enabled = true;
            _definationImage.sprite = (item as ItemDefinition).sprite;
            _productionFrame.SetActive(false);
            _productionImage.sprite = null;
            if ((item as ItemDefinition).canProduce)
            {
                _productionFrame.SetActive(true);
                _productionImage.sprite = (item as ItemDefinition).productionSprite;
                _productionButton = FindObjectOfType<UnitSpawnButton>();
                if (_productionButton != null)
                {
                    _productionButton.Unit = (item as ItemDefinition)._unitGameobject;
                    _productionButton.spawnerBuild = buildGameobject;
                }
               

            }
            else
            {
                _productionFrame.SetActive(false);
                _productionImage.sprite = null;
            }
        }
    }
    public void HandleItemPicked(IProductionItem item,GameObject buildGameobject)
    {
        if (item != null)
        {
            Debug.Log("Picked");
            _nameText.text = (item as ItemDefinition).Name;
            _definationText.text = (item as ItemDefinition).Defination;
            _definationImage.enabled = true;
            _definationImage.sprite = (item as ItemDefinition).sprite;
            _productionButton = FindObjectOfType<UnitSpawnButton>();
            if ((item as ItemDefinition).canProduce)
            {
                _productionFrame.SetActive(true);
                _productionImage.sprite = (item as ItemDefinition).productionSprite;
                if (_productionButton != null)
                {
                    _productionButton.Unit = (item as ItemDefinition)._unitGameobject;
                    _productionButton.spawnerBuild = buildGameobject;
                }
            }
            else
            {
                _productionFrame.SetActive(false);
                _productionImage.sprite = null;
            }
        }
        else
        {
            Debug.Log(" no Picked");
            _nameText.text = string.Empty;
            _definationText.text = string.Empty;
            _definationImage.sprite = null;
            _definationImage.enabled = false;
            _productionFrame.SetActive(false);
            _productionImage.sprite = null;
        }
    }
}
