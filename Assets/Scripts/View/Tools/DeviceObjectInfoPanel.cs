using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Obstacles;
using Bombs;

public class DeviceObjectInfoPanel : MonoBehaviour
{
    Image icon;
    Text numbers;
    GenericDeviceGameObject parentDevice;
    ElectroChaffCloud parentObstacle;

    public void setParentObstacle(ElectroChaffCloud parentObstacle)
    {
        this.parentObstacle = parentObstacle;
    }

    private void Start()
    {
        icon = transform.Find("Fuse/Icon").GetComponent<Image>();
        numbers = transform.Find("Fuse/Number").GetComponent<Text>();
        parentDevice = transform.GetComponentInParent<GenericDeviceGameObject>();
        if (parentDevice != null)
        {
            var parentModelOffset = parentDevice.transform.Find("Explosion")?.transform.localPosition ?? Vector3.zero;
            transform.localPosition = new Vector3(parentModelOffset.x, transform.localPosition.y, parentModelOffset.z);
        }
    }

    private void Update()
    {
        if (parentDevice != null)
        {
            if(parentDevice.IsFused)
            {
                if(!icon.gameObject.activeInHierarchy)
                {
                    icon.gameObject.SetActive(true);
                }                    
                if (!numbers.gameObject.activeInHierarchy)
                {
                    numbers.gameObject.SetActive(true);
                }
                numbers.text = $"T-{parentDevice.Fuses}";
                LookAtCamera();
            }
            else
            {
                if (icon.gameObject.activeInHierarchy)
                {
                    icon.gameObject.SetActive(false);
                }
                if(numbers.gameObject.activeInHierarchy)
                {
                    numbers.gameObject.SetActive(false);
                }
            }
        }
        if (parentObstacle != null)
        {
            if (parentObstacle.IsFused)
            {
                if (!icon.gameObject.activeInHierarchy)
                {
                    icon.gameObject.SetActive(true);
                }
                if (!numbers.gameObject.activeInHierarchy)
                {
                    numbers.gameObject.SetActive(true);
                }
                numbers.text = $"T-{parentObstacle.Fuses}";
                LookAtCamera();
            }
            else
            {
                if (icon.gameObject.activeInHierarchy)
                {
                    icon.gameObject.SetActive(false);
                }
                if (numbers.gameObject.activeInHierarchy)
                {
                    numbers.gameObject.SetActive(false);
                }
            }
        }
    }

    protected void LookAtCamera()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position, Camera.main.transform.up);
    }
}
