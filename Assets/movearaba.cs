using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class movearaba : MonoBehaviour
{
    public float speed = 1000;
    public bool oyunbitti = false;
    public float puan = 0;
    double x;
    public TextMeshProUGUI oyunBittiText;
    public TextMeshProUGUI hasarText;
    public TextMeshProUGUI gecenZaman;
    public TextMeshProUGUI score;
    public TextMeshProUGUI finalPuan;
    public TextMeshProUGUI puann;
    public int hasar;
    public float toplamPuan;
   

    public float initialSpeed = 10; // Baþlangýç hýzý
    public float acceleration = 5; // Hýzlanma miktarý
    private float currentSpeed; // Þu anki hýz
                                // Start is called before the first frame update

    private bool isRunning = true;
    private float elapsedTime = 0f;
    private int baseScore = 200; // Baþlangýç puaný
    private int currentScore = 0; // Geçici skor


    void Start()
    {
        puan = 0;
        currentSpeed = initialSpeed; // Baþlangýç hýzýný ayarla
        oyunBittiText.gameObject.SetActive(false);
        hasar = 100;

    }

    // Update is called once per frame
    void Update()
    {
        puann.text = "Puan: " + puan.ToString();
        hasarText.text = string.Format("Saðlýk: {0}", hasar.ToString());
        currentSpeed += acceleration * Time.deltaTime;

        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerUI();
            UpdateScore();
        }


        if (oyunbitti==false)
        {
            if (Input.GetKey(KeyCode.W))
            {
                // Ýleri hareket için Z ekseninde pozitif ileri vektör kullanýlýr
                GetComponent<Rigidbody>().AddForce(Vector3.left * currentSpeed, ForceMode.Force);
                
                
            }

            if (!Input.GetKey(KeyCode.W))
            {
                currentSpeed -= acceleration * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.S))
            {
                // Geri hareket için Z ekseninde negatif geri vektör kullanýlýr
                GetComponent<Rigidbody>().AddForce(Vector3.left * -10, ForceMode.Force);
            }

            if (Input.GetKey(KeyCode.D))
            {
                // Saða dönme için X ekseninde pozitif sað vektör kullanýlýr
                //  transform.Translate(Vector3.left * speed * Time.deltaTime);
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.A))
            {
                // Sola dönme için X ekseninde negatif sol vektör kullanýlýr
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                // Yukarý ok tuþuna basýldýðýnda ileri hareket için Z ekseninde pozitif ileri vektör kullanýlýr
                GetComponent<Rigidbody>().AddForce(Vector3.left * currentSpeed, ForceMode.Force);
            }

            if (Input.GetKey(KeyCode.W))
            {
                // Ýleri hareket için Z ekseninde pozitif ileri vektör kullanýlýr
                GetComponent<Rigidbody>().AddForce(Vector3.left * currentSpeed, ForceMode.Force);


            }


        }
        else if (oyunbitti==true)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        if(GetComponent<Rigidbody>().position.x < -205)
        {
            toplamPuan = puan + currentScore;
            finalPuan.text = "Final: " + toplamPuan.ToString();
            Debug.Log(finalPuan);
            oyunbitti = true;
            isRunning = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            Invoke("RestartGame", 3f);
            oyunBittiText.gameObject.SetActive(true);
        }

        else if (GetComponent<Rigidbody>().position.x>184 || GetComponent<Rigidbody>().position.z>13.5 || GetComponent<Rigidbody>().position.z<-13.5 || puan<0)
        {
            //toplamPuan = puan + currentScore;
            currentScore = 0;
            toplamPuan = puan + currentScore;
            finalPuan.text = "Final: " + toplamPuan.ToString();
            Debug.Log(finalPuan);
            oyunbitti = true;
            isRunning = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            Invoke("RestartGame", 3f);
            oyunBittiText.gameObject.SetActive(true);
            
        }

        void UpdateTimerUI()
        {
            // Geçen zamaný metin öðesine aktar
            gecenZaman.text = "Geçen Zaman: " + Mathf.Round(elapsedTime) + "s";
        }

        void UpdateScore()
        {
            // Geçen süreye göre skoru hesapla
            int timeBasedScore = baseScore - Mathf.RoundToInt(elapsedTime) * 10;

            // Negatif skoru engelle
            timeBasedScore = Mathf.Max(timeBasedScore, 0);

            // Geçici skoru güncelle
            currentScore = timeBasedScore;
            score.text="Score: "+currentScore.ToString();
            

        }

        void RestartGame()
        {
            oyunbitti = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnCollisionEnter(Collision collision) //private olmayýnca çalýþmadý
    {
        if (collision.collider.CompareTag("engel"))
        {
            currentScore = 0;
            toplamPuan = puan + currentScore;
            finalPuan.text = "Final: " + toplamPuan.ToString();
            Debug.Log(finalPuan);
            oyunbitti = true;
            isRunning = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            Invoke("RestartGame", 3f);
            oyunBittiText.gameObject.SetActive(true);
        }

        if (collision.collider.tag == "coin")
        {
            puan+=10;
            Destroy(collision.gameObject);
            
        }

        if(collision.collider.tag == "zorcoin")
        {
            puan += 50;
            Destroy(collision.gameObject);
            
        }

        if (collision.collider.tag == "dur")
        {
            puan -= 10;
            currentSpeed = 0;
            hasar = hasar - 25;
        }

        if (collision.collider.tag == "hasarcan")
        {
            if (hasar < 100) 
            {
                hasar = hasar + 25;
            }
        }


        if (hasar<=0)
        {
            Invoke("RestartGame", 3f);
            oyunBittiText.gameObject.SetActive(true);
        }

       
    }
    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        oyunbitti = false;
        isRunning = false;
        finalPuan.text = "Final: " + toplamPuan.ToString();
        Debug.Log(toplamPuan);
    }

    void EndGame()
    {
        isRunning = false; // Zamanlayýcýyý durdur
        toplamPuan = puan + currentScore;
        finalPuan.text="Final: "+toplamPuan.ToString();
        Debug.Log(toplamPuan);
    }
}