using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public enum AnimalType
{
    Eagle,
    Crocodile,
    Tamaraw,
    Turtle
}
public enum GameState
{
    Title,
    InGame,
    GameOver
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private Transform canvas;
    [SerializeField]
    private GameObject popupReceiveAnimal, popupGameOver, popupTrivia, popupProfile, popupQuiz, popupCorrect, popupWrong, popupReport, popupReportNotification, popupLeaderboard, popupSocial, titleScreen;
    [SerializeField]
    private TextMeshProUGUI textReceiveAnimal, textFunds, textRangerCost, textAnimalsReleased, textTrivia, textReceivedFunds;
    [SerializeField]
    private TextMeshProUGUI textProfileName, textProfileStatus, textProfileThreats;
    [SerializeField]
    private TextMeshProUGUI textQuestion, textOpt1, textOpt2, textOpt3, textQuizReceivedFunds;
    [SerializeField]
    private Image imageProfile;
    [SerializeField]
    private Sprite sprEagle, sprCrocodile, sprTamaraw, sprTurtle;
    [SerializeField]
    private GameObject prefabRanger, prefabPoacher, prefabPoacherAlert, prefabAnimalRelease;
    [SerializeField]
    private GameObject prefabEagle, prefabCrocodile, prefabTurtle, prefabTamaraw;
    private List<Animal> animals = new List<Animal>();
    private List<Poacher> poachers = new List<Poacher>();
    private List<Ranger> rangers = new List<Ranger>();

    private Coroutine CReceiveAnimal, CExpenses, CSpawnPoacher;
    private Queue<AnimalType> queueReceivedAnimals = new Queue<AnimalType>();
    private GameState gameState;

