using UnityEngine;

public class GhostHealthController 
{
    private GhostHealthModel _healthModel;
    private GhostHealthView _healthView;

    public GhostHealthController(GhostHealthModel healthModel, GhostHealthView healthView)
    {
        _healthModel = healthModel;
        _healthView = healthView;

        UpdateView();
    }

    public void TakeDamage()
    {
        _healthModel.TakeDamage();
        UpdateView();
    }

    private void UpdateView()
    {
        _healthView.UpdateHealthBar(_healthModel.GetHealthPercentage(), _healthModel.CurrentHealth);
    }
}
