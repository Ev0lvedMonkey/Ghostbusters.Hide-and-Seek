
public class GhostbusterHealthController
{
    private GhostbusterHealthModel _healthModel;
    private GhostbusterHealthView _healthView;

    public GhostbusterHealthController(GhostbusterHealthModel healthModel, GhostbusterHealthView healthView)
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
