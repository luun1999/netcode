using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollVertical : MonoBehaviour
{
    [SerializeField] RectTransform rootRectTransform;
    [SerializeField] Button m_LoadMoreButton;
    [SerializeField] Button m_ShortenedButton;
    [SerializeField] GameObject[] listGameObject;
    bool m_isLoadMore = false;

    private void Awake()
    {
        m_LoadMoreButton.onClick.AddListener(() => SetLoadMore(true));
        m_ShortenedButton.onClick.AddListener(() => SetLoadMore(false));
    }

    private void Start()
    {
        UpdateButton();
    }

    public void SetLoadMore(bool isLoadMore)
    {
        m_isLoadMore = isLoadMore;
        UpdateButton();
        StartCoroutine(UpdateCanvas());
    }

    private void UpdateButton()
    {
        if (m_isLoadMore)
        {
            for (int i = 0; i < listGameObject.Length; i++)
            {
                listGameObject[i].gameObject.SetActive(true);
            }
            m_LoadMoreButton.gameObject.SetActive(false);
            m_ShortenedButton.gameObject.SetActive(true);
        }
        else
        {
            for (int i = 3; i < listGameObject.Length; i++)
            {
                listGameObject[i].gameObject.SetActive(false);
            }
            m_LoadMoreButton.gameObject.SetActive(true);
            m_ShortenedButton.gameObject.SetActive(false);
        }
    }

    private IEnumerator UpdateCanvas()
    {
        yield return new WaitForEndOfFrame();
        LayoutRebuilder.MarkLayoutForRebuild(this.transform as RectTransform);
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    private void RefreshTransform()
    {
        RectTransform rectTransform = (RectTransform)transform;
        RefreshTransform(rectTransform);
    }

    private void RefreshTransform(RectTransform transform)
    {

    }
}
