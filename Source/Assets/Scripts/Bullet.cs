using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int speed = 20;

    Generator generator;
    Player player;
    int bounced = 0;
    Rigidbody2D rigid;

    void Start()
    {
        generator = FindObjectOfType<Generator>();
        player = FindObjectOfType<Player>();
        Destroy(gameObject, 2+(generator.score/5000));
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = transform.right * speed;
    }

    void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Border")
        {
            if (generator.level < 5)
                Destroy(gameObject);
            else if(generator.level < 10)
            {
                if (bounced >= 1)
                    Destroy(gameObject);
                else
                    Bounce(collision.transform);
            } else
            {
                if (bounced >= 3)
                    Destroy(gameObject);
                else
                    Bounce(collision.transform);
            }
        }

        if (collision.gameObject.tag == "Enemy")
        {
            AudioSource.PlayClipAtPoint(player.sts[Random.Range(0, player.sts.Count)], Vector3.zero);

            collision.gameObject.GetComponent<Animator>().SetTrigger("Die");
            collision.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            Destroy(collision.gameObject.GetComponent<Collider2D>());
            Destroy(collision.gameObject, 0.5f);

            int newScore = 1;
            if (collision.gameObject.name == "Pentagon")
                newScore = 2;
            else if (collision.gameObject.name == "Octagon")
            {
                newScore = 5;
            }
            FindObjectOfType<Generator>().score += newScore;
            FindObjectOfType<Generator>().levelScore += newScore;

            Destroy(gameObject);
        }
    }

    public void Bounce(Transform wall)
    {
        print(wall);
        bounced++;

        /*Vector2 fwd = transform.rotation * Vector2.right;
        Vector2 reflect = Vector2.Reflect(fwd, wall.right);
        Quaternion newQuate = Quaternion.LookRotation(reflect);
        transform.rotation = newQuate;*/
    }
}
