using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DatabaseChecker : MonoBehaviour
{
    private const string URL = "https://api.farmlord.com.br/login"; //Mudar para o link/endpoint correto do backend
    private const string API_KEY = "ENTER_YOUR_API_KEY_HERE"; //Se necessário. Depende se o back end está usando

    [SerializeField]
    private TMP_InputField m_email;
    [SerializeField]
    private TMP_InputField m_senha;

    public void ChecarCadastro()
    {
        StartCoroutine(ProcuraUsuarioNoBackEnd(URL));
    }

    private IEnumerator ProcuraUsuarioNoBackEnd(string uri)
    {
        string cadastroJSON = JsonUtility.ToJson(new CadastroJSON(m_email.text,m_senha.text));
        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(uri, cadastroJSON)) //envia nossas informações de login para o servidor
        {
            Debug.Log(cadastroJSON);
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(cadastroJSON);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erro na requisição: " + request.error);
            Debug.LogError("Status Code: " + request.responseCode);
            Debug.LogError("Resposta do Servidor: " + request.downloadHandler.text);
        }
            else
            {
                Debug.Log(request.downloadHandler.text);//Se não deu erro, aqui a gente vê a mensagem
                if(request.responseCode == 200)
                {
                    SceneManager.LoadScene("GamePlayScene");//Mude para o nome para o nome da sua cena em que o jogo está
                }
            }
        }
    }
}

public class CadastroJSON
{
    public string email;
    public string password;

    public CadastroJSON(string newEmail, string newSenha)
    {
        email = newEmail;
        password = newSenha;
    }
}
