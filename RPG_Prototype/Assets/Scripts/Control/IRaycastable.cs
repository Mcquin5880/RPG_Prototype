namespace RPG.Control
{
    public interface IRaycastable
    {       
        bool HandleRaycast(Player player);
        MouseCursorType GetMouseCursorType();
    }
}

