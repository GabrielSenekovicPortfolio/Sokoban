using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The Gemstone Podium is a script that interacts with the Gemstone to cause effects within the puzzle
 * If all of them are activated, then the Win Point is activated
 * If the scope had been bigger, I could justify an "IPressable" for, perhaps, 
 * buttons that get permanently pressed even if the Gemstone is removed
 */
public class GemstonePodium : MonoBehaviour
{
    [SerializeField] ColorCode color;
    [SerializeField] int score;

    Gemstone currentGemstone;
    public void Start()
    {
        ProgressionManager.Instance.AddPodiums(this);
    }
    private void Update()
    {
        if(currentGemstone != null && (currentGemstone.transform.position - transform.position).magnitude < 0.5f)
        {
            currentGemstone.Activate();
            ProgressionManager.Instance.ActivatePodium(color);
            currentGemstone.transform.position = transform.position;
            currentGemstone = null;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Gemstone gemstone) && gemstone.GetColorCode() == color)
        {
            currentGemstone = gemstone;
            ScoreManager.Instance.AddScore(score);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(currentGemstone == collision.gameObject)
        {
            currentGemstone = null;
        }
    }
}