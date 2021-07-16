using UnityEngine;
using UnityEngine.UI;

public class HPBarHandler : MonoBehaviour
{
    private Image HealthBarImage;

    public int maxHP;

    public void SetMaxHp(int value)
    {
        maxHP = value;
    }
    /// <summary>
    /// Sets the health bar value
    /// </summary>
    /// <param name="value">should be between 0 to 1</param>
    public void SetHealthBarValue(float value)
    {
        value = value / (float)maxHP;
        HealthBarImage.fillAmount = value;
        if (value < 1 && value>0)
        {

            HealthBarImage.enabled = true;
        }
        else
        {

            HealthBarImage.enabled = false;
        }
        if (HealthBarImage.fillAmount < 0.2f)
        {
            SetHealthBarColor(Color.red);
        }
        else if (HealthBarImage.fillAmount < 0.4f)
        {
            SetHealthBarColor(Color.yellow);
        }
        else
        {
            SetHealthBarColor(Color.green);
        }
    }

    public float GetHealthBarValue()
    {
        return HealthBarImage.fillAmount;
    }

    /// <summary>
    /// Sets the health bar color
    /// </summary>
    /// <param name="healthColor">Color </param>
    public void SetHealthBarColor(Color healthColor)
    {
        HealthBarImage.color = healthColor;
    }

    /// <summary>
    /// Initialize the variable
    /// </summary>
    private void Start()
    {
        HealthBarImage = GetComponent<Image>();
        HealthBarImage.enabled = false;
    }
}