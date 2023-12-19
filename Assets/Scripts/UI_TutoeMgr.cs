using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_TutoeMgr : MonoBehaviour
{
    public Text FormattedText;
    private int index = 1;

    private Animator _animator;

    public Animator god;
    // Start is called before the first frame update
    void Start()
    {
        _animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (index)
        {
            case 5:
                _animator.Play("Ape");
                break;
            case 8:
                _animator.Play("Emo");
                break;
            case 11:
                _animator.Play("Card");
                break;
            case 12:
                _animator.Play("Pressure");
                break;
        }
    }
    
    /// <summary>
    /// Play Dialog From .csv file
    /// </summary>
    public void PlayDialog()
    {
        index = (index < 16) ? index + 1 : index;
        god.Play((Random.Range(0, 2) == 0) ? "Teach" : "Speak");
        FormattedText.text = LocalizationManager.Localize("Tutor.Log.Log" + index.ToString());
    }

    public void ExitToTitle()
    {
        index = 0;
        SceneManager.LoadScene(0);
    }
}
