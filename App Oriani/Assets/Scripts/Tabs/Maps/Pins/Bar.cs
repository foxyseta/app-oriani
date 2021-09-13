public class Bar : Pin
{
    public TabButton barTabButton;

    protected override void Interact()
    {
        barTabButton.OnClick();
    }
}
