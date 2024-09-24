using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;
namespace Merge
{
    public class ClassicMode : GameMode
    {
        public List<Fruit> listFruit;
        public List<FruitType> fruitUnlocked;
        public FruitContainerSO fruitContainer;
        public Transform line;
        public Transform container;
        public GameObject instruction;
        private UIIngameScreen ingameScreen;
        private Camera mainCamera;
        public ParticleSystem fxMergePrefab;
        public float ySpawnPos = 5f;
        public Vector2 mapSize;
        public FruitType currentFruit;
        public FruitType nextFruit;
        public Fruit fruitCurrent;
        public bool isEnded;
        public bool isUsingBooster;
        public EBoosterType boosterUsing;
        public int score;
        private float spawnIntervalTimer;
        private float comboTimer;
        private int comboCount;
        private bool isSelected;
        private float bannerFlagTimer;
        private bool ready;
        private bool isStart;
        private float saveTimer;
        private float tutorialTime;
        private int showRate;
        private float timer;
        private static bool isFirstNative;
        private List<ComboSequence> listCombo;
        public Vector2 destroyHorizontalSize;
        public Vector2 destroyVerticalSize;
        [Serializable]
        public class HeightWeightConfig
        {
            public float heightThreshold;
            public float[] weights = new float[5];
        }
        public HeightWeightConfig[] heightWeightConfigs;


        public string saveData
        {
            get { return PlayerPrefs.GetString("classic_mode_save_data", ""); }
            set { PlayerPrefs.SetString("classic_mode_save_data", value); }
        }

        private int showRateCount
        {
            get { return PlayerPrefs.GetInt("show_rate_count", 0); }
            set { PlayerPrefs.SetInt("show_rate_count", value); }
        }
        public override void Initialize()
        {
            TrackingController.OnStartGame();
            Observer.AddObserver(UIBoosterPanel.CancerUseBoosterKey, CancerBooster);
            Fruit.onCollisionWithFruit += CollisionBetweenFruitsCallback;
           // Debug.Log("bbbb");
            GameManager.PassMinutes += PassMinutes;
            listFruit = new List<Fruit>();
            listCombo = new List<ComboSequence>();
            fruitUnlocked = new List<FruitType>();
            Bounds bounds = new Bounds(transform.position, mapSize);
            mainCamera = Camera.main;
            SetView(bounds);
            ready = false;
            showRate = 1;
        }
        public void SetView(Bounds bounds)
        {
            float sizeX = bounds.size.x;
            float orthographicSize = sizeX * Screen.height / Screen.width * 0.5f;
            mainCamera.orthographicSize = orthographicSize;
        }

        private void CancerBooster(object data)
        {
            EnableHighLight(false);
            isUsingBooster = false;
        }

        private void PassMinutes(float totalMinute)
        {
            showRate--;
            if (showRate == 0 && showRateCount < 2)
            {
                showRateCount++;
                // SDKManager.Instance.showRate();
            }
        }

