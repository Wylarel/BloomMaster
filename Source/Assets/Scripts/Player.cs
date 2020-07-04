using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 direction;
    public Vector3 rotation;
    public float speed;
    public GameObject bulletPfb, bulletPfb2;
    public Transform bulletParent;
    public float bulletCountdown = 5;
    public int gunCount = 0;
    public List<GameObject> guns;
    public List<AudioClip> sts;
    public GameObject ray;
    public int bulletSpeed = 12;

    private Rigidbody2D rigid;
    private float countdown;
    private Generator generator;
    public float rayCountdown;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        generator = FindObjectOfType<Generator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //rayCountdown = 1;
        }
        if (rayCountdown > 0)
        {
            rayCountdown -= Time.deltaTime;
            ray.SetActive(true);
        }
        else
        {
            rayCountdown = 0;
            ray.SetActive(false);
        }
        rotation = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        direction = direction.normalized * Mathf.Clamp(direction.magnitude, 0, 1);

        if(countdown <= 0.7f && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.Space)))
            Shoot();
        else if (generator.level >= 2 && (countdown <= 0 && (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2) || Input.GetKeyDown(KeyCode.Space))))
            Shoot();

        if (countdown > 0)
            countdown -= Time.deltaTime*bulletCountdown;
        else if (countdown != 0)
            countdown = 0;
        var newScale = Mathf.Clamp(1 + (float)generator.score / 3000, 1, 2);
        transform.localScale = new Vector3(newScale, newScale, 0.01f);
    }

    private void FixedUpdate()
    {
        rigid.velocity = direction * Time.fixedDeltaTime * 26 * speed;
    }

    public void Shoot()
    {
        if (gunCount >= 1)
        {
            Instantiate(bulletPfb, guns[0].transform.position, guns[0].transform.rotation, bulletParent).GetComponent<Bullet>().speed = bulletSpeed;
            AudioSource.PlayClipAtPoint(sts[UnityEngine.Random.Range(0, sts.Count)], Vector3.zero);
        }
        if (gunCount >= 2 && gunCount != 3)
            Instantiate(bulletPfb, guns[1].transform.position, guns[1].transform.rotation, bulletParent).GetComponent<Bullet>().speed = bulletSpeed;
        if (gunCount == 3)
        {
            Instantiate(bulletPfb, guns[2].transform.position, guns[2].transform.rotation, bulletParent).GetComponent<Bullet>().speed = bulletSpeed / 2;
            Instantiate(bulletPfb, guns[3].transform.position, guns[3].transform.rotation, bulletParent).GetComponent<Bullet>().speed = bulletSpeed / 2;
        }
        if (gunCount >= 4)
        {
            Instantiate(bulletPfb, guns[2].transform.position, guns[2].transform.rotation, bulletParent).GetComponent<Bullet>().speed = bulletSpeed;
            Instantiate(bulletPfb, guns[3].transform.position, guns[3].transform.rotation, bulletParent).GetComponent<Bullet>().speed = bulletSpeed;
        }

        countdown = 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            AudioSource.PlayClipAtPoint(sts[UnityEngine.Random.Range(0, sts.Count)], Vector3.zero);
            collision.gameObject.GetComponent<Animator>().SetTrigger("Die");
            collision.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            Destroy(collision.gameObject, 0.5f);

            int newScore = 1;
            if (collision.gameObject.name == "Pentagon")
                newScore = 2;
            else if (collision.gameObject.name == "Octagon")
            {
                newScore = 5;
            }
            generator.score += newScore;
            generator.levelScore += newScore;
        }
    }
}
