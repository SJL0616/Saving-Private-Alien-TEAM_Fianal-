using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 적 총 발사 컨트롤러
작성자: 이상준
내용:총 발사 - 라이플, 샷건 
마지막 수정일: 2022.6.19
*/
public class EnemyGunController : MonoBehaviour
{
    public GameObject gun;             // 총 게임오브젝트
    public GameObject holder;             // 총 발사 위치 게임오브젝트
    public GameObject bullet;             // 총알 게임오브젝트
    public GameObject sparks;             // 총 발포 스파크 파티클
    private int fireRate = 7;             
    public GameObject shotgunRange;       // 샷건 범위
    private BoxCollider collider;         // 샷건용 박스 콜라이더   
    private new EnemyAudioController audio; 
    private EnemyMovementContorller move;
    private Animator anim;

    //사격 모드 제어 메서드
    public void Shooting(string type, bool isShooting){
        if(isShooting){ // 사격 모드일 때
           
            switch(type){ // 총 종류에 따라 다른 총 발사
                case "M5": // 라이플 (M5)
                    move.shoot(true); //총 발사 애니메이션 시작
                    audio.PlaySound("M5"); //총 발사 음향 재생
                    GameObject bullets;  //총 클론 생성, 발사
                    holder.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles);
                    bullets = Instantiate(bullet,holder.transform.position,holder.transform.rotation) as GameObject;
                    Rigidbody rigid = bullets.GetComponent<Rigidbody>();
                    rigid.AddForce(transform.forward  * 50.0f, ForceMode.Impulse);
                    
                break;
                case "Shotgun":
                    if (fireRate % 7 == 0)
                    {   move.shoot(true);
                        StartCoroutine(PlayShotGunSound());
                        StartCoroutine(shootShotGun());
                    }
                    fireRate++;
                    break;
            }
        }
    }
    // 샷건 사격 제어 메서드
    void ShootM5(){
       anim.SetBool("Shoot_b",false);
    }
    IEnumerator shootShotGun(){ // 샷건 사격을 실행시키는 코루틴 메서드
        GameObject spark;
        gun.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles);
        holder.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles);
        //샷건 발포 시 스파크 파티클 플레이
        spark = Instantiate(sparks, holder.transform.position,holder.transform.rotation)as GameObject;
        var flashPs = spark.GetComponent<ParticleSystem>();
        if(flashPs != null){
            collider.enabled = true;
            flashPs.Play();
            Destroy(spark, flashPs.main.duration);
            yield return new WaitForSeconds(0.5f);
            collider.enabled = false;
        }
    }
    IEnumerator PlayShotGunSound()
    {
        audio.PlaySound("Shotgun");
        AudioSource audioSource = audio.GetComponent<AudioSource>();
        yield return new WaitForSeconds(0.9f);
        if (!audioSource.isPlaying)
        {
            audio.PlaySound("ShotgunReload");
            anim.SetBool("Shoot_b",false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<EnemyAudioController>();
        move = GetComponent<EnemyMovementContorller>();
        anim = GetComponent<Animator>();
        if(shotgunRange != null){
            collider = shotgunRange.GetComponent<BoxCollider>();
            collider.enabled =false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