        public override void LoadLevel()
        {
            gameObject.SetActive(true);
            ingameScreen = UIManager.Instance.ShowScreen<UIIngameScreen>();
            timer = 0;
            tutorialTime = 0;
            listFruit.Clear();
            listCombo.Clear();
            isEnded = false;
            isUsingBooster = false;
            isSelected = false;
            ready = true;
            if (isFirstNative || DataManager.TotalMinutePlay >= 2)
            {
                // ingameScreen.ShowNative();
            }
            if (saveData.Length > 0)
            {
                ClassicDataSave serializableFruitList = JsonUtility.FromJson<ClassicDataSave>(saveData);
                fruitUnlocked = serializableFruitList.fruitTypes;
                for (int i = 0; i < serializableFruitList.listFruitData.Count; i++)
                {
                    FruitInfo info = fruitContainer.GetFruit(serializableFruitList.listFruitData[i].type);
                    Fruit fruit = SpawnFruit(info.fruitType, serializableFruitList.listFruitData[i].position, true, false);
                    fruit.hasContacted = true;
                }
                score = serializableFruitList.score;
            }
            else
            {
                score = 0;
            }
            ingameScreen.SetScore(score, score);
            currentFruit = fruitContainer.container[0].fruitType;
            nextFruit = fruitContainer.container[0].fruitType;
            spawnIntervalTimer = 0;
            ingameScreen.SetNextFruit(nextFruit);
            bannerFlagTimer = GameManager.CollapseSystem[1];

        }
        private void Update()
        {
            if (GameManager.GameState != EGameState.RUNNING) return;
            if (isEnded || !ready) return;
            timer += Time.deltaTime;
            saveTimer += Time.deltaTime;
            if (saveTimer >= 5)
            {
                Save();
                saveTimer = 0;
            }
            if (timer >= 60 && DataManager.TotalMinutePlay > 1 && !isFirstNative)
            {
                isFirstNative = true;
                // ingameScreen.ShowNative();
                timer = 0;
            }
            spawnIntervalTimer -= Time.deltaTime;
            line.gameObject.SetActive(spawnIntervalTimer <= 0);
            if (comboTimer > 0)
            {
                comboTimer -= Time.deltaTime;
                if (comboTimer <= 0)
                {
                    if (comboCount >= 2)
                    {
                        int bonus = comboCount * 25;
                        ingameScreen.SetCombo(comboCount, bonus, () =>
                        {
                            int s = score;
                            score += bonus;
                            ingameScreen.SetScore(s, score);
                        });
                    }
                    comboTimer = 0;
                    comboCount = 0;
                }
            }
            tutorialTime += Time.deltaTime;
            if(tutorialTime >= 10 && !isUsingBooster)
            {
                UIStartTutorial uIStartTutorial = UIManager.Instance.popupHolder.GetComponentInChildren<UIStartTutorial>(true);
                uIStartTutorial.gameObject.SetActive(true);
                uIStartTutorial.txtTutorial.SetActive(false);
                tutorialTime = 0;
            }
            for (int i = 0; i < listCombo.Count; i++)
            {
                listCombo[i].UpdateCombo(Time.deltaTime);
            }
            HandleBannerCollapse();
            HandleCurrentSpawnFruit();
            HandleSelect();
            BoosterHandleSelectFruit();
            CheckLose();
            BoosterHandlerSeclectPosition();
    #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.L))
            {
                OnLose();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                OnMissionComplete();
            }
    #endif
        }
        private bool IsPointerOverUIObject()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Any(result => result.gameObject.layer == LayerMask.NameToLayer("UI"));
        }
        private void HandleSelect()
        {
            if (isUsingBooster|| IsPointerOverUIObject())
            {   
                isSelected = false;
                return;
            }
            
            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
            {
                if (!isStart)
                {
                    isStart = true;
                    TrackingController.StartCountTime();
                }
                Vector3 mousePos = Input.mousePosition;
                mousePos = mainCamera.ScreenToWorldPoint(mousePos);
                float sizeX = mapSize.x / 2f;
                if (fruitCurrent)
                {
                    sizeX -= (fruitCurrent.GetBounds().size.x / 2f);
                }
                mousePos.x = Mathf.Clamp(mousePos.x, -sizeX, sizeX);
                mousePos.y = ySpawnPos;
                line.position = new Vector3(mousePos.x, line.position.y);
                if (fruitCurrent)
                {
                    fruitCurrent.transform.position = new Vector3(mousePos.x, fruitCurrent.transform.position.y, 0);
                }
                isSelected = true;
                tutorialTime = 0;
            }
            if (Input.GetMouseButtonUp(0) && fruitCurrent && isSelected)
            {
                spawnIntervalTimer = 0.5f;
                AudioManager.Instance.PlayOneShot("Drop", 1f);
                fruitCurrent.EnablePhysic();
                fruitCurrent.StartDrop();
                listFruit.Add(fruitCurrent);
                fruitCurrent = null;
                HandleNextFruit();
                TrackingController.LogMove();
            }
        }
        private void HandleCurrentSpawnFruit()
        {
            if (fruitCurrent || spawnIntervalTimer > 0) return;
            fruitCurrent = SpawnFruit(currentFruit, new Vector3(line.position.x, ySpawnPos, 0), false, false);
            listFruit.Remove(fruitCurrent);
        }
        private void HandleBannerCollapse()
        {
            bannerFlagTimer -= Time.deltaTime;
            if (DataManager.Level >= GameManager.CollapseSystem[0] && bannerFlagTimer <= 0)
            {
                bannerFlagTimer = GameManager.CollapseSystem[1];
                // AdsHelperWrapper.ShowCollapseBanner("cl_ingame");
            }
        }
        private void CheckLose()
        {
            if (listFruit == null) return;
            for (int i = 0; i < listFruit.Count; i++)
            {
                if (!listFruit[i].hasContacted) continue;
                if (listFruit[i].position.y >= ySpawnPos)
                {
                    TrackingController.OnEndGame(false, "classic_hit game over line");
                    OnLose();
                    break;
                }
            }
        }
        private void OnLose()
        {
            if (isEnded) return;
            isEnded = true;
            UILoseScreen loseScreen = UIManager.Instance.ShowScreen<UILoseScreen>();
            loseScreen.SetUp(score, DataManager.HighScoreClassicMode, (cb) =>
            {
                if (cb)
                {
                    isEnded = false;
                    ingameScreen = UIManager.Instance.ShowScreen<UIIngameScreen>();
                    TrackingController.OnEndGame(false, "classic_lose");
                    ingameScreen.SetScore(score, score);
                    List<Fruit> fruits = new List<Fruit>(listFruit);
                    for (int i = 0; i < fruits.Count; i++)
                    {
                        if ((int)(fruits[i].GetFruitType()) <= 3)
                        {
                            RemoveFruit(fruits[i]);
                            Destroy(fruits[i].gameObject);
                        }
                    }
                }
                else
                {
                    saveData = "";
                    Clear();
                    GameManager.Instance.PlayClassicMode();
                }
            });
        }

