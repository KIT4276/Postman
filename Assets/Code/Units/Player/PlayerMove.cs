using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class PlayerMove : MonoBehaviour, ISavedProgress
{
    [SerializeField]
    private CharacterController _characterController;
    [SerializeField]
    private float _movementSpeed = 4;

    private IInputService _input;

    public void Init(IInputService input) => 
        _input = input;

    public void UpdateProgress(PlayerProgress progress) =>
        progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(), transform.position.AsVectorData());

    public void LoadProgress(PlayerProgress progress)
    {
        if (CurrentLevel() == progress.WorldData.PositionOnLevel.Level)
        {
            Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;
            if (savedPosition != null)
            {
                Warp(to: savedPosition);
            }
        }
    }

    private void Warp(Vector3Data to)
    {
        _characterController.enabled = false;
        transform.position = to.AsUnityVector().AddY(_characterController.height);
        _characterController.enabled = true;
    }

    private static string CurrentLevel() =>
        SceneManager.GetActiveScene().name;


    private void Update()
    {
        Vector3 movementVector = Vector3.zero;

        if (_input == null)
        {
            Debug.Log("οσμ-οσμ-μσσσσσμ");
            return;//βπεμεννξ
        }

        if (_input.Axis.sqrMagnitude > Constants.Epsilon)
        {
            movementVector = Camera.main.transform.TransformDirection(_input.Axis);
            movementVector.y = 0;
            movementVector.Normalize();

            transform.forward = movementVector;
        }

        movementVector += Physics.gravity;

        _characterController.Move(_movementSpeed * movementVector * Time.deltaTime);
    }
}
