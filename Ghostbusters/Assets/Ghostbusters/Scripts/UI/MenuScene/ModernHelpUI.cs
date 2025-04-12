using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModernHelpUI : HelpUI
{
    [Header("Modern Components")]
    [SerializeField] private Button _rightButton;
    [SerializeField] private Button _leftButton;
    [SerializeField] private List<GameObject> _slides;

    private int _currentIndex;

    public override void Init()
    {
        base.Init();

        _rightButton.onClick.AddListener(SwipeRight);
        _leftButton.onClick.AddListener(SwipeLeft);
    }

    public override void Show()
    {
        base.Show();
        _currentIndex = 0;

        for (int i = 0; i < _slides.Count; i++)
        {
            _slides[i].SetActive(i == _currentIndex);
        }
    }

    private void SwipeRight()
    {
        _slides[_currentIndex].SetActive(false);
        _currentIndex = (_currentIndex + 1) % _slides.Count;
        _slides[_currentIndex].SetActive(true);
    }

    private void SwipeLeft()
    {
        _slides[_currentIndex].SetActive(false);
        _currentIndex = (_currentIndex - 1 + _slides.Count) % _slides.Count;
        _slides[_currentIndex].SetActive(true);
    }
}