        private void OnMissionComplete()
        {
            if (isEnded) return;
            isEnded = true;
            // UIADWinScreen winScreen = UIManager.Instance.ShowScreen<UIADWinScreen>();
            //winScreen.SetUp(score, score);

        }
        private void CollisionBetweenFruitsCallback(Fruit arg1, Fruit arg2)
        {
            if (!arg1 || !arg2) return;
            ProcessMerge(arg1, arg2);
        }
        private void ProcessMerge(Fruit sender, Fruit otherfruit)
        {
            if (sender.hasCollided || otherfruit.hasCollided) return;
            sender.hasCollided = true;
            otherfruit.hasCollided = true;
            FruitType nextFruitType = FruitType.Size10;
            if (sender.GetFruitType() != FruitType.Size10)
            {
                nextFruitType = sender.GetFruitType() + 1;
            }
            Vector2 fruitSpawnPos = Vector3.Lerp(otherfruit.position, sender.position, 0.5f);
            // CheckHeartFruit(nextFruitType, new Vector3(fruitSpawnPos.x, fruitSpawnPos.y, 0));
            int newScore = score + (((int)sender.GetFruitType() + 1) * 10);
            ingameScreen.SetScore(score, newScore);
            score = newScore;
            comboTimer = 0.5f;
            comboCount++;
            AudioManager.Instance.PlayOneShot("Pop4", 1f);
            // OnMergeFruit?.Invoke(sender, otherfruit, comboCount);
            ComboSequence cb = GetComboSequence(sender);
            if (cb == null)
            {
                cb = GetComboSequence(otherfruit);
            }
            if (cb != null && cb.count >= 2)
            {
                AudioManager.Instance.PlayOneShot("Merge" + Random.Range(1, 4), 0.8f);
            }
            if (sender.position.y > otherfruit.position.y)
            {
                otherfruit.DisablePhysic();
                otherfruit.transform.DOScale(0, 0.2f).SetEase(Ease.InCubic).SetTarget(otherfruit);
                sender.transform.DOMoveX(otherfruit.transform.position.x, 0.2f).SetEase(Ease.InCubic).OnComplete(() =>
                {
                    if (sender.GetFruitType() != FruitType.Size10)
                    {
                        Fruit newfruit = SpawnFruit(nextFruitType, sender.transform.position, true, true);
                        newfruit.hasContacted = true;
                        if (cb != null)
                        {
                            cb.IncreaseCombo(newfruit);
                        }
                        else
                        {
                            cb = CreateCombo(newfruit);
                        }
                    }
                    ParticleSystem p = Instantiate(fxMergePrefab, transform);
                    p.transform.position = fruitSpawnPos;
                    ChangeColorParticle(p, nextFruitType);
                    p.transform.localScale = Vector3.one * GetFruitTypeScale(nextFruitType);
                    p.Play();
                    Destroy(p.gameObject, 2f);
                    Destroy(otherfruit.gameObject);
                    Destroy(sender.gameObject);
                }).SetTarget(sender);

            }
            else
            {
                sender.DisablePhysic();
                sender.transform.DOScale(0, 0.2f).SetEase(Ease.InCubic).SetTarget(sender);
                otherfruit.transform.DOMoveX(sender.transform.position.x, 0.2f).SetEase(Ease.InCubic).OnComplete(() =>
                {
                    if (sender.GetFruitType() != FruitType.Size10)
                    {
                        Fruit newfruit = SpawnFruit(nextFruitType, otherfruit.transform.position, true, true);
                        newfruit.hasContacted = true;
                        if (cb != null)
                        {
                            cb.IncreaseCombo(newfruit);
                        }
                        else
                        {
                            cb = CreateCombo(newfruit);
                        }
                    }
                    ParticleSystem p = Instantiate(fxMergePrefab, transform);
                    p.transform.position = fruitSpawnPos;
                    ChangeColorParticle(p, nextFruitType);
                    p.transform.localScale = Vector3.one * GetFruitTypeScale(nextFruitType);
                    p.Play();
                    Destroy(p.gameObject, 2f);
                    Destroy(otherfruit.gameObject);
                    Destroy(sender.gameObject);
                }).SetTarget(otherfruit);
            }
            RemoveFruit(sender);
            RemoveFruit(otherfruit);
        }

