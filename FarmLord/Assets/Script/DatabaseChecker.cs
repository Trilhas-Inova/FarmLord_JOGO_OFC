using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DatabaseChecker : MonoBehaviour
{
    private const string URL = "https://www.weeklyhow.com";//Mudar para o link/endpoint correto do backend
    private const string API_KEY = "ENTER_YOUR_API_KEY_HERE"; //Se necess�rio. Depende se o back end est� usando

    [SerializeField]
    private TMP_InputField m_email;
    [SerializeField]
    private TMP_InputField m_senha;

    public void ChecarCadastro()
    {
        SceneManager.LoadScene("GameplayScene"); ;
    }

}
