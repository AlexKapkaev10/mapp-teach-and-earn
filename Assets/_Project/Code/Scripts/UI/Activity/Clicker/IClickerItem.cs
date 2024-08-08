using System;

namespace _Project.Scripts.UI
{
    public interface IClickerItem
    {
        event Action ClickItem;
        void ClickAnimation();
    }
}