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
   

    public float initialSpeed = 10; // Ba�lang�� h�z�
    public float acceleration = 5; // H�zlanma miktar�
    private float currentSpeed; // �u anki h�z
                                // Start is called before the first frame update

    private bool isRunning = true;
    private float elapsedTime = 0f;
    private int baseScore = 200; // Ba�lang�� puan�
    private int currentScore = 0; // Ge�ici skor


    void Start()
    {
        puan = 0;
        currentSpeed = initialSpeed; // Ba�lang�� h�z�n� ayarla
        oyunBittiText.gameObject.SetActive(false);
        hasar = 100;

    }

    // Update is called once per frame
    void Update()
    {
        puann.text = "Puan: " + puan.ToString();
        hasarText.text = string.Format("Sa�l�k: {0}", hasar.ToString());
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
                // �leri hareket i�in Z ekseninde pozitif ileri vekt�r kullan�l�r
                GetComponent<Rigidbody>().AddForce(Vector3.left * currentSpeed, ForceMode.Force);
                
                
            }

            if (!Input.GetKey(KeyCode.W))
            {
                currentSpeed -= acceleration * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.S))
            {
                // Geri hareket i�in Z ekseninde negatif geri vekt�r kullan�l�r
                GetComponent<Rigidbody>().AddForce(Vector3.left * -10, ForceMode.Force);
            }

            if (Input.GetKey(KeyCode.D))
            {
                // Sa�a d�nme i�in X ekseninde pozitif sa� vekt�r kullan�l�r
                //  transform.Translate(Vector3.left * speed * Time.deltaTime);
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.A))
            {
                // Sola d�nme i�in X ekseninde negatif sol vekt�r kullan�l�r
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                // Yukar� ok tu�una bas�ld���nda ileri hareket i�in Z ekseninde pozitif ileri vekt�r kullan�l�r
                GetComponent<Rigidbody>().AddForce(Vector3.left * currentSpeed, ForceMode.Force);
            }

            if (Input.GetKey(KeyCode.W))
            {
                // �leri hareket i�in Z ekseninde pozitif ileri vekt�r kullan�l�r
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
            // Ge�en zaman� metin ��esine aktar
            gecenZaman.text = "Ge�en Zaman: " + Mathf.Round(elapsedTime) + "s";
        }

        void UpdateScore()
        {
            // Ge�en s�reye g�re skoru hesapla
            int timeBasedScore = baseScore - Mathf.RoundToInt(elapsedTime) * 10;

            // Negatif skoru engelle
            timeBasedScore = Mathf.Max(timeBasedScore, 0);

            // Ge�ici skoru g�ncelle
            currentScore = timeBasedScore;
            score.text="Score: "+currentScore.ToString();
            

        }

        void RestartGame()
        {
            oyunbitti = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnCollisionEnter(Collision collision) //private olmay�nca �al��mad�
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
        isRunning = false; // Zamanlay�c�y� durdur
        toplamPuan = puan + currentScore;
        finalPuan.text="Final: "+toplamPuan.ToString();
        Debug.Log(toplamPuan);
    }
}