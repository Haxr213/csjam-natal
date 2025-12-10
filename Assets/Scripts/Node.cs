using UnityEngine;
using UnityEngine.SceneManagement;


public class Node : MonoBehaviour
{
    public static Node selectedNode;
    private Animator anim;
    public bool isSelected = false;
    public bool isOcuped = false;
    public Tower towerOcuped;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void AnimNode()
    {
        if (selectedNode && selectedNode != this)
        {
            selectedNode.OnCloseSelection();
        }

        selectedNode = this;
        isSelected = !isSelected;
        if (isSelected)
            TowerRequestManager.OpenRequestPanel();
        else
            TowerRequestManager.CloseRequestPanel();
        anim.SetBool("IsSelected", isSelected);
    }

    public void OnCloseSelection()
    {
        isSelected = false;
        anim.SetBool("IsSelected", isSelected);
    }
}
