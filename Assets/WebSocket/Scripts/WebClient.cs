using System.Collections;
using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class WebClient : MonoBehaviour
{
    private const string URL = "ws://localhost:7272"; // 필요에 맞게 URL 수정, WS로 웹소캣
    
    public int width = 640;
    public int height = 480;
    public RawImage webcamImage;
    public RawImage receivedImage;
    
    private WebCamTexture webcamTexture;
    private RenderTexture renderTexture;
    private Texture2D captureTexture;
    private Texture2D receivedTexture;

    private WebSocket websocket;
    
    private ConcurrentQueue<byte[]> receivedDataQueue = new();

    private void Start()
    {
        CreateInstance();// 웹소켓을 통한 실시간 웹캠 스트리밍 시스템의 기반
        CreateWebSocket();// WebSocket 연결을 설정하고 관리
        
        StartCoroutine(SendProcess());
        StartCoroutine(ReceiveProcess());
        StartCoroutine(ReconnectProcess());
    }

    private void CreateInstance()
    {
        // 웹캠 텍스쳐를 지정된 너비와 높이로 생성
        webcamTexture = new WebCamTexture(width, height);
        // `Play()`를 호출하여 웹캠 캡처 시작
        webcamTexture.Play();
        
        // 웹캠 이미지를 표시를 위한 `RenderTexture`생성 
        renderTexture = new RenderTexture(width, height, 0);
        // `renderTexture`를 UI의 `RawImage`에 연결
        webcamImage.texture = renderTexture;
        
        // 'captureTexture' 웹캠 이미지를 인코딩하기 위한 '텍스처2D' 생성
        captureTexture = new Texture2D(width, height, TextureFormat.RGB24, false);
        // 'receivedTexture' 수신된 이미지를 표시하기 위한 '텍스처2D' 생성
        receivedTexture = new Texture2D(width, height, TextureFormat.RGB24, false);
        // 'receivedImage'를 UI의 'RawImage'에 연결   
        receivedImage.texture = receivedTexture;
    }

    private void CreateWebSocket()
    {
        // URL을 사용하여 WebSocket 인스턴스를 생성
        websocket = new WebSocket(URL);
        // WebSocket 연결 성공 시 실행되는 이벤트 핸들러
        websocket.OnOpen += (sender, e) => {
            Debug.Log("WebSocket Connected");
            StartCoroutine(SendProcess());
        };
        // WebSocket 연결 종료 시 실행되는 이벤트 핸들러
        websocket.OnClose += (sender, e) => {
            Debug.LogWarning("WebSocket Disconnected: " + e.Reason);
        };
        // 메시지 수신 시 실행되는 이벤트 핸들러
        websocket.OnMessage += (sender, e) => {
            if (e.IsBinary)
            {
                OnReceiveTexture(e.RawData);
            }
        };
        // WebSocket 비동기 연결 시작
        websocket.ConnectAsync();
    }

    private void OnReceiveTexture(byte[] bytes)
    {
        Debug.Log($"Receive Texture : {bytes.Length}");
        receivedDataQueue.Enqueue(bytes);
    }

    private IEnumerator SendProcess()
    {
        while (true)
        {
            // webcamTexture가 업데이트 될 때까지 대기
            yield return new WaitUntil(() => webcamTexture.didUpdateThisFrame);

            // webcamTexture를 renderTexture로 복사, - `Blit`은 GPU를 사용하여 효율적으로 텍스처를 복사
            Graphics.Blit(webcamTexture, renderTexture);

            if (websocket.IsAlive) // - 웹소켓 연결이 활성화 상태인지 확인
            {
                // RenderTexture 내용을 captureTexture로 복사
                RenderTexture.active = renderTexture;
                // ReadPixels로 지정된 영역의 픽셀을 읽어와 captureTexture에 저장
                captureTexture.ReadPixels(new Rect(0, 0, 640, 480), 0, 0);
                // 변경사항 적용
                captureTexture.Apply();
                // RenderTexture.active를 null로 설정하여 활성화된 RenderTexture를 해제
                RenderTexture.active = null;
                
                // jpg로 인코딩 후 전송
                var bytes = captureTexture.EncodeToJPG(70);
                if (bytes != null)
                {
                    Debug.Log($"Send Texture : {bytes.Length}");
                    websocket.Send(bytes);
                }
            }

            yield return null;
        }
    }

    private IEnumerator ReceiveProcess()
    {
        while (true)
        {
            yield return null; // 다음 프레임까지 코루틴 실행을 일시 중단
                               // (Unity의 메인 스레드가 다른 작업을 수행하도록)
            if (!websocket.IsAlive)
                continue;

            while (receivedDataQueue.Count > 0) // 큐에 수신된 데이터가 있는 동안 반복
            {
                if (receivedDataQueue.TryDequeue(out var bytes)) // 큐에서 수신받았던 데이터 추출
                {
                    // 수신된 바이트 배열을 Texture2D로 변환 (PNG 또는 JPG 형식 지원)
                    receivedTexture.LoadImage(bytes);
                    // 수신된 텍스처를 UI의 RawImage에 적용(GPU에서 처리)
                    receivedTexture.Apply();
                }
            }
        }
    }

    private IEnumerator ReconnectProcess()
    {
        var wfs = new WaitForSeconds(1f); // 1초 동안 대기하는 객체를 생성, 이 객체를 매 루프마다 재사용되어 가비지 컬렉션을 최소화

        while (true)
        {
            yield return wfs;

            if (websocket.IsAlive)
                continue;
            
            Debug.Log("Reconnecting...");
            websocket.ConnectAsync(); // WebSocket을 비동기적으로 재연결

        }
    }

    private void OnApplicationQuit()
    {
        if (websocket != null && websocket.IsAlive)
        {
            // 웹소켓 연결이 활성화 상태일 때 정상(Normal) 종료
            websocket.Close(CloseStatusCode.Normal, "Application Quit");
        }

        StopAllCoroutines(); // 해당 MonoBehaviour에서 실행 중인 모든 코루틴을 중지

    }
}