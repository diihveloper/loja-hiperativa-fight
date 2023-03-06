using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    FighterBase fighterBase;

    // Start is called before the first frame update
    void Start()
    {
        fighterBase = GetComponent<FighterBase>();
    }

    // Update is called once per frame
    void Update()
    {
        fighterBase.Move(Input.GetAxisRaw("Horizontal")); // obtém a entrada horizontal do jogador

        if (Input.GetButtonDown("Jump")) // verifica se o botão de pulo foi pressionado
        {
            fighterBase.Jump();
        }

        if (Input.GetButtonDown("Fire3"))
        {
            fighterBase.Punch();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            fighterBase.Kick();
        }
    }
}