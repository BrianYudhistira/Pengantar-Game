using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float move = -20;
    public float maxRotate = 60; // Batasan maksimum rotasi.
    private float vertical;
    private float timeSinceLastInput; // Menyimpan waktu sejak input terakhir.

    public float inputCooldown = 0.5f; // Waktu jeda antara input.
    public float minRotationZ = -60; // Batasan rotasi Z minimum.
    public float maxRotationZ = 60;  // Batasan rotasi Z maksimum.

    public float minX = -160; // Batasan translasi X minimum.
    public float maxX = 224;  // Batasan translasi X maksimum.
    public float minY = 31;  // Batasan translasi Y minimum.
    public float maxY = 466;  // Batasan translasi Y maksimum.

    private float currentRotationZ = 0;
    private bool reverseMovement = false;

    private bool isRotating = false;
    private float currentRotationY = 0;
    private float targetRotationY = 0;
    private float rotationSpeed = 90.0f;
        // Start is called before the first frame update
        void Start()
        {
            timeSinceLastInput = Time.time; // Inisialisasi waktu terakhir input pada awal permainan.
        }

        // Update is called once per frame
        void Update()
        {
            vertical = Input.GetAxis("Vertical");
            // Periksa apakah tombol "V" ditekan.
            if (Input.GetKey(KeyCode.V))
            {
                // Pastikan sudah berlalu cukup waktu sejak input terakhir.
                if (Time.time - timeSinceLastInput >= inputCooldown && !isRotating)
                {
                    // Tambahkan rotasi 180 derajat pada sumbu Y saat tombol "V" ditekan dengan animasi.
                    targetRotationY += 180;
                    timeSinceLastInput = Time.time; // Perbarui waktu terakhir input.
                    move = -move;
                    reverseMovement = !reverseMovement;
                    StartCoroutine(RotateToTarget());
                }
            }

            // Hitung perubahan rotasi berdasarkan waktu.
            float deltaRotationZ = -Time.deltaTime * vertical * maxRotate; // Perubahan rotasi Z
            currentRotationZ += deltaRotationZ;

            // Batasi nilai rotasi Z antara -60 dan 60.
            currentRotationZ = Mathf.Clamp(currentRotationZ, minRotationZ, maxRotationZ);

            // Terapkan rotasi ke objek.
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, currentRotationZ);

            // Periksa apakah tombol spasi ditekan.
            if (Input.GetKey(KeyCode.Space))
            {
                // Hitung perubahan translasi berdasarkan sudut rotasi Z saat ini.
                float deltaX = Mathf.Cos(Mathf.Deg2Rad * currentRotationZ) * move * Time.deltaTime;
                float deltaY = Mathf.Sin(Mathf.Deg2Rad * currentRotationZ) * move * Time.deltaTime;

                if (reverseMovement)
                {
                    deltaX = Mathf.Cos(Mathf.Deg2Rad * -currentRotationZ) * move * Time.deltaTime;
                    deltaY = Mathf.Sin(Mathf.Deg2Rad * -currentRotationZ) * move * Time.deltaTime;
                }

                // Terapkan batasan nilai maksimum dan minimum pada translasi X dan Y.
                Vector3 currentPosition = transform.position;
                currentPosition.x = Mathf.Clamp(currentPosition.x + deltaX, minX, maxX);
                currentPosition.y = Mathf.Clamp(currentPosition.y + deltaY, minY, maxY);
                transform.position = currentPosition;
            }
            IEnumerator RotateToTarget()
            {
                isRotating = true;
                float startRotationY = currentRotationY;
                float elapsedTime = 0;

                while (elapsedTime < inputCooldown)
                {
                    currentRotationY = Mathf.Lerp(startRotationY, targetRotationY, elapsedTime / inputCooldown);
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, currentRotationY, currentRotationZ);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                currentRotationY = targetRotationY;
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, currentRotationY, currentRotationZ);
                isRotating = false;
            }
        }
    }
