
public class HealthController
{
    private HealthModel _healthModel;
    private HealthView _healthView;

    public HealthController(HealthModel healthModel, HealthView healthView)
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
