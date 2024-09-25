using UnityEngine;
using UnityEngine.UI;
namespace Merge
{
    public class UIBoosterButton : MonoBehaviour
    {
        public EBoosterType boosterType;
        public Text amountText;
        public GameObject adIcon;
        public GameObject cancerIcon;
        public GameObject unlock;
        public Button button;
        public GameObject boosterBG;
        private int amount;
        private UIBoosterPanel boosterPanel;
        private bool isUsingBooster;
        // private AdventureMode adventureMode;
        private ClassicMode classicMode;
        public void Initialize(UIBoosterPanel boosterPanel)
        {
            if (GameManager.Instance.currentMode)
            {
                classicMode = GameManager.Instance.currentMode.GetComponent<ClassicMode>();
                // adventureMode = GameManager.Instance.currentMode.GetComponent<AdventureMode>();
            }
            this.boosterPanel = boosterPanel;
            button = GetComponent<Button>();
            button.onClick.AddListener(UseBooster);
            //amount = GameRes.GetRes(new DataTypeResource(RES_type.BOOSTER, (int)boosterType));
            if ((int)boosterType == 0)
            {

                amount = GameManager.Instance.minigame.items[0].quantity;
            }
            if ((int)boosterType == 6)
            {

                amount = GameManager.Instance.minigame.items[1].quantity;
            }
            amountText.text = GameRes.GetRes(new DataTypeResource(RES_type.BOOSTER, (int)boosterType)).ToString();
            if (!classicMode)
            {
                SetActiveCancer(false);
                UpdateBoosterAvailability();
            }
        }
        private void OnEnable()
        {
            if ((int)boosterType == 0)
            {
                amountText.text = (GameManager.Instance.minigame.items[0].quantity).ToString();
            }
            if ((int)boosterType == 6)
            {
                amountText.text = (GameManager.Instance.minigame.items[1].quantity).ToString();
            }
        }
        public void Refresh()
        {
            if (GameManager.Instance.currentMode)
            {
                classicMode = GameManager.Instance.currentMode.GetComponent<ClassicMode>();
                // adventureMode = GameManager.Instance.currentMode.GetComponent<AdventureMode>();
            }
            //amount = GameRes.GetRes(new DataTypeResource(RES_type.BOOSTER, (int)boosterType));
            if ((int)boosterType == 0)
            {
                amount = GameManager.Instance.minigame.items[0].quantity;
            }
            if ((int)boosterType == 6)
            {

                amount = GameManager.Instance.minigame.items[1].quantity;
            }
            amountText.text = amount.ToString();
            adIcon.gameObject.SetActive(amount <= 0);
            if (!classicMode)
            {
                amountText.transform.parent.gameObject.SetActive(amount > 0);
                if (boosterBG)
                {
                    boosterBG.SetActive(false);
                }
                UpdateBoosterAvailability();
                if (amount <= 0) { CheckAddIcon(); }
            }
            else
            {
                amountText.gameObject.SetActive(amount > 0);
            }
        }

        private void UseBooster()
        {
            // adventureMode = GameManager.Instance.currentMode.GetComponent<AdventureMode>();
            // if (adventureMode)
            // {
            //     if (isUsingBooster)
            //     {
            //         Observer.Notify(UIBoosterPanel.CancerUseBoosterKey);
            //         if (adventureMode.isUsingBooster == true)
            //         {
            //             adventureMode.isUsingBooster = false;
            //             adventureMode.pauseTimer = false;
            //             adventureMode.EnableHighLight(false);
            //             boosterBG.SetActive(false);
            //         }
            //         return;
            //     }
            //     if (amount > 0)
            //     {
            //         boosterBG.SetActive(true);
            //         boosterPanel.UseBooster(boosterType, () =>
            //         {
            //             var t = new DataTypeResource(RES_type.BOOSTER, (int)boosterType);
            //             GameRes.AddRes(t, -1, ""); 
            //             LogEventHub.LogLevelAction(new DataResource[]
            //             {
            //                 new DataResource()
            //                 {
            //                     dataTypeResource = t,
            //                     amount = 1,
            //                 }
            //             });
            //             Refresh();
            //             isUsingBooster = true;
            //         });
            //     }
            //     else
            //     {
            //         adventureMode.pauseTimer = true;
            //         UIBuyMoreBooster uIBuyMoreBooster = UIManager.Instance.ShowPopup<UIBuyMoreBooster>(null);
            //         uIBuyMoreBooster.boosterType = boosterType;
            //     }
            // }
            // else
            // {
                ClassicMode classicMode = GameManager.Instance.currentMode.GetComponent<ClassicMode>();
                if (classicMode)
                {
                    if (isUsingBooster)
                    {
                        Observer.Notify(UIBoosterPanel.CancerUseBoosterKey);
                        if (classicMode.isUsingBooster)
                        {
                            classicMode.isUsingBooster = false;
                            classicMode.EnableHighLight(false);
                        }
                        return;
                    }
                    if (amount > 0)
                    {
                        //Debug.LogError("Is Using Booster");
                        boosterPanel.UseBooster(boosterType, () =>
                        {
                            //GameRes.AddRes(new DataTypeResource(RES_type.BOOSTER, (int)boosterType), -1, "");
                            if((int)boosterType == 0)
                            {
                                GameManager.Instance.minigame.items[0].quantity -= 1;
                            }
                            if((int)boosterType == 6) {

                                GameManager.Instance.minigame.items[1].quantity -= 1;
                            }
                            Refresh();
                            isUsingBooster = true;
                        });
                    }
                    else
                    {
                        UIBuyMoreBooster uIBuyMoreBooster = UIManager.Instance.ShowPopup<UIBuyMoreBooster>(null);
                        uIBuyMoreBooster.boosterType = boosterType;
                    }
                }
            // }
            cancerIcon.SetActive(false);
        }

        public void SetActiveCancer(bool status)
        {
            cancerIcon.SetActive(status);
            isUsingBooster = status;
        }

        private void UpdateBoosterAvailability()
        {
            int currentLevel = DataManager.Level;
            bool isAvailable = false;

            switch (boosterType)
            {
                case EBoosterType.REMOVE:
                    isAvailable = currentLevel >= 3;
                    break;
                case EBoosterType.DESTROYHORIZONTAL:
                    isAvailable = currentLevel >= 5;
                    break;
                case EBoosterType.DESTROYVERTICAL:
                    isAvailable = currentLevel >= 7;
                    break;
                case EBoosterType.REROLL:
                    isAvailable = currentLevel >= 9;
                    break;
            }
            button.interactable = isAvailable;
            if (unlock)
            {
                unlock.SetActive(!isAvailable);
            }
        }

        private void CheckAddIcon()
        {
            int currentLevel = DataManager.Level;
            bool isAvailable = false;
            switch (boosterType)
            {
                case EBoosterType.REMOVE:
                    isAvailable = currentLevel >= 3;
                    break;
                case EBoosterType.DESTROYHORIZONTAL:
                    isAvailable = currentLevel >= 5;
                    break;
                case EBoosterType.DESTROYVERTICAL:
                    isAvailable = currentLevel >= 7;
                    break;
                case EBoosterType.REROLL:
                    isAvailable = currentLevel >= 9;
                    break;
            }
            // adIcon.SetActive(isAvailable);
        }
    }
}