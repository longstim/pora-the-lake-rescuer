using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelEditorCreateManager : MonoBehaviour {

    [System.Serializable]
    public class RubbishPackPlay
    {
        public string PackageName;
        public GameObject[] Size1;
        public GameObject[] Size2;
        public GameObject[] Size3;
        public GameObject[] Size4;
        public GameObject[] Size5;
    }


    [System.Serializable]
    public class BarrierPackPlay
    {
        public string PackageName;
        public GameObject[] Barriers;
    }

    [System.Serializable]
    public class PlaceItem
    {
        public string Name = "";
        public GameObject[] item;
    }

    [System.Serializable]
    public class BubblePackPlay
    {
        public string PackageName;
        public GameObject WhitePreview;
        public GameObject RedPreview;
        public GameObject OrangePreview;
    }

    [System.Serializable]
    public class ParentGroup
    {
        public GameObject Bubble;
        public GameObject Barrier;
        public GameObject Rubbish;
        public GameObject Background;
    }

    public enum LevelEditorMode
    { 
        Developer,
        User
    }
    public LevelEditorMode Mode;

    public ParentGroup ParentObject;
    public PlaceItem[] Places;
    public BubblePackPlay[] Bubbles;
    public RubbishPackPlay[] Rubbishes;
    public BarrierPackPlay[] Barriers;

    public MessagePopupController messagePopupController;
    public ConfirmationPopupController confirmationPopupController;
    public SavelLevelPopupController saveLevelPopupController;
    public GameObject ScrollbarRotationSpeed;
    public GameObject ScrollbarRotation;


    List<CommunicationData> dataRecieveds;

    public Transform dragLayer;
    public DragCallback dragBack;

    bool validatedStatus = false;
    public bool ValidatedStatus
    {
        get
        {
            return validatedStatus;
        }
        set
        {
            validatedStatus = value;
        }
    }
	// Use this for initialization
	void Start () {
        ReadMessage();
        ReadLevel();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void ReadMessage()
    {
        SceneCommunication sceneCommunication = new SceneCommunication(SceneName.LevelEditorCreate);
        dataRecieveds = sceneCommunication.RetrieveMessages();
    }

    private void ReadLevel()
    {
        if (dataRecieveds.Count == 3 && (dataRecieveds[0].Sender == SceneName.PlayParbaba || dataRecieveds[0].Sender == SceneName.PlayBatuguru || dataRecieveds[0].Sender == SceneName.PlayParapat || dataRecieveds[0].Sender == SceneName.PlayTomok))
        {
            Level dataLevel = (Level)dataRecieveds[0].Data;
            RefreshLevel(dataLevel);
            ValidatedStatus = ((bool)dataRecieveds[1].Data);
            bool SaveStatus = ((bool)dataRecieveds[2].Data);
            
            if (validatedStatus && SaveStatus)
            {
                if (Mode == LevelEditorMode.Developer)
                {
                    saveLevelPopupController.Show(new StaticLevel(dataLevel));
                }
                else {
                    saveLevelPopupController.Show(dataLevel);
                }
            }
        }
    }

    public void Play()
    {
        if (ParentObject.Rubbish.transform.childCount == 0)
        {
            messagePopupController.Show("Better to add rubbishes, isn't it?");
        }
        else if (ParentObject.Bubble.transform.childCount == 0)
        {
            messagePopupController.Show("Better to add bubble, isn't it?");
        }
        else if (validatedStatus)
        {
            if (Mode == LevelEditorMode.Developer)
            {
                saveLevelPopupController.Show(CreateLevelStatic());
            }
            else
            {
                saveLevelPopupController.Show(CreateLevel());
            }
        }
        else
        {
            Level level = CreateLevel();
            SceneCommunication scom = new SceneCommunication(SceneName.LevelEditorCreate);
            if (level.Place == "Parbaba")
            {
                scom.SendMessage(SceneName.PlayParbaba, level);
                Application.LoadLevel(SceneName.PlayParbaba.ToString());
            }
            else if (level.Place == "Batuguru")
            {
                scom.SendMessage(SceneName.PlayBatuguru, level);
                Application.LoadLevel(SceneName.PlayBatuguru.ToString());
            }
            else if (level.Place == "Parapat")
            {
                scom.SendMessage(SceneName.PlayParapat, level);
                Application.LoadLevel(SceneName.PlayParapat.ToString());
            }
            else if (level.Place == "Tomok")
            {
                scom.SendMessage(SceneName.PlayTomok, level);
                Application.LoadLevel(SceneName.PlayTomok.ToString());
            }
        }
    }

    private Level CreateLevel()
    {
        Level level = new Level();
        level.ScoreFor1Star = 0;
        level.ScoreFor2Star = 0;
        level.ScoreFor3Star = 0;
        
        for (int i = 0; i < ParentObject.Background.transform.childCount; i++)
        {
            if (ParentObject.Background.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                level.Place = ParentObject.Background.transform.GetChild(i).gameObject.name;
            }
        }
        for (int i = 0; i < ParentObject.Rubbish.transform.childCount; i++)
        {
            RubbishController controller = ParentObject.Rubbish.transform.GetChild(i).gameObject.GetComponent<RubbishController>();
            level.Rubbishes.Add(new Sampah(controller.PackageName, controller.Size, controller.transform.GetComponent<RectTransform>().localPosition, controller.Rotation, controller.RotationSpeed));
        }
        for (int i = 0; i < ParentObject.Barrier.transform.childCount; i++)
        {
            BarrierController controller = ParentObject.Barrier.transform.GetChild(i).gameObject.GetComponent<BarrierController>();
            level.Barriers.Add(new Penghalang(controller.PackageName, controller.Name, controller.transform.GetComponent<RectTransform>().localPosition, controller.Rotation, controller.RotationSpeed));
        }
        for (int i = 0; i < ParentObject.Bubble.transform.childCount; i++)
        {
            ItemGelembungController controller = ParentObject.Bubble.transform.GetChild(i).gameObject.GetComponent<ItemGelembungController>();
            level.Bubbles.Add(new Gelembung(controller.PackageName, controller.Type));
        }
        return level;
    }

    private StaticLevel CreateLevelStatic()
    {
        return new StaticLevel(CreateLevel());
    }

    public void AddBubble(GameObject bubble)
    {
        if (ParentObject.Bubble.transform.childCount < 10)
        {
            validatedStatus = false;

            GameObject bgObject = (GameObject)Instantiate(bubble);
            bgObject.transform.SetParent(ParentObject.Bubble.transform);
            bgObject.transform.localScale = new Vector3(1, 1, 1);
            bgObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
            bgObject.AddComponent<CanvasGroup>();
        }
        else {
            messagePopupController.Show("Max capacity of bubble is 10!");
        }
    }

    public void DeleteAll()
    {
        for (int i = 0; i < ParentObject.Rubbish.transform.childCount; i++)
        {
            Destroy(ParentObject.Rubbish.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < ParentObject.Barrier.transform.childCount; i++)
        {
            Destroy(ParentObject.Barrier.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < ParentObject.Bubble.transform.childCount; i++)
        {
            Destroy(ParentObject.Bubble.transform.GetChild(i).gameObject);
        }
    }

    private void RefreshLevel(Level dataLevel)
    {
        RefreshBackground(dataLevel.Place);
        RefreshRubbish(dataLevel.Rubbishes);
        RefreshBubble(dataLevel.Bubbles);
        RefreshBarrier(dataLevel.Barriers);
    }

    private void RefreshBackground(string Place)
    {
        for (int i = 0; i < Places.Length; i++)
        {
            if (Places[i].Name == Place)
            {
                for (int j = 0; j < Places[i].item.Length; j++)
                {
                    Places[i].item[j].SetActive(true);
                }
            }
        }
    }

    private void RefreshRubbish(List<Sampah> daftarSampah)
    {
        foreach (Sampah sampah in daftarSampah)
        {
            for (int i = 0; i < Rubbishes.Length; i++)
            {
                if (sampah.PackageName == Rubbishes[i].PackageName)
                {
                    GameObject objek;
                    switch (sampah.Size)
                    {
                        case RubbishController.RubbishSize.One:
                            objek = (GameObject)Instantiate(Rubbishes[i].Size1[Random.Range(0, Rubbishes[i].Size1.Length - 1)]);
                            break;
                        case RubbishController.RubbishSize.Two:
                            objek = (GameObject)Instantiate(Rubbishes[i].Size2[Random.Range(0, Rubbishes[i].Size2.Length - 1)]);
                            break;
                        case RubbishController.RubbishSize.Three:
                            objek = (GameObject)Instantiate(Rubbishes[i].Size3[Random.Range(0, Rubbishes[i].Size3.Length - 1)]);
                            break;
                        case RubbishController.RubbishSize.Four:
                            objek = (GameObject)Instantiate(Rubbishes[i].Size4[Random.Range(0, Rubbishes[i].Size4.Length - 1)]);
                            break;
                        default:
                            objek = (GameObject)Instantiate(Rubbishes[i].Size5[Random.Range(0, Rubbishes[i].Size5.Length - 1)]);
                            break;
                    }
                    objek.GetComponent<RubbishController>().Rotation = sampah.Rotation;
                    objek.GetComponent<RubbishController>().RotationSpeed = sampah.RotationSpeed;
                    objek.transform.SetParent(ParentObject.Rubbish.transform);
                    objek.transform.localScale = new Vector3(1, 1, 1);
                    objek.GetComponent<RectTransform>().localPosition = sampah.Position;
                    objek.AddComponent<CanvasGroup>();
                    objek.GetComponent<RubbishController>().IsEditMode = true;
                    DragHandler draghander = objek.AddComponent<DragHandler>();
                    draghander.dragLayer = dragLayer;
                    draghander.dragBack = dragBack;
                }
            }
        }
    }

    private void RefreshBarrier(List<Penghalang> daftarPenghalang)
    {
        foreach (Penghalang penghalang in daftarPenghalang)
        {
            Debug.Log(penghalang.PackageName);
            Debug.Log(penghalang.Name);
            for (int i = 0; i < Barriers.Length; i++)
            {
                if (penghalang.PackageName == Barriers[i].PackageName)
                {
                    for (int j = 0; j < Barriers[i].Barriers.Length; j++)
                    {
                        Debug.Log(Barriers[i].Barriers[j].GetComponent<BarrierController>().Name);
                        if (penghalang.Name == Barriers[i].Barriers[j].GetComponent<BarrierController>().Name)
                        {
                            GameObject objek = (GameObject)Instantiate(Barriers[i].Barriers[j]);
                            objek.GetComponent<BarrierController>().Rotation = penghalang.Rotation;
                            objek.GetComponent<BarrierController>().RotationSpeed = penghalang.RotationSpeed;
                            objek.transform.SetParent(ParentObject.Barrier.transform);
                            objek.transform.localScale = new Vector3(1, 1, 1);
                            objek.GetComponent<RectTransform>().localPosition = penghalang.Position;
                            objek.AddComponent<CanvasGroup>();
                            objek.GetComponent<BarrierController>().IsEditMode = true;
                            DragHandler draghander = objek.AddComponent<DragHandler>();
                            draghander.dragLayer = dragLayer;
                            draghander.dragBack = dragBack;
                        }
                    }
                }
            }
        }
    }

    private void RefreshBubble(List<Gelembung> daftarGelembung)
    {
        foreach (Gelembung gelembung in daftarGelembung)
        {
            for (int i = 0; i < Bubbles.Length; i++)
            {
                if (gelembung.PackageName == Bubbles[i].PackageName)
                {
                    if (gelembung.Type == BubbleType.White)
                    {
                        GameObject objek = (GameObject)Instantiate(Bubbles[i].WhitePreview);
                        objek.transform.SetParent(ParentObject.Bubble.transform);
                        objek.transform.localScale = new Vector3(1, 1, 1);
                        objek.GetComponent<RectTransform>().localPosition = Vector3.zero;
                        objek.AddComponent<CanvasGroup>();
                    }
                    else if (gelembung.Type == BubbleType.Red)
                    {
                        GameObject objek = (GameObject)Instantiate(Bubbles[i].RedPreview);
                        objek.transform.SetParent(ParentObject.Bubble.transform);
                        objek.transform.localScale = new Vector3(1, 1, 1);
                        objek.GetComponent<RectTransform>().localPosition = Vector3.zero;
                        objek.AddComponent<CanvasGroup>();
                    }
                    else if (gelembung.Type == BubbleType.Orange)
                    {
                        GameObject objek = (GameObject)Instantiate(Bubbles[i].OrangePreview);
                        objek.transform.SetParent(ParentObject.Bubble.transform);
                        objek.transform.localScale = new Vector3(1, 1, 1);
                        objek.GetComponent<RectTransform>().localPosition = Vector3.zero;
                        objek.AddComponent<CanvasGroup>();
                    }
                }
            }
        }
    }

    public void ImportLevel() {
/*
 * #if UNITY_EDITOR || UNITY_STANDALONE
        System.Windows.Forms.OpenFileDialog File = new System.Windows.Forms.OpenFileDialog();

        if (File.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
        {
            string Path = File.FileName;
            RefreshLevel(EntityManager.Load<StaticLevel>(Path));
            validatedStatus = false;
        }
#endif
 */
    }
}