        public void ChangeColorParticle(ParticleSystem particleSystem, FruitType fruitType)
        {
            var main = particleSystem.main;
            main.startColor = new ParticleSystem.MinMaxGradient(GetFruitTypeColor(fruitType), Color.white);
        }
        public Color GetFruitTypeColor(FruitType fruitType)
        {
            FruitInfo fruitInfo = new FruitInfo();
            for (int i = 0; i < fruitContainer.container.Length; i++)
            {
                FruitInfo f = fruitContainer.container[i];
                if (f.fruitType == fruitType)
                {
                    fruitInfo = f;
                    break;
                }
            }
            return fruitInfo.color;
        }

        public float GetFruitTypeScale(FruitType fruitType)
        {
            FruitInfo fruitInfo = new FruitInfo();
            for (int i = 0; i < fruitContainer.container.Length; i++)
            {
                FruitInfo f = fruitContainer.container[i];
                if (f.fruitType == fruitType)
                {
                    fruitInfo = f;
                    break;
                }
            }
            return fruitInfo.scale;
        }

        private ComboSequence GetComboSequence(Fruit fruit)
        {
            for (int i = 0; i < listCombo.Count; i++)
            {
                if (listCombo[i].fruit == fruit)
                {
                    return listCombo[i];
                }
            }
            return null;
        }
        private ComboSequence CreateCombo(Fruit f)
        {
            ComboSequence combo = null;
            combo = new ComboSequence(f, () =>
            {
                int bonus = combo.count * 25;
                if (combo.count >= 2)
                {
                    ingameScreen.SetCombo(combo.count, bonus, () =>
                    {
                        int s = score;
                        score += bonus;
                        ingameScreen.SetScore(s, score);
                    });
                }
                listCombo.Remove(combo);
            });
            listCombo.Add(combo);
            return combo;
        }
        private Fruit SpawnFruit(FruitType fruitType, Vector3 spawnPosition, bool enablePhysic, bool animationSpawn)
        {
            FruitInfo info = fruitContainer.GetFruit(fruitType);
            Fruit newFruit = Instantiate(info.GetObject(), container);
            if (animationSpawn)
            {
                newFruit.OnSpawn();
                CheckNewFruit(newFruit);
                CheckHeartFruit(fruitType, spawnPosition);
            }
            listFruit.Add(newFruit);
            newFruit.gameObject.SetActive(true);
            newFruit.Initialize();
            if (enablePhysic)
            {
                newFruit.EnablePhysic();
            }
            newFruit.transform.position = spawnPosition;
            return newFruit;
        }
        private void HandleNextFruit()
        {
            currentFruit = nextFruit;
            float[] weights = GetWeightsBasedOnHeight(HighestFruit());
            nextFruit = GetRandomFruitBasedOnWeights(weights);
            ingameScreen.SetNextFruit(nextFruit);
        }