    public int funds, rangerCost, expenses, animalsReleased;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }
    private void Start()
    {
        Time.timeScale = 1;
        titleScreen.gameObject.SetActive(true);
        gameState = GameState.Title;
    }
    private void StartGame()
    {
        titleScreen.gameObject.SetActive(false);
        gameState = GameState.InGame;

        CReceiveAnimal = StartCoroutine(IEReceiveAnimal());
        CExpenses = StartCoroutine(IEExpenses());
        CSpawnPoacher = StartCoroutine(IESpawnPoacher());
        funds = 100000;
        rangerCost = 10000;
        expenses = 1000;
        animalsReleased = 0;
        textFunds.text = funds.ToString("n0") + "<color=\"red\">(-" + expenses.ToString("n0") + ")";
        textRangerCost.text = "<sprite=0> " + rangerCost.ToString("n0");
        textAnimalsReleased.text = animalsReleased.ToString();
    }

    private string[] textsReceiveAnimal = {
        " was confiscated from illegal activities.",
        " was retrieved from improper domestication.",
        " was donated by its previous owners.",
        " was rescued for nursing."
    };
    private IEnumerator IEReceiveAnimal()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        if (Random.value > 0.5f)
            queueReceivedAnimals.Enqueue((AnimalType)Random.Range(0, System.Enum.GetValues(typeof(AnimalType)).Length));
        CReceiveAnimal = StartCoroutine(IEReceiveAnimal());
    }
    private IEnumerator IEExpenses()
    {
        yield return new WaitForSeconds(1f);
        funds -= expenses;
        textFunds.text = funds.ToString("n0") + "<color=\"red\">(-" + expenses.ToString("n0") + ")";
        CExpenses = StartCoroutine(IEExpenses());
    }
    private IEnumerator IESpawnPoacher()
    {
        yield return new WaitForSeconds(Random.Range(2f, 6f));
        Instantiate(prefabPoacher, RandomCircle(Vector3.zero, 5.5f), Quaternion.Euler(0, 0, 0), MapManager.Instance.GetMainField());
        CSpawnPoacher = StartCoroutine(IESpawnPoacher());
    }
    private void Update()
    {
        if (gameState == GameState.Title)
        {
            if (Input.GetMouseButton(0))
                StartGame();
        }
        else if (gameState == GameState.InGame)
        {
            if (!popupReceiveAnimal.activeSelf && queueReceivedAnimals.Count > 0)
                PopupReceiveAnimal(queueReceivedAnimals.Peek());
            if (funds <= 0)
                GameOver();
        }
    }

    public void PopupReceiveAnimal(AnimalType at)
    {
        popupReceiveAnimal.SetActive(true);
        string animalName = "";
        switch (at)
        {
            case AnimalType.Eagle:
                animalName = "Eagle";
                break;
            case AnimalType.Crocodile:
                animalName = "Crocodile";
                break;
            case AnimalType.Tamaraw:
                animalName = "Tamaraw";
                break;
            case AnimalType.Turtle:
                animalName = "Turtle";
                break;
            default:
                break;
        }
        textReceiveAnimal.text = "A new " + animalName + textsReceiveAnimal[Random.Range(0, textsReceiveAnimal.Length)];
    }
    public void CloseReceiveAnimal()
    {
        popupReceiveAnimal.SetActive(false);
        GameObject prefab = null;
        if (queueReceivedAnimals.Count > 0)
        {
            switch (queueReceivedAnimals.Peek())
            {
                case AnimalType.Eagle:
                    prefab = prefabEagle;
                    break;
                case AnimalType.Crocodile:
                    prefab = prefabCrocodile;
                    break;
                case AnimalType.Tamaraw:
                    prefab = prefabTamaraw;
                    break;
                case AnimalType.Turtle:
                    prefab = prefabTurtle;
                    break;
                default:
                    break;
            }
            animals.Add(Instantiate(prefab, new Vector3(Random.Range(-4f, 4f), 0f, Random.Range(-4f, 4f)), Quaternion.Euler(0f, 0f, 0f), MapManager.Instance.GetMainField()).GetComponent<Animal>());
            queueReceivedAnimals.Dequeue();
        }
    }

    private Vector3 RandomCircle(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        //pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;
    }

    public Transform GetClosestAnimal(Vector3 fromPoint)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Animal a in animals)
        {
            Transform potentialTarget = a.transform;
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }
    public Transform GetClosestRanger(Vector3 fromPoint)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Ranger a in rangers)
        {
            if (!a.IsApprehending())
            {
                Transform potentialTarget = a.transform;
                Vector3 directionToTarget = potentialTarget.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;
                }
            }
        }
        return bestTarget;
    }
    public void RemoveAnimal(Animal a)
    {
        if (animals.Contains(a))
            animals.Remove(a);
    }
    public void RemovePoacher(Poacher p)
    {
        if (poachers.Contains(p))
            poachers.Remove(p);
    }
    public void HireRanger()
    {
        if (funds >= rangerCost)
        {
            rangers.Add(Instantiate(prefabRanger, new Vector3(Random.Range(-4f, 4f), 0, Random.Range(-4f, 4f)), Quaternion.Euler(0, 0, 0), MapManager.Instance.GetMainField()).GetComponent<Ranger>());
            funds -= rangerCost;
            rangerCost = Mathf.RoundToInt(rangerCost + rangerCost * Random.Range(0f, 0.5f));
            expenses = Mathf.RoundToInt(expenses + expenses * Random.Range(0f, 0.5f));
            textFunds.text = funds.ToString("n0") + "<color=\"red\">(-" + expenses.ToString("n0") + ")";
            textRangerCost.text = "<sprite=0> " + rangerCost.ToString("n0");
        }
    }
    public PoacherAlert CreatePoacherAlert()
    {
        return Instantiate(prefabPoacherAlert, canvas).GetComponent<PoacherAlert>();
    }
    public AnimalRelease CreateAnimalRelease()
    {
        return Instantiate(prefabAnimalRelease, canvas).GetComponent<AnimalRelease>();
    }
    public void ReleaseAnimal()
    {
        animalsReleased++;
        textAnimalsReleased.text = animalsReleased.ToString();
    }
    public void GameOver()
    {
        Time.timeScale = 0;
        gameState = GameState.GameOver;
        popupGameOver.SetActive(true);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OpenTrivia()
    {
        Time.timeScale = 0;
        popupQuiz.SetActive(false);
        popupTrivia.SetActive(true);
        int receivedFunds = Random.Range(10000, 20000);

        string[] trivia =
        {
            "the Philippine eagle is monogamous and loyal.",
            "the Philippine eagle doesn’t usually eat monkeys despite being called the monkey-eating eagle",
            "tamaraw cannot lie about their age. Their horn’s length and thickness can be used to age them.",
            "the tamaraw is the largest mammal found in the Philippines.",
            "green turtles nurture the growth of sea grass which helps curb the effects of climate change.",
            "green turtles are in fact named for the greenish colour of their cartilage and fat.",
            "the Philippine crocodile is considered to be the most threatened crocodile species in the world.",
            "the International Union for the Conservation of Nature and Natural Resources (IUCN), has declared 418 animal species in the Philippines to be threatened."
        };

        textTrivia.text = "An impressed investor funded you because of a conference where you mentioned <b><i>" + trivia[Random.Range(0, trivia.Length)] + "</i></b>";
        textReceivedFunds.text = "<sprite=0> " + receivedFunds.ToString("n0");
        funds += receivedFunds;
        textFunds.text = funds.ToString("n0") + "<color=\"red\">(-" + expenses.ToString("n0") + ")";
    }
    public void CloseTrivia()
    {
        Time.timeScale = 1;
        popupTrivia.SetActive(false);
    }
    public void OpenQuiz()
    {
        Time.timeScale = 0;
        popupTrivia.SetActive(false);
        popupQuiz.SetActive(true);
        int quizIndex = Random.Range(0, 8);
        switch (quizIndex)
        {
            case 0:
                {
                    textQuestion.text = "Which is the most severely threatened crocodile species in the world?";
                    textOpt1.text = "Philippine Saltwater Crocodile";
                    textOpt2.text = "Philippine Freshwater Crocodile";
                    textOpt3.text = "Philippine Dirtywater Crocodile";
                }
                break;
            case 1:
                {
                    textQuestion.text = "What is the illegal taking or killing of wildlife, in violation of local, state, federal, or international law?";
                    textOpt1.text = "Smuggling";
                    textOpt2.text = "Poaching";
                    textOpt3.text = "Trafficking";
                }
                break;
            case 2:
                {
                    textQuestion.text = "Which is the topmost critically endangered species in the Philippines?";
                    textOpt1.text = "Tamaraw";
                    textOpt2.text = "Philippine Eagle";
                    textOpt3.text = "Visayan Warty Pig";
                }
                break;
            case 3:
                {
                    textQuestion.text = "Who are the eyes and ears of the forest that protects wildlife?";
                    textOpt1.text = "Naturalists";
                    textOpt2.text = "Rangers";
                    textOpt3.text = "Poachers";
                }
                break;
            case 4:
                {
                    textQuestion.text = "What means you will no longer see certain live species of animals due to illegal wildlife activities?";
                    textOpt1.text = "Poaching";
                    textOpt2.text = "Smuggling";
                    textOpt3.text = "Trafficking";
                }
                break;
            case 5:
                {
                    textQuestion.text = "What human activity primarily results to endangering the population of green sea turtles?";
                    textOpt1.text = "Marine Debris";
                    textOpt2.text = "Commercial Fishing";
                    textOpt3.text = "Oil Spills";
                }
                break;
            case 6:
                {
                    textQuestion.text = "Which is the national bird of the Philippines?";
                    textOpt1.text = "Bukarot";
                    textOpt2.text = "Haribon";
                    textOpt3.text = "Pawikan";
                }
                break;
            case 7:
                {
                    textQuestion.text = "Which is a government-administered centre that protects marine turtles with educational programs?";
                    textOpt1.text = "Haribon Foundation";
                    textOpt2.text = "Pawikan Conservation Centre";
                    textOpt3.text = "Tamaraw Conservation Project";
                }
                break;
            default:
                break;
        }
    }
    public void OpenProfile(AnimalType at)
    {
        popupProfile.SetActive(true);
        switch (at)
        {
            case AnimalType.Eagle:
                textProfileName.text = "Phil. Eagle (Haribon)";
                textProfileStatus.text = "Critically Endangered";
                textProfileThreats.text = "Deforestation, Mining, Pollution";
                imageProfile.sprite = sprEagle;
                break;
            case AnimalType.Crocodile:
                textProfileName.text = "Phil. Freshwater Crocodile (Bukarot)";
                textProfileStatus.text = "Critically Endangered";
                textProfileThreats.text = "Exploitation, Deforestation, Hunting";
                imageProfile.sprite = sprCrocodile;
                break;
            case AnimalType.Tamaraw:
                textProfileName.text = "Mindoro Dwarf Buffalo (Tamaraw)";
                textProfileStatus.text = "Critically Endangered";
                textProfileThreats.text = "Hunting, Deforestation, Land Clearing";
                imageProfile.sprite = sprTamaraw;
                break;
            case AnimalType.Turtle:
                textProfileName.text = "Green Sea Turtle (Pawikan)";
                textProfileStatus.text = "Critically Endangered";
                textProfileThreats.text = "Fishing, Pollution, Habitat Destruction";
                imageProfile.sprite = sprTurtle;
                break;
            default:
                break;
        }
    }
    public void CloseProfile()
    {
        popupProfile.SetActive(false);
    }
    public void EarnFunds()
    {
        if (Random.value > 0.5f)
            OpenTrivia();
        else
            OpenQuiz();
    }
    public void QuizCorrect()
    {
        popupQuiz.SetActive(false);
        popupCorrect.SetActive(true);
        int receivedFunds = Random.Range(20000, 40000);
        textQuizReceivedFunds.text = "<sprite=0> " + receivedFunds.ToString("n0");
        funds += receivedFunds;
        textFunds.text = funds.ToString("n0") + "<color=\"red\">(-" + expenses.ToString("n0") + ")";
    }
    public void QuizWrong()
    {
        popupQuiz.SetActive(false);
        popupWrong.SetActive(true);
    }
    public void CloseQuizCorrect()
    {
        Time.timeScale = 1;
        popupCorrect.SetActive(false);
    }
    public void CloseQuizWrong()
    {
        Time.timeScale = 1;
        popupWrong.SetActive(false);
    }
    public void OpenReport()
    {
        Time.timeScale = 0;
        popupReport.SetActive(true);
    }
    public void CloseReport()
    {
        Time.timeScale = 1;
        popupReport.SetActive(false);
    }
    public void ReportConfirmation()
    {
        popupReport.SetActive(false);
        popupReportNotification.SetActive(true);
    }
    public void CloseReportConfirmation()
    {
        Time.timeScale = 1;
        popupReportNotification.SetActive(false);
    }
    public void OpenLeaderboard()
    {
        Time.timeScale = 0;
        popupLeaderboard.SetActive(true);
    }
    public void CloseLeaderboard()
    {
        Time.timeScale = 1;
        popupLeaderboard.SetActive(false);
    }
    public void OpenSocial()
    {
        Time.timeScale = 0;
        popupSocial.SetActive(true);
    }
    public void CloseSocial()
    {
        Time.timeScale = 1;
        popupSocial.SetActive(false);
    }
}