using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Unity.VisualScripting;

public class ShardsReflection : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource audioSource;

    public GameObject brokenglassPrefab;
    GameObject[] shatteredShards;
    string screenshotName;
    string screenshotPath;
    Texture2D screenshotTexture;
    RawImage glasscrackImage;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        DeleteFile();
    }
    void Update()
    {
        RandomEncounterUpdate();
    }
    void TakeScreenshot()
    {
        screenshotName = "screenshot.png";
        screenshotPath = Path.Combine(Application.persistentDataPath, screenshotName);
        DeleteFile();
        ScreenCapture.CaptureScreenshot(screenshotPath);
        print("screenshot taken.");
    }

    bool isBattleQueue = false;

    public void StartRandomEncounterFX()
    {
        TakeScreenshot();
        audioSource.Play();
        isBattleQueue = true;
    }

    void RandomEncounterUpdate()
    {
        if (File.Exists(screenshotPath) && isBattleQueue)
        {
            glasscrackImage = transform.Find("ScreenCrack").GetComponent<RawImage>();
            glasscrackImage.enabled = true;
            StartCoroutine(QueueGlass());
            isBattleQueue = false;
        }
        if(brokenglassPrefab && isBattleQueue)
        {
            Destroy(brokenglassPrefab,6.0f);
        }
    }
    IEnumerator QueueGlass()
    {
        yield return new WaitForSeconds(0.4f);
        SpawnGlass();
        DeleteFile();
    }
    void SpawnGlass()
    {
        Instantiate(brokenglassPrefab, transform);
        print("Spawning Glass");
        byte[] fileData = File.ReadAllBytes(screenshotPath);
        screenshotTexture = new Texture2D(2, 2);
        screenshotTexture.LoadImage(fileData);

        shatteredShards = GameObject.FindGameObjectsWithTag("ShardsFX");
        foreach (var shard in shatteredShards)
        {
            Renderer renderer = shard.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = new Material(Shader.Find("HDRP/Lit")); // Or use a shader that supports transparency
                material.mainTexture = screenshotTexture;
                material.mainTextureScale = new Vector2(3, 3); // 3x3 tiling
                material.SetFloat("_Metallic", 1.0f); // Metallic = 1
                material.SetFloat("_Glossiness", 1.0f); // Smoothness = 1
                renderer.material = material;
            }
        }
        foreach (var shard in shatteredShards)
        {
            Rigidbody shardRb = shard.GetComponent<Rigidbody>();
            shardRb.AddForce(Vector3.down * 500.0f, ForceMode.Acceleration); // Change the value to adjust the gravity scale
        }

        glasscrackImage.enabled = false;
    }

    void DeleteFile()
    {
        if (File.Exists(screenshotPath))
        {
            File.Delete(screenshotPath);
        }
    }


}
