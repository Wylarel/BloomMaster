using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Generator : MonoBehaviour
{
    public int mobCap;
    public int mapSize;
    public int score;
    public int level;
    public int levelScore;
    public List<int> levelScores = new List<int>();

    public List<int> chances = new List<int>();
    public List<GameObject> prefabs = new List<GameObject>();
    public List<Transform> borders = new List<Transform>();
    public Camera cam, minicam;

    public Text score1;
    public Text score2;
    public Text score3;
    public Slider slider;

    public GameObject gun, map, middleBlock;

    public AudioClip messageSound;

    private Message message;
    private Player player;

    void Start()
    {
        message = FindObjectOfType<Message>();
        player = FindObjectOfType<Player>();
        message.Pop("Welcome", "Use WASD or the\nArrows to move\nyour player");
    }

    void Update()
    {
        score1.text = levelScore.ToString() + "/" + levelScores[level];
        mapSize = score/40 + 10;
        if (level < 1) mobCap = 1;
        if (level >= 1) mobCap = Mathf.RoundToInt((mapSize ^ 2) / 1.5f);
        if (level >= 5) mobCap = (mapSize ^ 2);
        if (level >= 7) mobCap = Mathf.RoundToInt((mapSize ^ 2) * 1.1f);
        if (level >= 11) mobCap = Mathf.RoundToInt((mapSize ^ 2) * 3.2f);
        slider.value = Mathf.Lerp(slider.value, (float)levelScore / (float)levelScores[level], 0.2f);
        score2.text = level.ToString();
        score3.text = (level + 1).ToString();

        if(levelScore >= levelScores[level])
        {
            levelScore -= levelScores[level];
            level++;
            AudioSource.PlayClipAtPoint(messageSound, Vector3.zero);
            switch (level)
            {
                case 1:
                    message.Pop("Good Start!", "You can now\nshoot the targets\nwith your mouse");
                    player.gunCount = 1;
                    player.guns[0].SetActive(true);
                    break;
                case 2:
                    message.Pop("Machine Gun", "Hold left click\nto keep sending\nbullets");
                    gun.SetActive(true);
                    break;
                case 3:
                    message.Pop("Pentagons", "These new\ntargets give\nyou 2 points\nwhen killed");
                    break;
                case 4:
                    player.speed = 11;
                    message.Pop("Faster", "You are now\nmoving faster ");
                    break;
                case 5:
                    message.Pop("Bounce Back!", "Your bullets can\nnow bounce\nonce when they\nhit the border");
                    break;
                case 6:
                    message.Pop("Octogon", "These new\ntargets give\nyou 5 points\nwhen killed");
                    break;
                case 7:
                    player.bulletPfb = player.bulletPfb2;
                    player.bulletSpeed = 20;
                    message.Pop("Big fast bullets", "For a\nbig fat\nboy");
                    break;
                case 8:
                    message.Pop("Two guns", "To do twice as\nmuch damage");
                    player.gunCount = 2;
                    player.guns[1].SetActive(true);
                    break;
                case 9:
                    player.bulletCountdown = 9;
                    message.Pop("Always more", "You can now\nshoot bullets\neven faster");
                    break;
                case 10:
                    map.SetActive(true);
                    message.Pop("Where are you?", "The map is 144\ntimes bigger than\n when you started");
                    break;
                case 11:
                    message.Pop("Laser beam", "Enjoy it while\nit lasts");
                    player.rayCountdown = 1200;
                    break;
                case 12:
                    player.rayCountdown = 0;
                    player.speed = 13;
                    message.Pop("Three guns", "Just...\n\nEnjoy! <3");
                    player.gunCount = 3;
                    player.guns[1].SetActive(false);
                    player.guns[2].SetActive(true);
                    player.guns[3].SetActive(true);
                    break;
                case 13:
                    message.Pop("Donut Shape", "You should move\naway from\nthe middle");
                    middleBlock.SetActive(true);
                    break;
                case 14:
                    message.Pop("Four guns", "In case\nyou haven't\nhad enough.");
                    player.gunCount = 4;
                    player.guns[1].SetActive(true);

                    break;
                default:
                    message.Pop("The end", "For now!\nMore content\ncoming soon");
                    break;
            }
        }

        if (transform.childCount < mobCap)
        {
            var val = Random.value;
            var type = prefabs[0];
            if (level >= 3) type = prefabs[Random.Range(0, 2)];
            if (level >= 6) type = prefabs[Random.Range(0, prefabs.Count)];

            Instantiate
                (
                type, 
                new Vector3(Random.Range((float)-mapSize/2+1, mapSize/2-1), Random.Range((float)-mapSize/2+1, mapSize/2-1), Random.Range(0,0.01f)), 
                Quaternion.Euler(0,0,Random.Range(0f,360f)), 
                transform
                ).name = type.name;
        }
        borders[0].transform.position = new Vector3(Mathf.Lerp(borders[0].position.x, -25 - ((float)mapSize / 2), 0.1f), 0, 0.01f);
        borders[1].transform.position = new Vector3(Mathf.Lerp(borders[1].position.x, +25 + ((float)mapSize / 2), 0.1f), 0, 0.01f);
        borders[2].transform.position = new Vector3(0, Mathf.Lerp(borders[2].position.y, +25 + ((float)mapSize / 2), 0.1f), 0.01f);
        borders[3].transform.position = new Vector3(0, Mathf.Lerp(borders[3].position.y, -25 - ((float)mapSize / 2), 0.1f), 0.01f);

        if(mapSize < 100)
            cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, Mathf.Lerp(cam.transform.position.z, Mathf.Clamp(-9-level, -30, -10), 0.01f));
        minicam.orthographicSize = Mathf.Lerp(minicam.orthographicSize, mapSize/1.9f, 0.01f);
    }
}
