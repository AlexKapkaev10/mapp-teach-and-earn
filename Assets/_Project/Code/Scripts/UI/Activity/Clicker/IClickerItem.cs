using System;

namespace Project.Scripts.UI
{
    public interface IClickerItem
    {
        event Action ClickItem;
        void ClickAnimation();
    }
}