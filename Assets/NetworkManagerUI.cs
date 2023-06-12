using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button _serverButton;
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _clientButton;
    [SerializeField] private Canvas _canvas;


    private void Awake()
    {
        _serverButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
            _canvas.gameObject.SetActive(false);
        });
        _hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            _canvas.gameObject.SetActive(false);
        });
        _clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            _canvas.gameObject.SetActive(false);
        });
    }
}
