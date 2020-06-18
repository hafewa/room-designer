using System.Collections;
using UnityEngine;

public abstract class State
{
    protected MenuSystem MenuSystem;

    protected State(MenuSystem menuSystem)
    {
        MenuSystem = menuSystem;
    }
    
    public virtual IEnumerator Start()
    {
        yield break;
    }

    public virtual IEnumerator PlaceObj()
    {
        yield break;
    }

    public virtual IEnumerator ChooseObject()
    {
        yield break;
    }

    public virtual IEnumerator ChangeColor()
    {
        yield break;
    }

    public virtual IEnumerator PressTrigger()
    {
        yield break;
    }
    
    public virtual IEnumerator PressBack()
    {
        yield break;
    }
    
    public virtual IEnumerator ObjectBtnClicked(GameObject objPrefab)
    {
        yield break;
    }
    
    public virtual IEnumerator MaterialBtnClicked(Material material)
    {
        yield break;
    }
    
    public virtual IEnumerator CloseBtnClicked()
    {
        yield break;
    }

    public virtual IEnumerator Move()
    {
        yield break;
    }
    
    public virtual IEnumerator WallClicked(GameObject wall)
    {
        yield break;
    }
    
    public virtual IEnumerator EditBtnClicked()
    {
        yield break;
    }
}