        private float HighestFruit()
        {
            float highestFruit = listFruit[0].position.y;
            for (int i = 0; i < listFruit.Count - 1; i++)
            {
                if (listFruit[i].position.y > highestFruit)
                {
                    highestFruit = listFruit[i].position.y;
                }
            }
            return highestFruit;
        }
        private float[] GetWeightsBasedOnHeight(float height)
        {
            foreach (var config in heightWeightConfigs)
            {
                if (height < config.heightThreshold)
                {
                    return config.weights;
                }
            }
            return heightWeightConfigs[heightWeightConfigs.Length - 1].weights;
        }

        private FruitType GetRandomFruitBasedOnWeights(float[] weights)
        {
            float total = 0;
            foreach (float weight in weights)
            {
                total += weight;
            }

            float randomPoint = Random.value * total;

            for (int i = 0; i < weights.Length; i++)
            {
                if (randomPoint < weights[i])
                {
                    return fruitContainer.container[i].fruitType;
                }
                else
                {
                    randomPoint -= weights[i];
                }
            }
            return fruitContainer.container[0].fruitType;
        }

        private void CheckNewFruit(Fruit fruit)
        {
            FruitType fruitType = fruit.GetFruitType();
            if (fruitUnlocked.Contains(fruitType)) return;
            if (GameManager.NewFruitSystem[0] == 1 && (int)fruitType >= GameManager.NewFruitSystem[1])
            {
                var popup = UIManager.Instance.ShowPopup<UINewFruitPopup>(() =>
                {
                    UIManager.Instance.GetScreen<UIIngameScreen>().gameObject.SetActive(true);
                });
                UIManager.Instance.GetScreen<UIIngameScreen>().gameObject.SetActive(false);
                popup.SetIcon(fruit);
            }
            fruitUnlocked.Add(fruitType);
            Save();
        }
        private void CheckHeartFruit(FruitType fruitType, Vector3 fruitTransform)
        {
            // FruitType fruitType = fruit.GetFruitType();
            if ((int)fruitType > 5)
            {
                CoinFx.Instance.PlayFx(() =>
                    {
                        CapybaraMain.Manager.Instance.SetHeart(CapybaraMain.Manager.Instance.GetHeart() + 1);
                        UIManager.Instance.GetScreen<UIIngameScreen>().HeartText();
                    }, 0, fruitTransform, (int)fruitType - 5);
            }
        }
        #region Booster
        private void BoosterHandleSelectFruit()
        {
            if (!isUsingBooster) return;
            if (Input.GetMouseButtonDown(0))
            {
                var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                ray.direction = Vector3.forward;
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray, 100f);
                if (hit)
                {
                    Fruit fruit = hit.transform.GetComponent<Fruit>();
                    if (fruit)
                    {
                        switch (boosterUsing)
                        {
                            case EBoosterType.REMOVE:
                                RemoveFruit(fruit);
                                Helper.CreateCounter(0.1f, () =>
                                {
                                    isUsingBooster = false;
                                });
                                EnableHighLight(false);
                                ParticleSystem p = Instantiate(fxMergePrefab, transform);
                                p.transform.position = fruit.position;
                                p.Play();
                                Destroy(p.gameObject, 2f);
                                Destroy(fruit.gameObject);
                                Observer.Notify(UIBoosterPanel.FinishBoosterKey, EBoosterType.REMOVE);
                                break;
                            case EBoosterType.EVOLUTION:
                                if (fruit.GetFruitType() != FruitType.Size10)
                                {
                                    FruitType nf = fruit.GetFruitType() + 1;
                                    Fruit f = SpawnFruit(nf, fruit.position, true, true);
                                    f.hasContacted = true;
                                }
                                RemoveFruit(fruit);
                                Helper.CreateCounter(0.1f, () =>
                                {
                                    isUsingBooster = false;
                                });
                                EnableHighLight(false);
                                ParticleSystem px = Instantiate(fxMergePrefab, transform);
                                px.transform.position = fruit.position;
                                px.Play();
                                Destroy(fruit.gameObject);
                                Destroy(px.gameObject, 2f);
                                Observer.Notify(UIBoosterPanel.FinishBoosterKey, EBoosterType.EVOLUTION);
                                break;
                        }
                    }
                }
            }
        }

        private void BoosterHandlerSeclectPosition()
        {
            if (!isUsingBooster) return;
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Input.mousePosition;
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                switch (boosterUsing)
                {
                    case EBoosterType.DESTROYHORIZONTAL:
                        Collider2D[] hcolliders = Physics2D.OverlapBoxAll(worldPosition, destroyHorizontalSize, 0);

                        Vector2 halfSize = destroyHorizontalSize / 2f;
                        Vector2 topLeft = new Vector2(worldPosition.x - halfSize.x, worldPosition.y + halfSize.y);
                        Vector2 topRight = new Vector2(worldPosition.x + halfSize.x, worldPosition.y + halfSize.y);
                        Vector2 bottomLeft = new Vector2(worldPosition.x - halfSize.x, worldPosition.y - halfSize.y);
                        Vector2 bottomRight = new Vector2(worldPosition.x + halfSize.x, worldPosition.y - halfSize.y);

                        Debug.DrawLine(topLeft, topRight, Color.red);
                        Debug.DrawLine(topRight, bottomRight, Color.red);
                        Debug.DrawLine(bottomRight, bottomLeft, Color.red);
                        Debug.DrawLine(bottomLeft, topLeft, Color.red);

                        foreach (Collider2D collider in hcolliders)
                        {
                            if (collider.CompareTag("Fruit"))
                            {
                                Fruit fruit = collider.transform.GetComponent<Fruit>();
                                RemoveFruit(fruit);
                                Destroy(fruit.gameObject);
                                Helper.CreateCounter(0.1f, () =>
                                {
                                    isUsingBooster = false;
                                });
                                Observer.Notify(UIBoosterPanel.FinishBoosterKey, EBoosterType.DESTROYHORIZONTAL);
                            }
                        }
                        break;
                    case EBoosterType.DESTROYVERTICAL:
                        Collider2D[] colliders = Physics2D.OverlapBoxAll(worldPosition, destroyVerticalSize, 0);

                        Vector2 halfVerticalSize = destroyVerticalSize / 2f;
                        Vector2 topVerticalLeft = new Vector2(worldPosition.x - halfVerticalSize.x, worldPosition.y + halfVerticalSize.y);
                        Vector2 topVerticalRight = new Vector2(worldPosition.x + halfVerticalSize.x, worldPosition.y + halfVerticalSize.y);
                        Vector2 bottomVerticalLeft = new Vector2(worldPosition.x - halfVerticalSize.x, worldPosition.y - halfVerticalSize.y);
                        Vector2 bottomVerticalRight = new Vector2(worldPosition.x + halfVerticalSize.x, worldPosition.y - halfVerticalSize.y);

                        Debug.DrawLine(topVerticalLeft, topVerticalRight, Color.blue);
                        Debug.DrawLine(topVerticalRight, bottomVerticalRight, Color.blue);
                        Debug.DrawLine(bottomVerticalRight, bottomVerticalLeft, Color.blue);
                        Debug.DrawLine(bottomVerticalLeft, topVerticalLeft, Color.blue);

                        foreach (Collider2D collider in colliders)
                        {
                            if (collider.CompareTag("Fruit"))
                            {
                                Fruit fruit = collider.transform.GetComponent<Fruit>();
                                RemoveFruit(fruit);
                                Destroy(fruit.gameObject);
                                Helper.CreateCounter(0.1f, () =>
                                {
                                    isUsingBooster = false;
                                });
                                Observer.Notify(UIBoosterPanel.FinishBoosterKey, EBoosterType.DESTROYVERTICAL);
                            }
                        }
                        break;
                }
            }
        }
        public void SelectRemove()
        {
            isUsingBooster = true;
            boosterUsing = EBoosterType.REMOVE;
            EnableHighLight(true);
        }
        public void EvolveFruit()
        {
            isUsingBooster = true;
            boosterUsing = EBoosterType.EVOLUTION;
            EnableHighLight(true);
        }
        public void ClearFruits()
        {
            List<Fruit> fruits = new List<Fruit>(listFruit);
            for (int i = 0; i < fruits.Count; i++)
            {
                if ((int)(fruits[i].GetFruitType()) <= 2)
                {
                    RemoveFruit(fruits[i]);
                    Destroy(fruits[i].gameObject);
                }
            }
            Observer.Notify(UIBoosterPanel.FinishBoosterKey, EBoosterType.CLEAR);
        }
        public void DestroyHorizontalFruits()
        {
            isUsingBooster = true;
            boosterUsing = EBoosterType.DESTROYHORIZONTAL;
        }
        public void DestroyVerticalFruits()
        {
            isUsingBooster = true;
            boosterUsing = EBoosterType.DESTROYVERTICAL;
        }
        public void EnableHighLight(bool status)
        {
            for (int i = 0; i < listFruit.Count; i++)
            {
                listFruit[i].highlightObj.SetActive(status);
            }
        }
        #endregion
        public void RemoveFruit(Fruit fruit)
        {
            if (listFruit.Contains(fruit))
            {
                listFruit.Remove(fruit);
            }
        }
        private void Save()
        {
            saveData = "";
            ClassicDataSave serializableFruitList = new ClassicDataSave();
            for (int i = 0; i < listFruit.Count; i++)
            {
                if (!listFruit[i].hasContacted) continue;
                FruitData fruitData = new FruitData();
                fruitData.type = listFruit[i].GetFruitType();
                fruitData.position = listFruit[i].position;
                serializableFruitList.listFruitData.Add(fruitData);
            }
            serializableFruitList.fruitTypes = new List<FruitType>(fruitUnlocked);
            serializableFruitList.score = score;
            string data = JsonUtility.ToJson(serializableFruitList);
            saveData = data;
        }
        public override void Clear()
        {
            for (int i = 0; i < listFruit.Count; i++)
            {
                Destroy(listFruit[i].gameObject);
            }
            if (fruitCurrent)
            {
                Destroy(fruitCurrent.gameObject);
            }
            listFruit.Clear();
            Observer.RemoveObserver(UIBoosterPanel.CancerUseBoosterKey, CancerBooster);
            Fruit.onCollisionWithFruit -= CollisionBetweenFruitsCallback;
            GameManager.PassMinutes -= PassMinutes;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, mapSize);
        }
    }
    [System.Serializable]
    public class ClassicDataSave
    {
        public ClassicDataSave()
        {
            listFruitData = new List<FruitData>();
            fruitTypes = new List<FruitType>();
            score = 0;
        }
        public List<FruitData> listFruitData;
        public List<FruitType> fruitTypes;
        public int score;
    }
    [System.Serializable]
    public class FruitData
    {
        public FruitType type;
        public Vector3 position;
    }
    public class ComboSequence
    {
        public ComboSequence(Fruit fruit, Action complete)
        {
            onComplete = complete;
            count = 1;
            timer = 0.75f;
            this.fruit = fruit;
            isActive = true;
        }
        public event Action onComplete;
        public Fruit fruit;
        public int count;
        public float timer;
        private bool isActive;
        public void UpdateCombo(float deltaTime)
        {
            if (!isActive) return;
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                isActive = false;
                onComplete?.Invoke();
            }
        }
        public void IncreaseCombo(Fruit fruit)
        {
            this.fruit = fruit;
            count++;
            timer = 0.75f;
        }
    }
}