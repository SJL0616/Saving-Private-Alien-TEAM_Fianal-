using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
게임 매니저 클래스
작성자: 이상준
내용: 게임 시작 / 재시작 / 종료 메서드
마지막 수정일: 2022.6.19
*/
public class GameManager : MonoBehaviour
{
    public static int scenesNum = 0;            //씬 넘버 분류용 static 변수
    public static int currentScore = 0;         //현재 점수 저장용 static 변수
    public static int yourMaxScore = 0;         //누적 점수 저장용 static 변수
    public static bool isRestart = false;       //재시작인지 판단용 static 변수
    public static bool isStageRe = false;   
    
    public GameObject menuCam;                  // 메뉴 카메라 게임 오브젝트
    public GameObject gameCam;                  // 게임 카메라 게임 오브젝트
    public GameObject player;
    public GameObject playerHeathImage;
    private GameObject[] enemyArr;              //전체 Enemy 오브젝트 배열
    public int enemyCount = 0;
    public bool playerIsDead = false;           //플레이어가 죽었으면 true가 됨
    public bool gameEnd = false;               
    public GameObject menuPanel;                //메뉴, 인게임, ESC 용 판넬
    public GameObject gamePanel;
    public GameObject endPanel;
    public GameObject escPanel;
    public Text maxScoreTxt;                    //누적점수, 현재점수 저장용 Text
    public Text endMaxScoreTxt;
    public Text scorTxt;
    public Text stageTxt;
    public Text playerHealthTxt;

    // 플레이 타임 UI (미구현)
    // public Text playTimeTxt;
    // public Text playerAmmoTxt;
    // public Text playerCoinTxt;
    // public float playTime;      
    void Awake()
    {   
        scenesNum++;
        if(scenesNum > 1){  //게임 씬 설정
          GameStart();
        }
        maxScoreTxt.text = string.Format("{0:n0}", yourMaxScore );   
    }

    //게임 시작 스크립트
    public void GameStart() 
    { 
      if(isRestart){ // 로비로 돌아갈시
          Debug.Log("isRestart");
          Time.timeScale = 1;
          gameCam.SetActive(false);
          menuCam.SetActive(true);
          gamePanel.SetActive(true);
          isRestart = false;
      }else if(isStageRe){ // 게임 재시작시
          Debug.Log("isStageRe");
          Time.timeScale = 1;
          gameCam.SetActive(true);
          menuPanel.SetActive(false);
          menuCam.SetActive(false);
          gamePanel.SetActive(true);
          isStageRe = false;


      }else{ // 첫 게임 시작시
          Debug.Log("GameStart");    
          if(menuCam != null){
            menuCam.SetActive(false);
          }
          gameCam.SetActive(true);
          menuPanel.SetActive(false);
          gamePanel.SetActive(true);
          endPanel.SetActive(false);
          escPanel.SetActive(false);
          player.gameObject.SetActive(true);
       }

      // 적 숫자 저장용 배열에 저장
      enemyArr = GameObject.FindGameObjectsWithTag("Enemy");
      enemyCount = enemyArr.Length;
      //게임 시작/종료 판단용 코루틴 시작
      StartCoroutine("GameOnOff", 0.2f);
    }

    //이하는 버튼 클릭 메서드
    
    //다음 스테이지로 이동 버튼 클릭시 실행 메서드
    public void NextStage(){ 
      SceneManager.LoadScene("Final 2");
    }
    //게임(스테이지) 재시작버튼 클릭시 실행  메서드
    public void Restart(){
      isStageRe = true;
      currentScore = 0;
      scenesNum = 0;
      Time.timeScale = 1;
        gameEnd = false;
        playerIsDead = false;
      SceneManager.LoadScene("Final 1");
    }

    //게임 계속하기 실행(ESC 메뉴에서 계속하기 클릭) 메서드
    public void Continue(){
      Debug.Log("Continue");
      escPanel.SetActive(false);
      Time.timeScale = 1;
    }
    //게임 종료 실행 메서드
    public void Quit(){
      Application.Quit();
    }

    //게임 클리어 / 게임 오버 판단용 코루틴
    IEnumerator GameOnOff(float dealy){
      while (true){
        yield return new WaitForSeconds(dealy);

        //적이 모두 죽었다면 게임 클리어
        if(enemyCount <= 0 && !gameEnd){ 
          gameEnd = true;
          GameClear();
        }
        //플레이어가 죽었다면 게임 오버
        if(playerIsDead && !gameEnd){
          gameEnd = true;Debug.Log("GameEnd");
          GameOver();
        }
      }
    }

    //게임 클리어 메서드
    public void GameClear(){
      gamePanel.SetActive(false);
      endPanel.transform.GetChild(1).gameObject.SetActive(true);
      endPanel.transform.GetChild(0).gameObject.SetActive(false); // 계속하기 버튼
      endMaxScoreTxt.text = string.Format("{0:n0}", yourMaxScore );   
      endPanel.SetActive(true);
      if(scenesNum > 1){
        gameCam.GetComponent<Follow>().gameSet("WholeWin");
      }else{
        gameCam.GetComponent<Follow>().gameSet("Win");
      }
      
    }

    //게임 오버 메서드
    public void GameOver(){
      gamePanel.SetActive(false);
      endPanel.transform.GetChild(1).gameObject.SetActive(false);
      endPanel.transform.GetChild(0).gameObject.SetActive(true); // 다시하기 버튼
      endMaxScoreTxt.text = string.Format("{0:n0}", yourMaxScore );  
      endPanel.SetActive(true);
      currentScore = 0;
      scenesNum = 0;
      gameCam.GetComponent<Follow>().gameSet("Lose");
    }

    void LateUpdate()
    {
        
        stageTxt.text = "STAGE" + scenesNum;
        scorTxt.text = string.Format("{0:n0}",currentScore);
        //좌측 상단 체력바 설정
        playerHealthTxt.text = player.GetComponent<AlienController>().HP + " / 100";
        int currentInt = 100 - player.GetComponent<AlienController>().HP;
        playerHeathImage.transform.localScale = new Vector3(1-(currentInt*0.01f),1,1);

        // ESC 누를시 메뉴 UI표시, 시간 정지
        if(Input.GetKeyDown(KeyCode.Escape)){
          Time.timeScale = 0;
          escPanel.SetActive(true);
        }

        if(currentScore >= yourMaxScore){
         yourMaxScore = currentScore;
        }
    }
}
