using UnityEngine;
using UnityEngine.UI;

public class SettingInformation : MonoBehaviour
{
    /// <summary>
    /// ������
    /// </summary>
    #region �������̕ύX
    [Header("������")]
    public Text widDirectionText;
    [HideInInspector]
    public int windDirection = 0;   

    /// <summary>
    /// �e�L�X�g�̑��
    /// </summary>
    public void Direction()
    {
        switch (windDirection)
        {
            case 0:
                widDirectionText.text = "����";
                wind.windless();
                break;
            case 1:
                widDirectionText.text = "�E����";
                wind.rightwind();
                break;
            case 2:
                widDirectionText.text = "������";
                wind.leftwind();
                break;
            case 3:
                widDirectionText.text = "�����";
                wind.upwind();
                break;
            case 4:
                widDirectionText.text = "������";
                wind.downwind();
                break;
            case 5:
                widDirectionText.text = "�E�����";
                wind.upperright();
                break;
            case 6:
                widDirectionText.text = "�E������";
                wind.lowerright();
                break;
            case 7:
                widDirectionText.text = "�������";
                wind.upperleft();
                break;
            case 8:
                widDirectionText.text = "��������";
                wind.lowerleft();
                break;
            case 9:
                widDirectionText.text = "�����_��";
                wind.randomwind();
                break;
        }
    }
    #endregion

    /// <summary>
    /// �������̋���
    /// </summary>
    #region �������̋����ύX
    [Header("�������̋���")]
    public Text windPowerText;
    [HideInInspector]
    public int windPower = 0;

    /// <summary>
    /// �e�L�X�g�̑��
    /// </summary>
    public void Power()
    {
        switch (windPower)
        {
            case 0:
                windPowerText.text = "�f�t�H���g";
                wind.windvaluehold = 0;
                break;
            case 1:
                windPowerText.text = "����";
                wind.windvaluehold = 0.3f;
                break;
            case 2:
                windPowerText.text = "�ア";
                wind.windvaluehold = 0.2f;
                break;
            case 3:
                windPowerText.text = "�����_��";
                wind.randomwind();
                break;
        }
    }
    #endregion

    /// <summary>
    /// �򗈕��̎��
    /// </summary>
    #region �򗈕��̎�ނ̕ύX
    [Header("�򗈕��̎��")]
    public Text enemyTypeText;
    [HideInInspector]
    public int enemyType = 0;

    /// <summary>
    /// �e�L�X�g�̑��
    /// </summary>
    public void Type()
    {
        switch (enemyType)
        {
            case 0:
                enemyTypeText.text = "�f�t�H���g";
                break;
            case 1:
                enemyTypeText.text = "�U���̂�";
                break;
            case 2:
                enemyTypeText.text = "��U���̂�";
                break;
        }
    }
    #endregion

    /// <summary>
    /// �G�̑���
    /// </summary>
    #region �G�̑����̕ύX
    [Header("�G�̑���")]
    public Text enemySpeedText;
    [HideInInspector]
    public int enemySpeed = 0;
    public static float _enemySpeed2 = 0;
    /// <summary>
    /// �e�L�X�g�̑��
    /// </summary>
    public void Speed()
    {
        switch (enemySpeed)
        {
            case 0:
                enemySpeedText.text = "�f�t�H���g";
                _enemySpeed2 = 0;
                break;
            case 1:
                enemySpeedText.text = "����";
                _enemySpeed2 = 6;

                break;
            case 2:
                enemySpeedText.text = "�x��";
                _enemySpeed2 = 2;
                break;
            case 3:
                enemySpeedText.text = "����";
                _enemySpeed2 = 4;
                break;
        }
    }
    #endregion

    /// <summary>
    /// �G�̐�
    /// </summary>
    #region �G�̐��̕ύX
    [Header("�G�̐�")]
    public Text enemyNumText;
    [Header("�G�̐�2")]
    public Text enemyNumText2;
    [HideInInspector]
    public int enemyNum = 0;
    [HideInInspector]
    public int enemyNum2 = 0;

    /// <summary>
    /// �e�L�X�g�̑��
    /// </summary>
    public void Num()
    {
        switch (enemyNum)
        {
            case 0:
                enemyNumText.text = "1";
                break;
            case 1:
                enemyNumText.text = "2";
                break;
            case 2:
                enemyNumText.text = "3";
                break;
            case 3:
                enemyNumText.text = "4";
                break;
            case 4:
                enemyNumText.text = "5";
                break;
        }
    }
    public void Num2()
    {
        switch (enemyNum2)
        {
            case 0:
                enemyNumText2.text = "1";
                break;
            case 1:
                enemyNumText2.text = "2";
                break;
            case 2:
                enemyNumText2.text = "3";
                break;
            case 3:
                enemyNumText2.text = "4";
                break;
            case 4:
                enemyNumText2.text = "5";
                break;
        }
    }
    #endregion

    /// <summary>
    /// �G�̍ďo������
    /// </summary>
    /// 
    #region �G�̍ďo�����Ԃ̐ݒ�
    [Header("�G�̍ďo������")]
    public Text enemyRepopTimeText;
    [Header("�G�̍ďo������2")]
    public Text enemyRepopTimeText2;
    [HideInInspector]
    public int enemyRepopTime = 0;
    [HideInInspector]
    public int enemyRepopTime2 = 0;
    /// <summary>
    /// �e�L�X�g�̑��
    /// </summary>
    public void Repop_time()
    {
        switch (enemyRepopTime)
        {
            case 0:
                enemyRepopTimeText.text = "3";
                break;
            case 1:
                enemyRepopTimeText.text = "4";
                break;
            case 2:
                enemyRepopTimeText.text = "5";
                break;
            case 3:
                enemyRepopTimeText.text = "6";
                break;
            case 4:
                enemyRepopTimeText.text = "7";
                break;
            case 5:
                enemyRepopTimeText.text = "8";
                break;
            case 6:
                enemyRepopTimeText.text = "9";
                break;
            case 7:
                enemyRepopTimeText.text = "10";
                break;
        }
    }
    public void Repop_time2()
    {
        switch (enemyRepopTime2)
        {
            case 0:
                enemyRepopTimeText2.text = "3";
                break;
            case 1:
                enemyRepopTimeText2.text = "4";
                break;
            case 2:
                enemyRepopTimeText2.text = "5";
                break;
            case 3:
                enemyRepopTimeText2.text = "6";
                break;
            case 4:
                enemyRepopTimeText2.text = "7";
                break;
            case 5:
                enemyRepopTimeText2.text = "8";
                break;
            case 6:
                enemyRepopTimeText2.text = "9";
                break;
            case 7:
                enemyRepopTimeText2.text = "10";
                break;
        }
    }
    #endregion

    /// <summary>
    /// ���Z�b�g
    /// </summary>

    public Wind wind;
    public void Reset()
    {
        #region ������
        windDirection = 0;
        windPower = 0;
        enemyType = 0;
        enemySpeed = 0;
        enemyNum = 0;
        enemyNum2 = 4;
        enemyRepopTime = 0;
        enemyRepopTime2 = 7;
        #endregion

        #region ���f
        Direction();
        Power();
        Type();
        Speed();
        Num();
        Repop_time();
        #endregion
    }
}
