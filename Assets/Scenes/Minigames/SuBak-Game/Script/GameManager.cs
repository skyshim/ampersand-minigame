//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class GameManager : MonoBehaviour {
//    [Header("게임 설정")]
//    public GameObject fruitPrefab;
//    public Transform spawnPoint;
//    public float spawnHeight = 8f;
//    public float moveSpeed = 5f;

//    [Header("스폰 범위")]
//    public float minX = -3f;
//    public float maxX = 3f;

//    [Header("게임 상태")]
//    public bool isGameOver = false;
//    public int currentScore = 0;

//    private GameObject currentFruit;
//    private Queue<int> nextFruitQueue = new Queue<int>();
//    private bool canSpawn = true;
//    private PlayerInput playerInput;

//    void Start() {
//        playerInput = GetComponent<PlayerInput>();

//        // 첫 번째 과일들을 큐에 추가
//        for (int i = 0; i < 3; i++) {
//            nextFruitQueue.Enqueue(Random.Range(1, 3)); // 1~2 레벨만 스폰
//        }

//        SpawnNextFruit();
//    }

//    void Update() {
//        if (isGameOver) return;

//        // 현재 과일 조작
//        if (currentFruit != null) {
//            HandleFruitMovement();

//            // 드롭 입력 감지
//            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
//                DropFruit();
//            }
//        }

//        // 자동 스폰 체크
//        CheckAutoSpawn();
//    }

//    void HandleFruitMovement() {
//        float horizontalInput = playerInput.GetHorizontalInput();

//        if (Mathf.Abs(horizontalInput) > 0.1f) {
//            Vector3 currentPos = currentFruit.transform.position;
//            currentPos.x += horizontalInput * moveSpeed * Time.deltaTime;
//            currentPos.x = Mathf.Clamp(currentPos.x, minX, maxX);
//            currentFruit.transform.position = currentPos;
//        }
//    }

//    void DropFruit() {
//        if (currentFruit != null) {
//            currentFruit.GetComponent<Rigidbody2D>().isKinematic = false;
//            currentFruit = null;
//            canSpawn = false;
//        }
//    }

//    void CheckAutoSpawn() {
//        if (!canSpawn || currentFruit != null) return;

//        // 모든 과일이 안정되었는지 확인
//        FruitController[] allFruits = FindObjectsOfType<FruitController>();
//        bool allStable = true;

//        foreach (FruitController fruit in allFruits) {
//            if (fruit.IsMoving()) {
//                allStable = false;
//                break;
//            }
//        }

//        if (allStable) {
//            StartCoroutine(SpawnDelay());
//        }
//    }

//    IEnumerator SpawnDelay() {
//        yield return new WaitForSeconds(0.5f); // 짧은 딜레이
//        if (!isGameOver) {
//            SpawnNextFruit();
//        }
//    }

//    void SpawnNextFruit() {
//        if (nextFruitQueue.Count == 0) return;

//        int fruitLevel = nextFruitQueue.Dequeue();
//        nextFruitQueue.Enqueue(Random.Range(1, 3)); // 새로운 과일 추가

//        Vector3 spawnPos = new Vector3(0, spawnHeight, 0);
//        currentFruit = Instantiate(fruitPrefab, spawnPos, Quaternion.identity);

//        FruitController fruitController = currentFruit.GetComponent<FruitController>();
//        fruitController.fruitLevel = fruitLevel;

//        // Kinematic으로 설정하여 드롭 전까지 물리 비활성화
//        currentFruit.GetComponent<Rigidbody2D>().isKinematic = true;

//        canSpawn = true;
//    }

//    public void AddScore(int points) {
//        currentScore += points;
//        Debug.Log($"Score: {currentScore}");
//    }

//    public void GameOver() {
//        isGameOver = true;
//        Debug.Log("Game Over!");

//        // 모든 과일의 물리 정지
//        FruitController[] allFruits = FindObjectsOfType<FruitController>();
//        foreach (FruitController fruit in allFruits) {
//            fruit.GetComponent<Rigidbody2D>().isKinematic = true;
//        }
//    }
//}
