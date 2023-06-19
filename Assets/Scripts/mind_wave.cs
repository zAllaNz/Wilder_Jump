using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mind_wave : MonoBehaviour
{
    private MindwaveDataModel m_MindwaveData;
    public Text TMPText;
    public string Status;
    public static string sStatus;
    public float Signal;
    public float Attention, Meditation, Delta, Theta;
    public float LowAlpha, HighAlpha, LowBeta, HighBeta, LowGamma, HighGamma, EEGValue, BlinkStrength;
    public static float sAttention, sMeditation, sDelta, sTheta, sSignal;
    public static float sLowAlpha, sHighAlpha, sLowBeta, sHighBeta, sLowGamma, sHighGamma, sEEGValue, sBlinkStrength;
    private int m_EEGValue;
    private int m_BlinkStrength;
    public bool control = false;
    public bool conectado;
    void Start()
    {
    }
    void Update()
    {
        if(control)
        {
            MindwaveManager.Instance.Controller.OnUpdateMindwaveData += OnUpdateMindwaveData;
            Connect();
        }
        
        sStatus = Status = m_MindwaveData.status;
        sSignal = Signal = m_MindwaveData.poorSignalLevel;
        sAttention = Attention = m_MindwaveData.eSense.attention;
        sMeditation = Meditation = m_MindwaveData.eSense.meditation;
        sDelta = Delta = m_MindwaveData.eegPower.delta;
        sTheta = Theta = m_MindwaveData.eegPower.theta;
        sLowAlpha = LowAlpha = m_MindwaveData.eegPower.lowAlpha;
        sHighAlpha = HighAlpha = m_MindwaveData.eegPower.highAlpha;
        sLowBeta = LowBeta = m_MindwaveData.eegPower.lowBeta;
        sHighBeta = HighBeta = m_MindwaveData.eegPower.highBeta;
        sLowGamma = LowGamma = m_MindwaveData.eegPower.lowGamma;
        sHighGamma = HighGamma = m_MindwaveData.eegPower.highGamma;
        sEEGValue = EEGValue = m_EEGValue;
        sBlinkStrength = BlinkStrength = m_BlinkStrength;
    }

    public void OnUpdateMindwaveData(MindwaveDataModel _Data)
    {
        m_MindwaveData = _Data;
    }
    public void OnUpdateRawEEG(int _EEGValue)
    {
        m_EEGValue = _EEGValue;
    }
    public void OnUpdateBlink(int _BlinkStrength)
    {
        m_BlinkStrength = _BlinkStrength;
    }
    public void Connect()
    {
        if (m_MindwaveData.eegPower.delta > 0)
        {
            TMPText.text = "Connected";
            conectado = true;
            control = true;
        }
        else
        {
            conectado = false;
            control = false;
        }

        if (MindwaveController.isTimeout)
        {
            TMPText.text = "Can't connect";
            conectado = false;
            control = false;
        }
    }
    public void RetryConnection()
    {
        MindwaveManager.Instance.Controller.Connect();
        MindwaveController.isTimeout = false;
        TMPText.text = "Retry Connection";
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}