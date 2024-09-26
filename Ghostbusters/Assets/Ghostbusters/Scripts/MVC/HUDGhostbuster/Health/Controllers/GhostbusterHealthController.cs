using UnityEngine;
using Mirror;

public class GhostbusterHealthController
{
    private GhostbusterHealthModel _healthModel;
    private GhostbusterHealthView _healthView;

    public GhostbusterHealthController(GhostbusterHealthModel healthModel, GhostbusterHealthView healthView)
    {
        _healthModel = healthModel;
        _healthView = healthView;

        RpcUpdateView();
    }

    public void TakeDamage()
    {
        _healthModel.TakeDamage();
        RpcUpdateView();
    }

    private void RpcUpdateView()
    {
        _healthView.UpdateHealthBar(_healthModel.GetHealthPercentage(), _healthModel.CurrentHealth);
    }
}
